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
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Thought;
using ViewCommon.Utils;
using ArrayBuilder = DataStructures.ArrayBuilder;

namespace Icarus.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        public StatsViewModel Stats { get; set; }
        public LineSeries mySeries { get; set; }
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
            MyResults.Series.Add(mySeries);
            MyResults.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Vertical, X = 0 });
        }

        double capital = 7000;
        double results = 0;
        public void Update(Trade myTrade) {

            Stats.UpdateStats(myTrade);

            results += myTrade.FinalResult;
            //Trace.Write(myTrade.FinalResult);

            Application.Current.Dispatcher.Invoke(() => {
                //for (int i = 0; i < myTrade.ResultArray.Length; i++) {
                //    var resulto = capital * (myTrade.ResultArray[i] + 1);

                //    mySeries.Points.Add(new DataPoint(mySeries.Points.Count + 1, resulto));

                //}
                capital += (myTrade.FinalResult+1) *((capital*0.02));
                mySeries.Points.Add(new DataPoint(mySeries.Points.Count + 1, results));
                MyResults.Axes.First(x => x.Tag == "xaxis").Maximum = mySeries.Points.Count + 5;

                
            });
            MyResults.InvalidatePlot(true);
            NotifyPropertyChanged($"MyResults");
        }

        public void Start() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Dowork));
        }

        private void Dowork(object callback) {
            ArrayBuilder.Callback = Update;

            Universe myunivers = new Universe();
            var stocks = Markets.ASX200();
            for(int i = 0; i< stocks.Count(); i++ ){
                try {
                    var stock = new Market(stocks[i]);
                    var stratto = new StaticStrategy.StrategyBuilder().
                        CreateStrategy(new IRuleSet[] { new ATRContraction(), }, stock);

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

            var backTest = new Backtest(myunivers);
            backTest.RunBackTest(new LongStrategyExecuter(false));


        }

    }
}
