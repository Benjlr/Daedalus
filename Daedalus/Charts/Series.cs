using Logic.Utils;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;

namespace Daedalus.Charts
{
    public class Series
    {
        public static PlotModel GenerateExpectanySeries(List<double> values, List<double> values2)
        {

            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Actionition = AxisActionition.Bottom,

                Maximum = 3,
                Minimum = 0
            });
            retval.Axes.Add(new LinearAxis
            {
                Actionition = AxisActionition.Left,
            });
            var series = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline

            };
            var series2 = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Dash,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline

            };
            for (int i = 0; i < values.Count; i++) series.Points.Add(new DataPoint(values[i], i + 1));
            for (int i = 0; i < values2.Count; i++) series2.Points.Add(new DataPoint(values2[i], i + 1));

            retval.Series.Add(series);
            retval.Series.Add(series2);
            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Vertical, X = 1.0});
            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Vertical, X = 1.5});



            return retval;
        }

        public static PlotModel GenerateMultipleSeries(List<List<double>> values)
        {

            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Actionition = AxisActionition.Bottom,
            });
            retval.Axes.Add(new LinearAxis
            {
                Actionition = AxisActionition.Left,
                Maximum = 3,
                Minimum = 0.2
            });

            var pallette = OxyPalettes.Hue(values.Count);

            for (int j = 0; j < values.Count; j++)
            {
                var t = values[j];
                var series = new LineSeries()
                {
                    Color = pallette.Colors[j],
                    LineStyle = LineStyle.Solid,
                    InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline

                };
                for (int i = 0; i < t.Count; i++) series.Points.Add(new DataPoint(i + 1, t[i]));
                retval.Series.Add(series);

            }

            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Horizontal, Y = 1.0});
            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Horizontal, Y = 2.0});

            return retval;
        }

        public static PlotModel GenerateBoundedSeries(List<BoundedStat> values)
        {
            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Actionition = AxisActionition.Bottom,
            });
            retval.Axes.Add(new LinearAxis
            {
                Actionition = AxisActionition.Left,
            });

            var lowerSeriesValues = values.Select(x => x.Lower).ToList();
            var lowerSeries = new LineSeries()
            {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < lowerSeriesValues.Count; i++) lowerSeries.Points.Add(new DataPoint(i + 1, lowerSeriesValues[i]));
            retval.Series.Add(lowerSeries);

            var miedianSeriesValues = values.Select(x => x.Median).ToList();
            var medianSeries = new LineSeries()
            {
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dot,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < miedianSeriesValues.Count; i++) medianSeries.Points.Add(new DataPoint(i + 1, miedianSeriesValues[i]));
            retval.Series.Add(medianSeries);

            var avgSeriesValues = values.Select(x => x.Average).ToList();
            var avgSeries = new LineSeries()
            {
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dash,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < avgSeriesValues.Count; i++) avgSeries.Points.Add(new DataPoint(i + 1, avgSeriesValues[i]));
            retval.Series.Add(avgSeries);

            var upperSeriesValues = values.Select(x => x.Upper).ToList();
            var upperSeries = new LineSeries()
            {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < upperSeriesValues.Count; i++) upperSeries.Points.Add(new DataPoint(i + 1, upperSeriesValues[i]));
            retval.Series.Add(upperSeries);



            return retval;
        }
    }
}