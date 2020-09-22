using Daedalus.Models;
using Daedalus.Utils;
using OxyPlot;
using OxyPlot.Series;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using OxyPlot.Annotations;
using OxyPlot.Axes;

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
        private int _padding = 200;

        public EntryViewModel() : base()
        {
            this.InitialiseData();
        }

        private void InitialiseData()
        {
            // var 

            for (int i = 0; i < ModelSingleton.Instance.MyStarrtegy.Entries.Length; i++)
                if(ModelSingleton.Instance.MyStarrtegy.Entries[i])
                    EntryPoints.Add(i+1);

            for (int i = 0; i < ModelSingleton.Instance.MyStarrtegy.Exits.Length; i++)
                if (ModelSingleton.Instance.MyStarrtegy.Exits[i])
                    _exitPoint.Add(i + 1);

            Update(0);

        }

        public void Update(int x)
        {

            PlotModel = new PlotModel();
            ControllerModel = new PlotController();


            var start = EntryPoints[x] - _padding;
            if (start < 0) start = 0;
            var end = _exitPoint.FirstOrDefault(y => y > EntryPoints[x]);

            if (end == 0) 
                end = ModelSingleton.Instance.Mymarket.CostanzaData.Length - 1;
            if (end + _padding > ModelSingleton.Instance.Mymarket.CostanzaData.Length) 
                end = ModelSingleton.Instance.Mymarket.CostanzaData.Length - 1;
            else 
                end = end + _padding;

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


            for (int i = start; i < end; i++)
            {
                var item = ModelSingleton.Instance.Mymarket.CostanzaData[i];
                series.Append(new OhlcvItem(i, item.Open, item.High, item.Low, item.Close, item.Volume));
            }



            series.ItemsSource = ohs;
            PlotModel.Series.Add(series);
            PlotModel.Annotations.Add(new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                X = EntryPoints[x],
            });
            PlotModel.Annotations.Add(new LineAnnotation()
            {
                Type = LineAnnotationType.Vertical,
                X = _exitPoint.FirstOrDefault(y => y > EntryPoints[x]),
            });

            LinearAxis xAx = new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Maximum = _exitPoint.FirstOrDefault(y => y > EntryPoints[x]) + 20,
                Minimum = EntryPoints[x] - 20
            };
            LinearAxis yAx = new LinearAxis()
            {
                Position = AxisPosition.Left,
                Maximum = series.Items.Max(y => y.High),
                Minimum = series.Items.Min(y => y.Low)
            };
            PlotModel.Axes.Add(xAx);
            PlotModel.Axes.Add(yAx);

            PlotModel.InvalidatePlot(true);
            NotifyPropertyChanged($"PlotModel");
        }


    }
}
