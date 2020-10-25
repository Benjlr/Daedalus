using OxyPlot.Series;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;

namespace Daedalus.Charts
{
    public class HeatMap
    {
        public static PlotModel GenerateHeatMap(List<List<double>> XYZvals, List<string> x_labels, List<string> y_label)
        {
            var dataXYZ = ConvertToArray(XYZvals);

            var retval = new PlotModel();
            retval.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "drawdown",
                ItemsSource = x_labels
            });
            retval.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "return",
                ItemsSource = y_label
            });
            retval.Axes.Add(new LinearColorAxis()
            {
                Palette = OxyPalettes.Jet(1000),
            });
            retval.Series.Add(new HeatMapSeries
            {
                X0 = 0,
                X1 = XYZvals[0].Count,
                Y0 = 0,
                Y1 = XYZvals.Count,
                Data = dataXYZ,
                Interpolate = true,
                XAxisKey = "drawdown",
                YAxisKey = "return",
                RenderMethod = HeatMapRenderMethod.Rectangles,
            });

            retval.Annotations.Add(new LineAnnotation(){Type = LineAnnotationType.Vertical, X = 0});

            return retval;
        }

        public static double[,] ConvertToArray(List<List<double>> list)
        {
            double[,] t = new double[list[0].Count, list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    t[j, i] = list[i][j];
                }
            }

            return t;
        }
    }
    public class Series
    {
        public static PlotModel GenerateSeries(List<double> values, List<double> values2)
        {

            var retval = new PlotModel();
            retval.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                //Key = "drawdown",
                //ItemsSource = x_labels
            });
            retval.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Maximum = 3,
                Minimum = 0
                //Key = "return",
                //ItemsSource = y_label
            });
            var series = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Solid,
            };
            var series2 = new LineSeries()
            {
                Color = OxyColors.Blue,
                LineStyle = LineStyle.Dash,
            };
            for (int i = 0; i < values.Count; i++) series.Points.Add(new DataPoint(i + 1, values[i]));
            for (int i = 0; i < values2.Count; i++) series2.Points.Add(new DataPoint(i + 1, values2[i]));

            retval.Series.Add(series);
            retval.Series.Add(series2);
            retval.Annotations.Add(new LineAnnotation(){Type = LineAnnotationType.Horizontal, Y = 1.0});
            retval.Annotations.Add(new LineAnnotation(){Type = LineAnnotationType.Horizontal, Y = 1.5});



            return retval;
        }
    }

}
