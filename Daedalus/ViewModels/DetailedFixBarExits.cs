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
        //public PlotController ControllerModelDrawdownLong { get; set; }

        public PlotModel PlotModelDrawdownShort { get; set; }
        //public PlotController ControllerModelDrawdownShort { get; set; }

        public PlotModel PlotModelReturnsLong { get; set; }
        //public PlotController ControllerReturnsLong { get; set; }

        public PlotModel PlotModelReturnsShort { get; set; }
        //public PlotController ControllerReturnsShort { get; set; } = new PlotController();

        public PlotModel ExpectancyLong { get; set; }
        //public PlotController controllerLong { get; set; } =  new PlotController();
        public PlotModel ExpectancyShort { get; set; }
        //public PlotController controllerShort { get; set; } = new PlotController();
        private PlaceholderName myTests { get; set; }
        
        public DetailedFixBarExits()
        {
            var market = MarketBuilder.CreateMarket(Markets.ASX200_Cash_5_Min);
            var strat = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new ATRContraction(), 
            }, market);

            myTests = new PlaceholderName();
            myTests.GenerateFixedBarResults(TestFactory.GenerateFixedBarExitTest(1, 500, strat, market));

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownLong, myTests.X_label_categorised, myTests.Y_label_categorised);
            //ControllerModelDrawdownLong = new PlotController();

            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTests.ReturnByDrawdownShort, myTests.X_label_categorised, myTests.Y_label_categorised);
            //ControllerModelDrawdownShort = new PlotController();
            
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTests.ReturnByFbeLong, myTests.X_label, myTests.Y_label);
            //ControllerReturnsLong = new PlotController();
            
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTests.ReturnByFbeShort, myTests.X_label, myTests.Y_label);
            //ControllerReturnsShort = new PlotController();

            ExpectancyLong = Series.GenerateSeries(myTests.ExpectancyLongMed);
            ExpectancyShort = Series.GenerateSeries(myTests.ExpectancyShortMed);

            PlotModelDrawdownLong.InvalidatePlot(true);
            PlotModelDrawdownShort.InvalidatePlot(true);
            PlotModelReturnsLong.InvalidatePlot(true);
            PlotModelReturnsShort.InvalidatePlot(true);
            ExpectancyLong.InvalidatePlot(true);
            ExpectancyShort.InvalidatePlot(true);

        }


    }
}
