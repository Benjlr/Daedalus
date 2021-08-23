using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using Thought;

namespace CommonViews.Charts
{
    public class CandleStickSeriesGenerator
    {
        private static readonly List<OxyColor> _colourList = new List<OxyColor>() {
            OxyColors.Lime,
            OxyColors.Pink,
            OxyColors.Blue,
            OxyColors.Gold,
            OxyColors.Turquoise,
            OxyColors.Black,
        };


        public static PlotModel Generate(MarketTrade data) {
            var myModel = new PlotModel();

            AddAxesAndMainSeries(data, myModel);
            AddEntryExitAnnotations(data, myModel);
            AddAverages(data, myModel);

            return myModel;
        }

        private static void AddAverages(MarketTrade data, PlotModel myModel) {
            for (int i = 0; i < data.MovingAverages.Length; i++) {
                var line = new LineSeries() {
                    Color = _colourList[i],
                    StrokeThickness = 1
                };
                for (int j = 0; j < data.MovingAverages[i].Length; j++)
                    line.Points.Add(new DataPoint(1+j, data.MovingAverages[i][j]));

                myModel.Series.Add(line);
            }
        }

        private static void AddEntryExitAnnotations(MarketTrade data, PlotModel myModel) {
            myModel.Annotations.Add(new LineAnnotation() {
                Type = LineAnnotationType.Vertical,
                X = data.EntryIndex,
            });
            myModel.Annotations.Add(new LineAnnotation() {
                MinimumX = data.EntryIndex - 3,
                MaximumX = data.EntryIndex + 3,
                Type = LineAnnotationType.Horizontal,
                //Y = data.EntryPrice,
            });

            myModel.Annotations.Add(new LineAnnotation() {
                Type = LineAnnotationType.Vertical,
                X = data.ExitIndex,
            });

            myModel.Annotations.Add(new LineAnnotation() {
                MinimumX = data.ExitIndex - 3,
                MaximumX = data.ExitIndex + 3,
                Type = LineAnnotationType.Horizontal,
                //Y = data.ExitPrice,
            });
        }

        private static void AddAxesAndMainSeries(MarketTrade data, PlotModel myModel) {
            myModel.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Bottom,
            });
            myModel.Axes.Add(new LinearAxis() {
                Position = AxisPosition.Left,
            });

            var series = new CandleStickAndVolumeSeries
            {
                PositiveColor = OxyColors.SeaGreen,
                NegativeColor = OxyColors.DarkRed,
                PositiveHollow = false,
                NegativeHollow = false,
                StrokeThickness = 1.3,
                RenderInLegend = false,
                VolumeStyle = VolumeStyle.Stacked,
                //CandleWidth = 0.92,
            };

            for (int i = 0; i < data.Prices.Length; i++)
                series.Append(new OhlcvItem(i + 1, data.Prices[i].Open.Mid, data.Prices[i].High.Mid, data.Prices[i].Low.Mid, data.Prices[i].Close.Mid,
                    data.Prices[i].Volume));

            myModel.Series.Add(series);
        }
    }
}
