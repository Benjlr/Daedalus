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

        private LinearBarSeries CapitalLong { get; set; }
        private LinearBarSeries CapitalShort { get; set; }
        private LinearBarSeries WinRatioLong { get; set; }
        private LinearBarSeries WinRatioShort { get; set; }

        public RangeTestsViewModel() : base()
        {
            this.InitialiseData();
        }


        protected void InitialiseData()
        {
            var _test = TestFactory.GenerateRangeTest(1000, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            var WinAnnotation = new LineAnnotation
            {
                Y = 0,
                LineStyle = LineStyle.Solid,
                YAxisKey = "Expectancy",
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

            WinRatioLong = new LinearBarSeries()
            {
                
                FillColor = OxyColors.LightBlue,
                
                YAxisKey = "Ratio"
            };
            WinRatioShort = new LinearBarSeries()
            {
                
                FillColor = OxyColors.LightPink,
                YAxisKey = "Ratio"
            };
            CapitalLong = new LinearBarSeries()
            {
                FillColor = OxyColors.Blue,
                YAxisKey = "Expectancy"
            };
            CapitalShort = new LinearBarSeries()
            {
                FillColor = OxyColors.Red,
                YAxisKey = "Expectancy"
            };
            for (int i = 0; i < _test.FinalResultLong.Length; i++)
            {
            //    WinRatioLong.Points.Add(new DataPoint(i + i*1, _test.WinRatioLong[i]));
            //    WinRatioShort.Points.Add(new DataPoint(i + i*2, _test.WinRatioShort[i]));
                CapitalLong.Points.Add(new DataPoint(i+1, _test.FinalResultLong[i]));
                //CapitalShort.Points.Add(new DataPoint(i + i*4, _test.FinalResultShort[i]));
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

