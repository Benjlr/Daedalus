using System.Collections.Generic;
using System.Linq;
using Daedalus.Models;
using LinqStatistics;
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

        private List<LineSeries> CapitalLong { get; set; }
        private List<LineSeries> CapitalShort { get; set; }

        public RangeTestsViewModel() : base()
        {
            this.InitialiseData();
        }


        protected void InitialiseData()
        {
            var _test = TestFactory.GenerateRangeTest(3000, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            var horiAxis = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            };

            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Key = "Expectancy"
            };

            foreach (var t in _test.FinalResultLong)
            {
                var newSeries = new LineSeries();
                for (int i = 0; i < t.Length; i++) newSeries.Points.Add(new DataPoint(i+1, t[i]));
                CapitalLong.Add(newSeries);
            }
            
            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);

            Update();
        }

        protected void Update()
        {
            PlotModel.Series.Clear();
            CapitalLong.ForEach(x => PlotModel.Series.Add(x));
            PlotModel.InvalidatePlot(true);
        }
    }
}

