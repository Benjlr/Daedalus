using DataStructures.StatsTools;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Linq;

namespace ViewCommon.Charts
{
    public class Series
    {
        public static PlotModel GenerateSeriesHorizontal(List<List<double>> lineSeries)
        {

            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,

            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });

            var pallette = OxyPalettes.Rainbow(lineSeries.Count);

            foreach (var line in lineSeries)
            {
                var series = new LineSeries()
                {
                    Color = pallette.Colors[lineSeries.IndexOf(line)],
                    LineStyle = LineStyle.Solid,
                    InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                };
                for (int i = 0; i < line.Count; i++) series.Points.Add(new DataPoint(i + 1, line[i]));
                retval.Series.Add(series);

            }

            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Vertical, X = 0});
            return retval;
        }

        public static PlotModel GenerateSeriesVertical(List<List<double>> lineSeries)
        {

            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });

            var pallette = OxyPalettes.Rainbow(lineSeries.Count);


            foreach (var line in lineSeries) {
                var series = new LineSeries()
                {
                    Color = pallette.Colors[lineSeries.IndexOf(line)],
                    LineStyle = LineStyle.Solid,
                    InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                };
                for (int i = 0; i < line.Count; i++) series.Points.Add(new DataPoint(line[i], i + 1));

                retval.Series.Add(series);
            }
            retval.Annotations.Add(new LineAnnotation() {Type = LineAnnotationType.Horizontal, Y = 0});
            return retval;
        }

        public static PlotModel GenerateHistogramSeries(List<BoundedStat> values, List<string> x_labels, List<string> y_label) {
            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,

            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            var minSeriesValues = values.Select(x => x.Minimum).ToList();
            var minSeries = new LineSeries()
            {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < minSeriesValues.Count; i++) minSeries.Points.Add(new DataPoint(minSeriesValues[i], i + 1));
            retval.Series.Add(minSeries);


            var lowerSeriesValues = values.Select(x => x.Lower).ToList();
            var lowerSeries = new LineSeries()
            {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < lowerSeriesValues.Count; i++) lowerSeries.Points.Add(new DataPoint(lowerSeriesValues[i], i + 1));
            retval.Series.Add(lowerSeries);

            var miedianSeriesValues = values.Select(x => x.Median).ToList();
            var medianSeries = new LineSeries()
            {
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dot,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < miedianSeriesValues.Count; i++) medianSeries.Points.Add(new DataPoint(miedianSeriesValues[i], i + 1));
            retval.Series.Add(medianSeries);

            var avgSeriesValues = values.Select(x => x.Average).ToList();
            var avgSeries = new LineSeries()
            {
                Color = OxyColors.Black,
                LineStyle = LineStyle.Dash,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < avgSeriesValues.Count; i++) avgSeries.Points.Add(new DataPoint(avgSeriesValues[i], i + 1));
            retval.Series.Add(avgSeries);

            var upperSeriesValues = values.Select(x => x.Upper).ToList();
            var upperSeries = new LineSeries()
            {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < upperSeriesValues.Count; i++) upperSeries.Points.Add(new DataPoint(upperSeriesValues[i], i + 1));
            retval.Series.Add(upperSeries);

            var maxSeriesValues = values.Select(x => x.Upper).ToList();
            var maxSeries = new LineSeries() {
                Color = OxyColors.LightGray,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline
            };
            for (int i = 0; i < maxSeriesValues.Count; i++) maxSeries.Points.Add(new DataPoint(maxSeriesValues[i], i + 1));
            retval.Series.Add(maxSeries);


            return retval;
        }


        public static PlotModel GenerateBoundedSeries(List<BoundedStat> values)
        {
            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false
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

        public static PlotModel GenerateBoundedSeries(List<BoundedStat> valuesLong, List<BoundedStat> valuesShort)
        {
            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            var lowerSeriesValues = valuesLong.Select(x => x.Lower).ToList();
            var lowerSeries = new LineSeries()
            {
                Color = OxyColors.LightBlue,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < lowerSeriesValues.Count; i++) lowerSeries.Points.Add(new DataPoint(i + 1, lowerSeriesValues[i]));
            retval.Series.Add(lowerSeries);

            var miedianSeriesValues = valuesLong.Select(x => x.Median).ToList();
            var medianSeries = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Dot,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < miedianSeriesValues.Count; i++) medianSeries.Points.Add(new DataPoint(i + 1, miedianSeriesValues[i]));
            retval.Series.Add(medianSeries);

            var avgSeriesValues = valuesLong.Select(x => x.Average).ToList();
            var avgSeries = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Dash,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < avgSeriesValues.Count; i++) avgSeries.Points.Add(new DataPoint(i + 1, avgSeriesValues[i]));
            retval.Series.Add(avgSeries);

            var upperSeriesValues = valuesLong.Select(x => x.Upper).ToList();
            var upperSeries = new LineSeries()
            {
                Color = OxyColors.LightBlue,
                LineStyle = LineStyle.Solid,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < upperSeriesValues.Count; i++) upperSeries.Points.Add(new DataPoint(i + 1, upperSeriesValues[i]));
            retval.Series.Add(upperSeries);


            lowerSeriesValues = valuesShort.Select(x => x.Lower).ToList();
            var lowerSeriesShort = new LineSeries()
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < lowerSeriesValues.Count; i++) lowerSeriesShort.Points.Add(new DataPoint(i + 1, lowerSeriesValues[i]));
            retval.Series.Add(lowerSeriesShort);

            miedianSeriesValues = valuesShort.Select(x => x.Median).ToList();
            var medianSeriesShort = new LineSeries()
            {
                Color = OxyColors.DarkRed,
                LineStyle = LineStyle.Dot,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < miedianSeriesValues.Count; i++) medianSeriesShort.Points.Add(new DataPoint(i + 1, miedianSeriesValues[i]));
            retval.Series.Add(medianSeriesShort);

            avgSeriesValues = valuesShort.Select(x => x.Average).ToList();
            var avgSeriesShort = new LineSeries()
            {
                Color = OxyColors.DarkRed,
                LineStyle = LineStyle.Dash,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < avgSeriesValues.Count; i++) avgSeriesShort.Points.Add(new DataPoint(i + 1, avgSeriesValues[i]));
            retval.Series.Add(avgSeriesShort);

            upperSeriesValues = valuesShort.Select(x => x.Upper).ToList();
            var upperSeriesShort = new LineSeries()
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
                InterpolationAlgorithm = InterpolationAlgorithms.UniformCatmullRomSpline

            };
            for (int i = 0; i < upperSeriesValues.Count; i++) upperSeriesShort.Points.Add(new DataPoint(i + 1, upperSeriesValues[i]));
            retval.Series.Add(upperSeriesShort);



            return retval;
        }
    }
}