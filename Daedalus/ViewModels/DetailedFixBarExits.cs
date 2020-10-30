using Daedalus.Charts;
using Daedalus.Utils;
using Logic;
using Logic.Analysis;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Utils;
using OxyPlot;
using OxyPlot.Axes;

namespace Daedalus.ViewModels
{
    public class DetailedFixBarExits:ViewModelBase
    {
        public PlotModel PlotModelDrawdownLong { get; set; }
        public PlotModel PlotModelDrawdownShort { get; set; }
        public PlotModel PlotModelReturnsLong { get; set; }
        public PlotModel PlotModelReturnsShort { get; set; }
        public PlotModel PlotModelDDsLong { get; set; }
        public PlotModel PlotModelDDsShort { get; set; }
        public PlotModel ExpectancyLong { get; set; }
        public PlotModel ExpectancyShort { get; set; }
        public PlotModel testone { get; set; }
        public PlotModel testtwo { get; set; }
        private PlaceholderName myTests { get; set; }
        
        public DetailedFixBarExits()
        {
            var market = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var strat = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new ATRContraction(), 
            }, market);

            myTests = new PlaceholderName();
            myTests.GenerateFixedBarResults(TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(50,250,10)));

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownLong, myTests.X_label_categorised, myTests.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownShort, myTests.X_label_categorised, myTests.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTests.ReturnByFbeLong, myTests.X_label, myTests.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTests.ReturnByFbeShort, myTests.X_label, myTests.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTests.DrawdownByFbeLong, myTests.X_label_categorised, myTests.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTests.DrawdownByFbeShort, myTests.X_label_categorised, myTests.Y_label);
            ExpectancyLong = Series.GenerateSeries(myTests.ExpectancyLongMed, myTests.ExpectancyLongAvg);
            ExpectancyShort = Series.GenerateSeries(myTests.ExpectancyShortMed, myTests.ExpectancyShortAvg);
            testone = Series.GenerateSeries(myTests.ExpByPlaceInSeriesLong);
            testtwo = Series.GenerateSeries(myTests.ExpByPlaceInSeriesShort);

            //PlotModelDrawdownLong.InvalidatePlot(true);
            //PlotModelDrawdownShort.InvalidatePlot(true);
            //PlotModelReturnsLong.InvalidatePlot(true);
            //PlotModelReturnsShort.InvalidatePlot(true);
            //ExpectancyLong.InvalidatePlot(true);
            //ExpectancyShort.InvalidatePlot(true);

        }


    }
}
