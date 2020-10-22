using System;
using Daedalus.Models;
using Daedalus.Utils;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Logic;
using Logic.Utils.Calculations;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Wpf;
using PriceSeries;
using PriceSeries.Indicators.Derived;
using AverageTrueRange = Logic.Utils.Calculations.AverageTrueRange;
using LineAnnotation = OxyPlot.Annotations.LineAnnotation;
using LinearAxis = OxyPlot.Axes.LinearAxis;

namespace Daedalus.ViewModels
{
    public class EntryViewModel : ViewModelBase
    {
        public PlotModel PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }

        public ObservableCollection<int> EntryPoints
        {
            get;
            set;
        } = new ObservableCollection<int>();

        private int _iterator { get; set; }
        public int Iterator
        {
            get { return _iterator;}
            set
            {
                Update(value);
                _iterator = value;
            }
        }

        private List<int> _exitPoint { get; set; } = new List<int >();
        public EntryViewModel() : base()
        {
            this.InitialiseData();
        }

        private void InitialiseData()
        {


            for (int i = 0; i < ModelSingleton.Instance.MyStrategy.Entries.Length; i++)
                if(ModelSingleton.Instance.MyStrategy.Entries[i])
                    EntryPoints.Add(i+1);

            for (int i = 0; i < ModelSingleton.Instance.MyStrategy.Exits.Length; i++)
                if (ModelSingleton.Instance.MyStrategy.Exits[i])
                    _exitPoint.Add(i + 1);

            Update(0);

        }

        public void Update(int x)
        {
            if(x > EntryPoints.Count) return;
            PlotModel = new PlotModel();
            ControllerModel = new PlotController();

            List<OhlcvItem> ohs = new List<OhlcvItem>();
            var series = new CandleStickAndVolumeSeries
            {
                PositiveColor = OxyColors.SeaGreen,
                NegativeColor = OxyColors.DarkRed,
                PositiveHollow = false,
                NegativeHollow = false,
                StrokeThickness = 1.3,
                RenderInLegend = false,
                VolumeStyle = VolumeStyle.None,
                //CandleWidth = 0.92,
            };

            var entryPoint = EntryPoints[x];
            var graphStart = EntryPoints[x] - 40;
            var graphEnd = entryPoint + 150;
            //var graphStart = 0;
            //var graphEnd = ModelSingleton.Instance.Mymarket.CostanzaData.Length;

            PlotModel.Annotations.Add(new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                X = EntryPoints[x],
            });
            PlotModel.Annotations.Add(new LineAnnotation()
            {
                MinimumX = entryPoint - 3,
                MaximumX = entryPoint + 3,
                Type = LineAnnotationType.Horizontal,
                Y = ModelSingleton.Instance.MyStrategy.Rules.First(x=>x.Order.Equals(Pos.Entry)).Dir == Thesis.Bull ? 
                    ModelSingleton.Instance.Mymarket.RawData[EntryPoints[x]].Open_Ask: ModelSingleton.Instance.Mymarket.RawData[EntryPoints[x]].Open_Bid,
            });

            if (_exitPoint.Any(y => y > EntryPoints[x]))
            {
                var exitPnt = _exitPoint.First(y => y > EntryPoints[x]);


                PlotModel.Annotations.Add(new LineAnnotation()
                {
                    Type = LineAnnotationType.Vertical,
                    X = exitPnt,
                });
                PlotModel.Annotations.Add(new LineAnnotation()
                {
                    MinimumX = exitPnt -3,
                    MaximumX= exitPnt +3,
                    Type = LineAnnotationType.Horizontal,
                    Y = ModelSingleton.Instance.MyStrategy.Rules.First(x => x.Order.Equals(Pos.Exit)).Dir == Thesis.Bull ?
                        ModelSingleton.Instance.Mymarket.RawData[exitPnt].Open_Bid : ModelSingleton.Instance.Mymarket.RawData[exitPnt].Open_Ask,
                });
                graphEnd = exitPnt + 50;
                graphEnd = ModelSingleton.Instance.Mymarket.CostanzaData.Length;
            }

            var atrpc = AverageTrueRange.CalculateATRPC(ModelSingleton.Instance.Mymarket.CostanzaData.ToList(),2,60);
            var retval = new List<Tuple<double, double, double>>();
            var mdpt = new List<double>();
            var dist = 0.1;

