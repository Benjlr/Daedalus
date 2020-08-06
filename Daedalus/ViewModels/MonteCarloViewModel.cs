using Daedalus.Models;
using Logic.Metrics;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;

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
            var _test = TestFactory.GenerateMonteCarloTest(500, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket, 10000, 5);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();

            List<LineSeries> mySeries = new List<LineSeries>();
            foreach (var t in _test.LongIterations)
            {
                var series = new LineSeries();
                for (int i = 0; i < t.Length; i++) series.Points.Add(new DataPoint(i + 1, t[i]));
                mySeries.Add(series);
            }



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

