using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using Logic.Metrics;
using ViewCommon.Models;

namespace Daedalus.ViewModels
{
    public class MonteCarloViewModel
    {
        public PlotModel PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }

        private LineSeries Capital { get; set; }

        public MonteCarloViewModel() : base()
        {
            this.InitialiseData();
        }


        protected void InitialiseData()
        {
            var _test = TestFactory.GenerateMonteCarloTest(500, ModelSingleton.Instance.MyStrategy, ModelSingleton.Instance.Mymarket, 10000, 10);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();

            List<LineSeries> mySeries = new List<LineSeries>();

            var upperSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Solid,
            };
            for (int i = 0; i < _test.UpperBound.Length; i++) upperSeries.Points.Add(new DataPoint(i + 1, _test.UpperBound[i]));
            mySeries.Add(upperSeries);

            var upperQuartSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.LongDash,
            };
            for (int i = 0; i < _test.UpperQuartile.Length; i++) upperQuartSeries.Points.Add(new DataPoint(i + 1, _test.UpperQuartile[i]));
            mySeries.Add(upperQuartSeries);

            var medianSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Dot,
            };
            for (int i = 0; i < _test.Median.Length; i++) medianSeries.Points.Add(new DataPoint(i + 1, _test.Median[i]));
            mySeries.Add(medianSeries);

            var averageSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Solid,
            };
            for (int i = 0; i < _test.Average.Length; i++) averageSeries.Points.Add(new DataPoint(i + 1, _test.Average[i]));
            mySeries.Add(averageSeries);

            var lowerQuartileSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.LongDash,
            };
            for (int i = 0; i < _test.LowerQuartile.Length; i++) lowerQuartileSeries.Points.Add(new DataPoint(i + 1, _test.LowerQuartile[i]));
            mySeries.Add(lowerQuartileSeries);
            
            var lowerBoundSeries = new LineSeries()
            {
                Color = OxyColors.Gray,
                LineStyle = LineStyle.Solid,
            };
            for (int i = 0; i < _test.LowerBound.Length; i++) lowerBoundSeries.Points.Add(new DataPoint(i + 1, _test.LowerBound[i]));
            mySeries.Add(lowerBoundSeries);



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
            mySeries.ForEach(x=>PlotModel.Series.Add(x));

            Update();
        }

        protected void Update()
        {

            //PlotModel.Series.Clear();

            PlotModel.InvalidatePlot(true);
        }
    }
}

