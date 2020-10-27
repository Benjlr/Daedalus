using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Daedalus.Utils.Enums;
using Logic.Metrics;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Daedalus.Utils
{
    public abstract class TestViewModelBase : ViewModelBase
    {
        protected List<ITest[]> _test { get; set; }
        public PlotModel PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }
        public ObservableCollection<TestDataEnum> LinesToDisplay { get; }

        private LineSeries ExpectancyLong { get; set; }
        private LineSeries ExpectancyShort { get; set; }
        private LineSeries WinRatioLong { get; set; }
        private LineSeries WinRatioShort { get; set; }

        protected TestViewModelBase()
        {
            LinesToDisplay = new ObservableCollection<TestDataEnum>()
            {
                TestDataEnum.ExpectancyLong,
                TestDataEnum.ExpectancyShort,
                TestDataEnum.WinRatioShort,
                TestDataEnum.WinRatioLong,
            };
        }

        protected virtual void InitialiseData()
        {

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();

            List<LineSeries> mySeries = new List<LineSeries>();

            //var upperSeries = new LineSeries()
            //{
            //    Color = OxyColors.Gray,
            //    LineStyle = LineStyle.Solid,
            //};
            //for (int i = 0; i < _test.Length; i++) upperSeries.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyLongMax));
            //mySeries.Add(upperSeries);

            //var upperQuartSeries = new LineSeries()
            //{
            //    Color = OxyColors.Gray,
            //    LineStyle = LineStyle.LongDash,
            //};
            //for (int i = 0; i < _test.Length; i++) upperQuartSeries.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyLongUQ));
            //mySeries.Add(upperQuartSeries);

            var medianSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Dot,
            };
            for (int i = 0; i < _test.Count; i++) medianSeries.Points.Add(new DataPoint(i + 1, _test[i][0].ExpectancyMedian));
            mySeries.Add(medianSeries);

            var averageSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Solid,
            };
            for (int i = 0; i < _test.Count; i++) averageSeries.Points.Add(new DataPoint(i + 1, _test[i][0].ExpectancyAverage));
            mySeries.Add(averageSeries);

            //var lowerQuartileSeries = new LineSeries()
            //{
            //    Color = OxyColors.Gray,
            //    LineStyle = LineStyle.LongDash,
            //};
            //for (int i = 0; i < _test.Length; i++) lowerQuartileSeries.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyLongLQ));
            //mySeries.Add(lowerQuartileSeries);

            //var lowerBoundSeries = new LineSeries()
            //{
            //    Color = OxyColors.Gray,
            //    LineStyle = LineStyle.Solid,
            //};
            //for (int i = 0; i < _test.Length; i++) lowerBoundSeries.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyLongMin));
            //mySeries.Add(lowerBoundSeries);



            var horiAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            };
            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
            };




            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);
            mySeries.ForEach(x => PlotModel.Series.Add(x));

            Update();
        }

        protected void Update()
        {

            //PlotModel.Series.Clear();
            //foreach (var dataEnum in LinesToDisplay)
            //{
            //    switch (dataEnum)
            //    {
            //        case TestDataEnum.ExpectancyLong: PlotModel.Series.Add(ExpectancyLong);
            //            break;
            //        case TestDataEnum.ExpectancyShort: PlotModel.Series.Add(ExpectancyShort);
            //            break;
            //        case TestDataEnum.WinRatioLong: PlotModel.Series.Add(WinRatioLong);
            //            break;
            //        case TestDataEnum.WinRatioShort: PlotModel.Series.Add(WinRatioShort);
            //            break;
            //    }
            //}

            PlotModel.InvalidatePlot(true);
        }

    }
}