            ModelSingleton.Instance.Mymarket.CostanzaData.ToList().ForEach(x => mdpt.Add(x.High - ((x.High - x.Low) / 2.0)));

            for (var i = 0; i < ModelSingleton.Instance.Mymarket.CostanzaData.Length; i++)
            {
                retval.Add(new Tuple<double, double, double>
                (mdpt[i] + (mdpt[i] / 200 * atrpc[i]),
                    mdpt[i] - (mdpt[i] / 200 * atrpc[i]),
                    mdpt[i]));
            }

            var p = new OxyPlot.Series.AreaSeries
            {
                Background = OxyColors.Transparent,
                Color = OxyColors.Gray,
                StrokeThickness = 3
            };

            var six= MovingAverage.ExponentialMovingAverage(ModelSingleton.Instance.Mymarket.CostanzaData.Select(x => x.Close).ToList(),6);
            var ten = MovingAverage.SimpleMovingAverage(ModelSingleton.Instance.Mymarket.CostanzaData.Select(x => x.Close).ToList(),10);
            var twenty = MovingAverage.ExponentialMovingAverage(ModelSingleton.Instance.Mymarket.CostanzaData.Select(x => x.Close).ToList(),20);
            var fifty = MovingAverage.SimpleMovingAverage(ModelSingleton.Instance.Mymarket.CostanzaData.Select(x => x.Close).ToList(),50);
            var twoHunMA = MovingAverage.SimpleMovingAverage(ModelSingleton.Instance.Mymarket.CostanzaData.Select(x => x.Close).ToList(), 200);

            var sixLine = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Lime,
                StrokeThickness = 1
            };
            var tenLine = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Pink,
                StrokeThickness = 1
            };
            var twentytLine = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 1
            };

            var fiftyLine = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Gold,
                StrokeThickness = 1
            };
            var twoHunLinw = new OxyPlot.Series.LineSeries()
            {
                Color = OxyColors.Black,
                StrokeThickness = 1
            };
            for (var i = 0; i < retval.Count; i++)
            {
                sixLine.Points.Add(new DataPoint(i, six[i]));
                tenLine.Points.Add(new DataPoint(i, ten[i]));
                twentytLine.Points.Add(new DataPoint(i, twenty[i]));
                fiftyLine.Points.Add(new DataPoint(i, fifty[i]));
                twoHunLinw.Points.Add(new DataPoint(i, twoHunMA[i]));
            }
            
            for (var i = 0; i < retval.Count; i++)
            {
                p.Points.Add(new DataPoint(i , retval[i].Item2));
                p.Points2.Add(new DataPoint(i , retval[i].Item1));
            }

            for (int i = graphStart; i < graphEnd; i++)
            {
                if (i < 0) continue;
                if (i > ModelSingleton.Instance.Mymarket.CostanzaData.Length - 1) continue;

                var item = ModelSingleton.Instance.Mymarket.CostanzaData[i];
                series.Append(new OhlcvItem(i, item.Open, item.High, item.Low, item.Close, item.Volume));
            }

            LinearAxis xAx = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Maximum = graphStart+150,
                Minimum = graphStart
            };
            var rane = ModelSingleton.Instance.Mymarket.CostanzaData.ToList().GetRange(graphStart, 150).ToList();
            var yMax = rane.Max(x=>x.High);
            var yMin = rane.Min(x=>x.Low);

            LinearAxis yAx = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Maximum = yMax,
                Minimum = yMin
            };
            PlotModel.Axes.Add(xAx);
            PlotModel.Axes.Add(yAx);
            PlotModel.Series.Add(series);
            //PlotModel.Series.Add(pivseries);
            //PlotModel.Series.Add(pivseries2);
            //PlotModel.Series.Add(pivseries3);
            PlotModel.Series.Add(p);
            //PlotModel.Series.Add(pline);
            PlotModel.Series.Add(twentytLine);
            PlotModel.Series.Add(fiftyLine);
            PlotModel.Series.Add(twoHunLinw);

            PlotModel.InvalidatePlot(true);
            NotifyPropertyChanged($"PlotModel");
        }


    }
}
