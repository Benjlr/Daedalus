using DataStructures;
using DataStructures.StatsTools;
using Logic;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using RuleSets;
using RuleSets.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using Thought;
using ViewCommon.Utils;

namespace Icarus.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        public StatsViewModel Stats { get; set; }
        public LineSeries mySeries { get; set; }
        public LineSeries mySeries2 { get; set; }
        public PlotModel MyResults { get; set; }
        private ICommand _clickCommand;
        public ICommand ClickCommand => _clickCommand ??= new Commandler(Start, () => CanExecute);
        public bool CanExecute => true;

        public StrategyViewModel()
        {
            Stats = new StatsViewModel();
            MyResults = new PlotModel();
            MyResults.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 10,
                Tag = "xaxis"

            });
            MyResults.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });
            mySeries = new LineSeries()
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
            };

            mySeries2 = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
            };
            MyResults.Series.Add(mySeries);
            MyResults.Series.Add(mySeries2);
            MyResults.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Vertical, X = 0 });
        }

        double capital = 0;
        double risk = 7000*0.02;
        double results = 0;
        double allResults = 0;
        List<Trade> tardeos = new List<Trade>();

        public void Update(Trade myTrade) {
            List<double> stats2 = new List<double>();

            var tradeStart = myTrade.ResultTimeline[0].Date;
            var amount = 20;
            var goodAmount = 0.3;
            var cutoffAmount = 15;

            var validTardes = tardeos.Where(x => x.ResultTimeline.Any(y => y.Date < tradeStart) && x.ResultTimeline.Any(y => y.Date > tradeStart)).ToList();
            var actualLast5 = validTardes.Skip(validTardes.Count - amount).ToList().OrderBy(x => x.ResultTimeline[^1].Date).ToList();
            for (int i = 0; i < actualLast5.Count; i++) {
                stats2.Add(actualLast5[i].ResultTimeline.Last(x => x.Date < tradeStart).Return);
            }



            tardeos.Add(myTrade);
            //allResults += myTrade.FinalResult;
            if (actualLast5.Count >= cutoffAmount) {
                if (stats2.Count(x=>x>0) / (double)stats2.Count > goodAmount) {
                    Stats.UpdateStats(myTrade);
                    results += myTrade.FinalResult;
                    capital += (600) * myTrade.FinalResult;
                }
            }
            allResults += (600) * myTrade.FinalResult;


            Application.Current.Dispatcher.Invoke(() => {
                mySeries.Points.Add(new DataPoint(mySeries.Points.Count + 1, capital));
                mySeries2.Points.Add(new DataPoint(mySeries2.Points.Count + 1, allResults));
                MyResults.Axes.First(x => x.Tag == "xaxis").Maximum = mySeries.Points.Count + 5;

                
            });
            MyResults.InvalidatePlot(true);
            NotifyPropertyChanged($"MyResults");
        }

        public void Start() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Dowork));
        }

        private void Dowork(object callback) {
            TradeCompiler.Callback = Update;

            Universe myunivers = new Universe();
            var stocks = Markets.ASX200();
            for (int i = 0; i < stocks.Count(); i++) {
                try {
                    var stock = new Market(stocks[i]);
                    var stratto = new StaticStrategy.StrategyBuilder().
                        CreateStrategy(new IRuleSet[] { new PivotPoint() ,   }, stock,
                            new TrailingStopPercentage(new ExitPrices(0.93, 2), 0.15));

                    myunivers.AddMarket(stock, stratto);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    //throw;
                }


            }


            //Market aumarket = new Market(Markets.asx200_cash_5);
            //Market audUdsd = new Market(Markets.aud_usd_5);
            //Market bitcoin = new Market(Markets.bitcoin_5);
            //Market sp500 = new Market(Markets.sp500_cash_5);

            //var strataumarket = new StaticStrategy.StrategyBuilder().
            //    CreateStrategy(new IRuleSet[] { new ATRContraction(), }, aumarket,
            //        new TrailingStopPercentage(new ExitPrices(0.95, 1.2), 0.01));
            //var stratAudUSd = new StaticStrategy.StrategyBuilder().
            //    CreateStrategy(new IRuleSet[] { new ATRContraction(), }, audUdsd,
            //        new TrailingStopPercentage(new ExitPrices(0.98, 1.2), 0.0001));
            //var stratBitcoin = new StaticStrategy.StrategyBuilder().
            //    CreateStrategy(new IRuleSet[] { new ATRContraction(), }, bitcoin,
            //        new TrailingStopPercentage(new ExitPrices(0.98, 1.2), 0.0001));
            //var stratSp500 = new StaticStrategy.StrategyBuilder().
            //    CreateStrategy(new IRuleSet[] { new ATRContraction(), }, sp500,
            //        new TrailingStopPercentage(new ExitPrices(0.98, 1.2), 0.0001));

            //myunivers.AddMarket(aumarket, strataumarket);
            //myunivers.AddMarket(audUdsd, stratAudUSd);
            //myunivers.AddMarket(bitcoin, stratBitcoin);
            //myunivers.AddMarket(sp500, stratSp500);

            var backTest = new Backtest(myunivers, MarketSide.Bull, false);
            var trades = backTest.RunBackTestByDates();
        }

    }
}
