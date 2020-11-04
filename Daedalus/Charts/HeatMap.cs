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
                Actionition = AxisActionition.Bottom,
                Key = "drawdown",
                ItemsSource = x_labels
            });
            retval.Axes.Add(new CategoryAxis
            {
                Actionition = AxisActionition.Left,
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
}
