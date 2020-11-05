using Daedalus.Charts;
using Daedalus.Models;
using Daedalus.Utils;
using Logic;
using Logic.Analysis;
using Logic.Metrics;
using Logic.Rules;
using Logic.Rules.Entry;
using Logic.Utils;
using OxyPlot;
using OxyPlot.Axes;
using System.Linq;
using Logic.Analysis.Metrics;

namespace Daedalus.ViewModels
{
    public class DetailedFixBarExits : ViewModelBase
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

        public DetailedFixBarExits()
        {
            var tests = TestFactory.GenerateFixedBarExitTest(ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket,
                new FixedBarExitTestOptions(5, 220, 1));

            var myTestsLong = new AnalysisBuilder();
            var myTestsShort = new AnalysisBuilder();
            myTestsLong.GenerateFixedBarResults(tests.Select(x=>x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x=>x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByFbe, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByFbe, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByFbe, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByFbe, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByFbe));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByFbe));

        }


    }

    public class DetailedRandomExits : ViewModelBase
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

        public DetailedRandomExits()
        {
            var tests = TestFactory.GenerateRandomExitTests(
                ModelSingleton.Instance.MyStrategy,
                ModelSingleton.Instance.Mymarket, 200, 200);

            var myTestsLong = new AnalysisBuilder();
            var myTestsShort = new AnalysisBuilder();
            myTestsLong.GenerateFixedBarResults(tests.Select(x => x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByFbe, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByFbe, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByFbe, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByFbe, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByFbe));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByFbe));

        }

    }
    public class DetailedFixStopExits : ViewModelBase
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

        public DetailedFixStopExits()
        {
            var tests = TestFactory.GenerateFixedStopTargetExitTest(
                ModelSingleton.Instance.MyStrategy,
                ModelSingleton.Instance.Mymarket,
                new FixedStopTargetExitTestOptions(0.04 / 10.0, 0.04 / 10.0, 0.008, 500));

            var myTestsLong = new AnalysisBuilder();
            var myTestsShort = new AnalysisBuilder();
            myTestsLong.GenerateFixedBarResults(tests.Select(x => x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByFbe, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByFbe, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByFbe, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByFbe, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByFbe));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByFbe));

        }

    }
}
