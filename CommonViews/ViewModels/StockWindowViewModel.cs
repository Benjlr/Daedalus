using CommonViews.Charts;
using CommonViews.Utils;
using OxyPlot;

namespace Viewer
{
    public class StockWindowViewModel : ViewModelBase
    {
        private Model _model { get; }
        public PlotModel PlotModel { get; set; }
        public PlotController ControllerModel { get; set; }



        public StockWindowViewModel(Model model) {
            _model = model;
            PlotModel = CandleStickSeriesGenerator.Generate(_model);
            PlotModel.InvalidatePlot(true);
            ControllerModel = new PlotController();
            NotifyPropertyChanged($"PlotModel");
            NotifyPropertyChanged($"ControllerModel");
        }
    }
}
