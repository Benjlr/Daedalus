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
        public PlotModel LongRollingExp { get; set; }
        public PlotModel ShortRollingExp { get; set; }
        public PlotModel LongDrawdowns { get; set; }
        public PlotModel ShortDrawdowns { get; set; }
        private PlaceholderName myTests { get; set; }
        
        public DetailedFixBarExits()
        {
            var market = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var strat = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new ATRContraction(), 
            }, market);

            myTests = new PlaceholderName();
            myTests.GenerateFixedBarResults(TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(5,220,1)));

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownLong, myTests.X_label_categorised, myTests.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownShort, myTests.X_label_categorised, myTests.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTests.ReturnByFbeLong, myTests.X_label, myTests.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTests.ReturnByFbeShort, myTests.X_label, myTests.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTests.DrawdownByFbeLong, myTests.X_label_categorised, myTests.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTests.DrawdownByFbeShort, myTests.X_label_categorised, myTests.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTests.ExpectancyLongMed, myTests.ExpectancyLongAvg);
            ExpectancyShort = Series.GenerateExpectanySeries(myTests.ExpectancyShortMed, myTests.ExpectancyShortAvg);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTests.RollingExpectancyLong));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTests.RollingExpectancyShort));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTests.DrawdownByFbeLong));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTests.DrawdownByFbeShort));

            //PlotModelDrawdownLong.InvalidatePlot(true);
            //PlotModelDrawdownShort.InvalidatePlot(true);
            //PlotModelReturnsLong.InvalidatePlot(true);
            //PlotModelReturnsShort.InvalidatePlot(true);
            //ExpectancyLong.InvalidatePlot(true);
            //ExpectancyShort.InvalidatePlot(true);

        }


    }
}
