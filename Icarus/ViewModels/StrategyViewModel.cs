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

        double capital = 7000;
        double results = 0;
        double allResults = 0;
        List<Trade> tardeos = new List<Trade>();
        List<double> stats = new List<double>();

        public void Update(Trade myTrade) {

            tardeos.Add(myTrade);
            allResults += myTrade.FinalResult;
            if (stats.Count > 5) {
                var stat = new TradeStatistics(stats.Skip(stats.Count - 5).ToList());
                if (stat.AverageExpectancy > 0) {
                    Stats.UpdateStats(myTrade);
                    results += myTrade.FinalResult;
                }
            }

            stats.Add(myTrade.FinalResult);

            Application.Current.Dispatcher.Invoke(() => {
                //capital += (myTrade.FinalResult+1) *((capital*0.02));
                mySeries.Points.Add(new DataPoint(mySeries.Points.Count + 1, results));
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
            var stocks = Markets.ASX300();
            for(int i = 0; i< stocks.Count(); i++ ){
                try {
                    var stock = new Market(stocks[i]);
                    var stratto = new StaticStrategy.StrategyBuilder().
                        CreateStrategy(new IRuleSet[] { new ATRContraction(), }, stock, 
                            new TrailingStopPercentage(new ExitPrices(0.93,1.35),0.15));

                    myunivers.AddMarket(stock, stratto);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    //throw;
                }
          

            }


          //  Market aumarket = new Market(Markets.asx200_cash_5);
          //  Market audUdsd = new Market(Markets.aud_usd_5);
           // Market bitcoin = new Market(Markets.bitcoin_5);
            //Market sp500 = new Market(Markets.sp500_cash_5);

           // var strataumarket = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[] { new InvestorBotEntry(), new ATRContraction(),   }, aumarket);
           // var stratAudUSd = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[] {new InvestorBotEntry(),new ATRContraction(), }, audUdsd);
           // var stratBitcoin = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[] {new InvestorBotEntry(),new ATRContraction(), }, bitcoin);
           // var stratSp500 = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[] {new InvestorBotEntry(),new ATRContraction(), }, sp500);

        //    myunivers.AddMarket(aumarket, strataumarket);
         //   myunivers.AddMarket(audUdsd, stratAudUSd);
          //  myunivers.AddMarket(bitcoin, stratBitcoin);
           // myunivers.AddMarket(sp500, stratSp500);

            //var backTest = new LinearBacktest(myunivers, new LongStrategyExecuter(false));
            //var trades = backTest.RunBackTest();

            var backTest = new Backtest(myunivers, MarketSide.Bull, true);
            var trades = backTest.RunBackTestByDates();
        }

    }
}
