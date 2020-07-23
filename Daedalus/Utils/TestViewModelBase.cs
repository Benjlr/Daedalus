using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        protected ITest[] _test { get; set; }
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

            var lineAnnotation = new LineAnnotation
            {
                Y = 1,
                //YAxisKey = "Expectancy",
                LineStyle = LineStyle.Solid,
                Type = LineAnnotationType.Horizontal,
            };
            var WinAnnotation = new LineAnnotation
            {
                Y = 0.5,
                LineStyle = LineStyle.Dot,
                //YAxisKey = "Ratio",
                Type = LineAnnotationType.Horizontal,
            };

            var horiAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            };

            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                //Key = "Expectancy"
            };
            //var vertAxisRatio = new LinearAxis()
            //{
            //    Position = AxisPosition.Right,
            //    Key = "Ratio"
            //};

            WinRatioLong = new LineSeries()
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.Blue,
                //YAxisKey = "Ratio"
            };
            WinRatioShort = new LineSeries()
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.Red,
                //YAxisKey = "Ratio"
            };

            ExpectancyLong = new LineSeries()
            {
                Color = OxyColors.Blue,
                //YAxisKey = "Expectancy"
            };
            ExpectancyShort = new LineSeries()
            {
                Color = OxyColors.Red,
                //YAxisKey = "Expectancy"
            };

            for (int i = 0; i < _test.Length; i++)
            {
                WinRatioLong.Points.Add(new DataPoint(i + 1, _test[i].WinPercentageLong));
                WinRatioShort.Points.Add(new DataPoint(i + 1, _test[i].WinPercentageShort));
                ExpectancyLong.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyLong));
                ExpectancyShort.Points.Add(new DataPoint(i + 1, _test[i].ExpectancyShort));
            }

            PlotModel.Annotations.Add(lineAnnotation);
            PlotModel.Annotations.Add(WinAnnotation);
            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);
            Update();
        }

        protected void Update()
        {

            PlotModel.Series.Clear();
            foreach (var dataEnum in LinesToDisplay)
            {
                switch (dataEnum)
                {
                    case TestDataEnum.ExpectancyLong: PlotModel.Series.Add(ExpectancyLong);
                        break;
                    case TestDataEnum.ExpectancyShort: PlotModel.Series.Add(ExpectancyShort);
                        break;
                    case TestDataEnum.WinRatioLong: PlotModel.Series.Add(WinRatioLong);
                        break;
                    case TestDataEnum.WinRatioShort: PlotModel.Series.Add(WinRatioShort);
                        break;
                }
            }

            PlotModel.InvalidatePlot(true);
        }

    }
}
