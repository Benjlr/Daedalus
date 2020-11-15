using Logic.Analysis;
using Logic.Analysis.Metrics;
using Logic.Utils;
using OxyPlot;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using ViewCommon.Charts;
using ViewCommon.Models;
using ViewCommon.Utils;

namespace Daedalus.ViewModels
{
    public abstract class EntryTestViewModel : ViewModelBase
    {
        public InitTestViewModel LoadStatus { get; set; }
        public Visibility LoadWindowVisibility { get; set; }
        public Visibility ChartVisibility { get; set; }
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
        protected BackgroundWorker _backGroundWorker { get; set; }

        protected EntryTestViewModel()
        {
            LoadStatus = new InitTestViewModel();
            LoadWindowVisibility = Visibility.Visible;
            ChartVisibility = Visibility.Hidden;
            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");

            ThreadPool.QueueUserWorkItem(new WaitCallback(bg_DoWork));
        }
        private void bg_DoWork(object callback)
        {
            var allTests = GenerateEntryTests();
            var myTestsLong = LongAnalysis(allTests.Select(x=>x[0]).ToList());
            var myTestsShort = ShortAnalysis(allTests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsLong.ReturnByDrawdown), myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsShort.ReturnByDrawdown), myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsLong.ReturnByTest), myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsShort.ReturnByTest), myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByTest), myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = Series.GenerateHistogramSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByTest), myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateSeriesVertical(new List<List<double>>(){myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage});
            ExpectancyShort = Series.GenerateSeriesVertical(new List<List<double>>(){ myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage });
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByTest));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByTest));
            

            Application.Current.Dispatcher.Invoke(finished_work);
        }
        private void finished_work()
        {
            LoadWindowVisibility = Visibility.Collapsed;
            ChartVisibility = Visibility.Visible;

            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");

            PlotModelDrawdownLong.InvalidatePlot(false);
            PlotModelDrawdownShort.InvalidatePlot(false);
            PlotModelReturnsLong.InvalidatePlot(false);
            PlotModelReturnsShort.InvalidatePlot(false);
            //PlotModelDDsLong.InvalidatePlot(false);
            //PlotModelDDsShort.InvalidatePlot(false);
            ExpectancyLong.InvalidatePlot(false);
            ExpectancyShort.InvalidatePlot(false);
            LongRollingExp.InvalidatePlot(false);
            ShortRollingExp.InvalidatePlot(false);
            LongDrawdowns.InvalidatePlot(false);
            ShortDrawdowns.InvalidatePlot(false);

            NotifyPropertyChanged($"PlotModelDrawdownLong");
            NotifyPropertyChanged($"PlotModelDrawdownShort");
            NotifyPropertyChanged($"PlotModelReturnsLong");
            NotifyPropertyChanged($"PlotModelReturnsShort");
            NotifyPropertyChanged($"PlotModelDDsLong");
            NotifyPropertyChanged($"PlotModelDDsShort");
            NotifyPropertyChanged($"ExpectancyLong");
            NotifyPropertyChanged($"ExpectancyShort");
            NotifyPropertyChanged($"LongRollingExp");
            NotifyPropertyChanged($"ShortRollingExp");
            NotifyPropertyChanged($"LongDrawdowns");
            NotifyPropertyChanged($"ShortDrawdowns");
        }

        protected abstract List<ITest[]> GenerateEntryTests();
        protected AnalysisBuilder LongAnalysis(List<ITest> longTests)        {
            LoadStatus.UpdateNameAndTotal($"Executing Long Analysis", longTests.Count);
            var myTestsLong = new AnalysisBuilder(LoadStatus.UpdateCount);
            myTestsLong.GenerateFixedBarResults(longTests.ToList());
            return myTestsLong;
        }
        protected AnalysisBuilder ShortAnalysis(List<ITest> shortTests) {
            LoadStatus.UpdateNameAndTotal($"Executing Short Analysis", shortTests.Count);
            var myTestsShort = new AnalysisBuilder(LoadStatus.UpdateCount);
            myTestsShort.GenerateFixedBarResults(shortTests.ToList());
            return myTestsShort;
        }

    }

    public class FixedBarExitViewmodel : EntryTestViewModel
    {
        protected override List<ITest[]> GenerateEntryTests() {
            var fixedBarExitOptions = new FixedBarExitTestOptions(25, 220, 1);
            LoadStatus.UpdateNameAndTotal($"Generating Long Fixed Bar Exit Tests", 
                (fixedBarExitOptions.MaximumExitPeriod - fixedBarExitOptions.MinimumExitPeriod) / fixedBarExitOptions.Increment);
            return TestFactory.GenerateFixedBarExitTest(ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket, fixedBarExitOptions, LoadStatus.UpdateCount);
        }
    }

    public class RandomExitViewmodel : EntryTestViewModel
    {
        protected override List<ITest[]> GenerateEntryTests()        {
            LoadStatus.UpdateNameAndTotal($"Generating Random Exit Tests", 200);
            return TestFactory.GenerateRandomExitTests(
                        ModelSingleton.Instance.MyStrategy,
                        ModelSingleton.Instance.Mymarket, 200, 200, LoadStatus.UpdateCount);
        }
    }

    public class StopTargetExitViewmodel : EntryTestViewModel
    {
        protected override List<ITest[]> GenerateEntryTests()        {
            var stopTargetExitOptions = new FixedStopTargetExitTestOptions(0.001, 0.001, 0.01, 12);
            //var stopTargetExitOptions = new FixedStopTargetExitTestOptions(0.003, 0.002, 0.002, 5);
            LoadStatus.UpdateNameAndTotal($"Generating Stop Target Exit Tests", stopTargetExitOptions.Divisions * stopTargetExitOptions.Divisions);
            return TestFactory.GenerateFixedStopTargetExitTest(ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket, 
                stopTargetExitOptions, LoadStatus.UpdateCount);
        }

    }
}
