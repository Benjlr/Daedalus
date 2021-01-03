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
using DataStructures.PriceAlgorithms;
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



        public void Update(Trade myTrade) {

            Application.Current.Dispatcher.Invoke(() => {
                mySeries.Points.Add(new DataPoint(mySeries.Points.Count + 1, myPortfolio.Cash ));
                //mySeries2.Points.Add(new DataPoint(mySeries2.Points.Count + 1, myPortfolio.CurrentExposure.Count));
                MyResults.Axes.First(x => x.Tag == "xaxis").Maximum = mySeries.Points.Count + 5;
            });
            Stats.UpdateStats(myTrade);
            MyResults.InvalidatePlot(true);
            NotifyPropertyChanged($"MyResults");
        }

        public void Start() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Dowork));
        }

        private Portfolio myPortfolio;
        private void Dowork(object callback) {
            TradeCompiler.Callback = Update;
            myPortfolio = new Portfolio(7000,0.03, false);
            Universe myunivers = new Universe();
            var stocks = Markets.AllASX();
            for (int i = 0; i < stocks.Count(); i++) {
                try {


                    var stock = new Market(stocks[i]);
                    if (LiquidityFilter.IsLiquid(stock.PriceData.Select(x => x.Close.Mid).ToList(), stock.PriceData.Select(x => x.Volume).ToList())) {

                        var stratto = new StaticStrategy.StrategyBuilder().
                            CreateStrategy(new IRuleSet[] { new PivotPoint() }, stock,
                                new TwentyMAViolation(new ExitPrices(0.93, 3), MarketSide.Bull));

                        myunivers.AddMarket(stock, stratto);
                    }

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

            var backTest = new Backtest(myunivers, MarketSide.Bull, myPortfolio);
            backTest.RunBackTestByDates();
        }

    }
}
