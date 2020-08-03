using Daedalus.Models;
using LinqStatistics;
using Logic.Metrics;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;

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
            var _test = TestFactory.GenerateRangeTest(1000, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            List<Bin> LongDistributions = _test.FinalResultLong.Where(x => x != 0).Histogram(50).ToList();
            List<Bin> ShortDistributions = _test.FinalResultShort.Where(x => x != 0).Histogram(100).OrderBy(x => x.RepresentativeValue).ToList();


            var horiAxis = new CategoryAxis()
            {
                Position = AxisPosition.Bottom,
            };
            horiAxis.ItemsSource = LongDistributions.Select(x => (int)x.RepresentativeValue);
            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Key = "Expectancy"
            };


            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);

            Update();
        }

        protected void Update()
        {

            PlotModel.Series.Clear();


            PlotModel.InvalidatePlot(true);
        }
    }
}

