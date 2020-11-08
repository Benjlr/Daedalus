using Daedalus.Charts;
using Daedalus.Models;
using Daedalus.Utils;
using Logic.Analysis;
using Logic.Analysis.Metrics;
using Logic.Utils;
using OxyPlot;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Daedalus.ViewModels
{
    public class DetailedFixBarExits : ViewModelBase
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

        public DetailedFixBarExits()
        {
            LoadStatus = new InitTestViewModel();
            LoadWindowVisibility = Visibility.Visible;
            ChartVisibility = Visibility.Hidden;
            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");

            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);

            bg.RunWorkerAsync();
            bg.RunWorkerCompleted += finished_work;
        }
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            var fixedBarExitOptions = new FixedBarExitTestOptions(5, 220, 1);

            LoadStatus.UpdateNameAndTotal($"Generating Fixed Bar Exit Tests", (fixedBarExitOptions.MaximumExitPeriod - fixedBarExitOptions.MinimumExitPeriod) / fixedBarExitOptions.Increment);
            var tests = TestFactory.GenerateFixedBarExitTest(ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket, fixedBarExitOptions , LoadStatus.UpdateCount);
            LoadStatus.UpdateNameAndTotal($"Executing Analysis", tests.Count*2);

            var myTestsLong = new AnalysisBuilder(LoadStatus.UpdateCount);
            var myTestsShort = new AnalysisBuilder(LoadStatus.UpdateCount);
            myTestsLong.GenerateFixedBarResults(tests.Select(x => x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByTest, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByTest, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByTest, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByTest, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByTest));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByTest));
        }

        void finished_work(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadWindowVisibility = Visibility.Collapsed;
            ChartVisibility = Visibility.Visible;

            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");


            PlotModelDrawdownLong.InvalidatePlot(false);
            PlotModelDrawdownShort.InvalidatePlot(false);
            PlotModelReturnsLong.InvalidatePlot(false);
            PlotModelReturnsShort.InvalidatePlot(false);
            PlotModelDDsLong.InvalidatePlot(false);
            PlotModelDDsShort.InvalidatePlot(false);
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
    }

    public class DetailedRandomExits : ViewModelBase
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

        public DetailedRandomExits()
        {
            LoadStatus = new InitTestViewModel();

            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);

            bg.RunWorkerAsync();
            bg.RunWorkerCompleted += finished_work;
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {

            LoadStatus.UpdateNameAndTotal($"Generating Random Exit Tests", 200);
            var tests = TestFactory.GenerateRandomExitTests(
            ModelSingleton.Instance.MyStrategy,
            ModelSingleton.Instance.Mymarket, 200, 200, null);

            IntialisationInformation.ChangeTotalCount(tests.Count);
            LoadStatus.UpdateNameAndTotal($"Executing Analysis", tests.Count);

            var myTestsLong = new AnalysisBuilder(LoadStatus.UpdateCount);
            var myTestsShort = new AnalysisBuilder(LoadStatus.UpdateCount);
            myTestsLong.GenerateFixedBarResults(tests.Select(x => x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByTest, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByTest, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByTest, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByTest, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByTest));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByTest));
        }

        void finished_work(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadWindowVisibility = Visibility.Collapsed;
            ChartVisibility = Visibility.Visible;

            NotifyPropertyChanged("LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");

            PlotModelDrawdownLong.InvalidatePlot(false);
            PlotModelDrawdownShort.InvalidatePlot(false);
            PlotModelReturnsLong.InvalidatePlot(false);
            PlotModelReturnsShort.InvalidatePlot(false);
            PlotModelDDsLong.InvalidatePlot(false);
            PlotModelDDsShort.InvalidatePlot(false);
            ExpectancyLong.InvalidatePlot(false);
            ExpectancyShort .InvalidatePlot(false);
            LongRollingExp.InvalidatePlot(false);
            ShortRollingExp.InvalidatePlot(false);
            LongDrawdowns.InvalidatePlot(false);
            ShortDrawdowns.InvalidatePlot(false);

            NotifyPropertyChanged("PlotModelDrawdownLong");
            NotifyPropertyChanged("PlotModelDrawdownShort");
            NotifyPropertyChanged("PlotModelReturnsLong");
            NotifyPropertyChanged("PlotModelReturnsShort");
            NotifyPropertyChanged("PlotModelDDsLong");
            NotifyPropertyChanged("PlotModelDDsShort");
            NotifyPropertyChanged("ExpectancyLong");
            NotifyPropertyChanged("ExpectancyShort");
            NotifyPropertyChanged("LongRollingExp");
            NotifyPropertyChanged("ShortRollingExp");
            NotifyPropertyChanged("LongDrawdowns");
            NotifyPropertyChanged("ShortDrawdowns");
        }
    }
    public class DetailedFixStopExits : ViewModelBase
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

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {

            var stopTargetExitOptions = new FixedStopTargetExitTestOptions(0.04 / 10.0, 0.04 / 10.0, 0.008, 500);

            LoadStatus.UpdateNameAndTotal($"Generating Stop Target Exit Tests", stopTargetExitOptions.Divisions);

            var tests = TestFactory.GenerateFixedStopTargetExitTest(ModelSingleton.Instance.MyStrategy,ModelSingleton.Instance.Mymarket, stopTargetExitOptions, LoadStatus.UpdateCount);


            LoadStatus.UpdateNameAndTotal($"Executing Analysis", tests.Count*2);



            var myTestsLong = new AnalysisBuilder(LoadStatus.UpdateCount);
            var myTestsShort = new AnalysisBuilder(LoadStatus.UpdateCount);
            myTestsLong.GenerateFixedBarResults(tests.Select(x => x[0]).ToList());
            myTestsShort.GenerateFixedBarResults(tests.Select(x => x[1]).ToList());

            PlotModelDrawdownLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByDrawdown, myTestsLong.X_label_categorised, myTestsLong.Y_label_categorised);
            PlotModelDrawdownShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByDrawdown, myTestsShort.X_label_categorised, myTestsShort.Y_label_categorised);
            PlotModelReturnsLong = HeatMap.GenerateHeatMap(myTestsLong.ReturnByTest, myTestsLong.X_label, myTestsLong.Y_label);
            PlotModelReturnsShort = HeatMap.GenerateHeatMap(myTestsShort.ReturnByTest, myTestsShort.X_label, myTestsShort.Y_label);
            PlotModelDDsLong = HeatMap.GenerateHeatMap(myTestsLong.DrawdownByTest, myTestsLong.X_label_categorised, myTestsLong.Y_label);
            PlotModelDDsShort = HeatMap.GenerateHeatMap(myTestsShort.DrawdownByTest, myTestsShort.X_label_categorised, myTestsShort.Y_label);
            ExpectancyLong = Series.GenerateExpectanySeries(myTestsLong.ExpectancyMedian, myTestsLong.ExpectancyAverage);
            ExpectancyShort = Series.GenerateExpectanySeries(myTestsShort.ExpectancyMedian, myTestsShort.ExpectancyAverage);
            LongRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy));
            ShortRollingExp = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.RollingExpectancy));
            LongDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsLong.DrawdownByTest));
            ShortDrawdowns = Series.GenerateBoundedSeries(GenerateBoundedStats.Generate(myTestsShort.DrawdownByTest));
        }

        void finished_work(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadWindowVisibility = Visibility.Collapsed;
            ChartVisibility = Visibility.Visible;

            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");

            PlotModelDrawdownLong.InvalidatePlot(false);
            PlotModelDrawdownShort.InvalidatePlot(false);
            PlotModelReturnsLong.InvalidatePlot(false);
            PlotModelReturnsShort.InvalidatePlot(false);
            PlotModelDDsLong.InvalidatePlot(false);
            PlotModelDDsShort.InvalidatePlot(false);
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

        public DetailedFixStopExits()
        {
            LoadStatus = new InitTestViewModel();

            LoadWindowVisibility = Visibility.Visible;
            ChartVisibility = Visibility.Collapsed;
            NotifyPropertyChanged($"LoadWindowVisibility");
            NotifyPropertyChanged($"ChartVisibility");


            var bg = new BackgroundWorker();
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);


            bg.RunWorkerAsync();
            bg.RunWorkerCompleted += finished_work;
        }

    }
}
