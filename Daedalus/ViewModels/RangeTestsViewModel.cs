using Daedalus.Models;
using Logic.Metrics;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Daedalus.ViewModels
{
    public class RangeTestsViewModel
    {
        public PlotModel PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }

        private LineSeries CapitalLong { get; set; }
        private LineSeries CapitalShort { get; set; }
        private LineSeries WinRatioLong { get; set; }
        private LineSeries WinRatioShort { get; set; }

        public RangeTestsViewModel() : base()
        {
            this.InitialiseData();
        }


        protected void InitialiseData()
        {
            var _test = TestFactory.GenerateRangeTest(100, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            var WinAnnotation = new LineAnnotation
            {
                Y = 0.5,
                LineStyle = LineStyle.Dot,
                YAxisKey = "Ratio",
                Type = LineAnnotationType.Horizontal,
            };

            var horiAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            };

            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Key = "Expectancy"
            };
            var vertAxisRatio = new LinearAxis()
            {
                Position = AxisPosition.Right,
                Key = "Ratio"
            };

            WinRatioLong = new LineSeries()
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.Blue,
                YAxisKey = "Ratio"
            };
            WinRatioShort = new LineSeries()
            {
                LineStyle = LineStyle.Dot,
                Color = OxyColors.Red,
                YAxisKey = "Ratio"
            };
            CapitalLong = new LineSeries()
            {
                Color = OxyColors.Blue,
                YAxisKey = "Expectancy"
            };
            CapitalShort = new LineSeries()
            {
                Color = OxyColors.Red,
                YAxisKey = "Expectancy"
            };
            for (int i = 0; i < _test.FinalResultLong.Length; i++)
            {
                WinRatioLong.Points.Add(new DataPoint(i + 1, _test.WinRatioLong[i]));
                WinRatioShort.Points.Add(new DataPoint(i + 1, _test.WinRatioShort[i]));
                CapitalLong.Points.Add(new DataPoint(i + 1, _test.FinalResultLong[i]));
                CapitalShort.Points.Add(new DataPoint(i + 1, _test.FinalResultShort[i]));
            }

            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);
            PlotModel.Axes.Add(vertAxisRatio);
            //PlotModel.Annotations.Add(lineAnnotation);
            PlotModel.Annotations.Add(WinAnnotation);

            Update();
        }

        protected void Update()
        {

            PlotModel.Series.Clear();
            PlotModel.Series.Add(CapitalLong);
            PlotModel.Series.Add(CapitalShort);
            PlotModel.Series.Add(WinRatioLong);
            PlotModel.Series.Add(WinRatioShort);

            PlotModel.InvalidatePlot(true);
        }
    }
}

