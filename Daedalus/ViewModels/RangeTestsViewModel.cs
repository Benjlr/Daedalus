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

        private ColumnSeries CapitalLong { get; set; }
        private ColumnSeries CapitalShort { get; set; }
        //private LinearBarSeries WinRatioLong { get; set; }
        //private LinearBarSeries WinRatioShort { get; set; }

        public RangeTestsViewModel() : base()
        {
            this.InitialiseData();
        }


        protected void InitialiseData()
        {
            var _test = TestFactory.GenerateRangeTest(3000, ModelSingleton.Instance.MyStarrtegy, ModelSingleton.Instance.Mymarket);

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            List<Bin> LongDistributions = _test.FinalResultLong.Histogram(10).ToList();
            List<Bin> ShortDistributions = _test.FinalResultShort.Histogram(100).OrderBy(x => x.RepresentativeValue).ToList();


            var horiAxis = new CategoryAxis()
            {
                Position = AxisPosition.Bottom,
            };
            horiAxis.ItemsSource = LongDistributions.Select(x => (int)x.RepresentativeValue);

            double aboveZero = LongDistributions.Where(x => x.RepresentativeValue > 0).Sum(x => x.Count);
            double belowZero = LongDistributions.Where(x => x.RepresentativeValue < 0).Sum(x => x.Count);

            PlotModel.Title = $"{aboveZero / (aboveZero + belowZero): 0.00%} - {LongDistributions.Sum(x=>x.Count):0}";
            var zeroAnnote = new LineAnnotation
            {
                X = LongDistributions.IndexOf(LongDistributions.First(x=>x.RepresentativeValue > 0))-0.5,
                LineStyle = LineStyle.Solid,
                Type = LineAnnotationType.Vertical,
            };

            var vertAxis = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Key = "Expectancy"
            };

            CapitalLong = new ColumnSeries()
            {
                FillColor = OxyColors.Blue,
                IsStacked = false,
            };
            CapitalShort = new ColumnSeries()
            {
                FillColor = OxyColors.Red,
                IsStacked = false,
            };
            for (int i = 0; i < LongDistributions.Count; i++)
            {
                CapitalLong.Items.Add(new ColumnItem( LongDistributions[i].Count ));
            }

            

            PlotModel.Axes.Add(horiAxis);
            PlotModel.Axes.Add(vertAxis);
            PlotModel.Annotations.Add(zeroAnnote);

            Update();
        }

        protected void Update()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(CapitalLong);
            PlotModel.InvalidatePlot(true);
        }
    }
}

