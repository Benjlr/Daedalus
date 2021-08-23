using CommonViews.Charts;
using CommonViews.Utils;
using CommonViews.Views;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Thought;

namespace CommonViews.ViewModels
{
    public class StockWindowViewModel : ViewModelBase
    {
        private MarketTrade _model { get; set; }
        public PlotModel MyPlotModel { get; set; }
        public PlotController MyControllerModel { get; set; }



        public StockWindowViewModel() {
        }

        public void Update(MarketTrade model) {
            _model = model;
            MyPlotModel = CandleStickSeriesGenerator.Generate(_model);
            MyControllerModel = new PlotController();

            MyPlotModel.InvalidatePlot(true);
            NotifyPropertyChanged($"MyPlotModel");
            NotifyPropertyChanged($"MyControllerModel");
            MyPlotModel.InvalidatePlot(true);
            MyPlotModel.InvalidatePlot(true);
            NotifyPropertyChanged($"MyPlotModel");
            NotifyPropertyChanged($"MyControllerModel");
        }
    }
}
