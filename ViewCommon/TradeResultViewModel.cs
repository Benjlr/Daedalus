using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Viewer
{
    public class TradeResultViewModel : ViewModelBase
    {
        public ObservableCollection<Trades> MyTrades { get; set; }

        private List<Model> _myModels { get; set; }
        private StockWindow _stockWindow { get; set; }


        public TradeResultViewModel(List<Trades> results, List<Model> models) {
            MyTrades = new ObservableCollection<Trades>(results);
            _myModels = new List<Model>(models);
            Iterator = 0;
            OnPropertyChanged($"MyTrades");
        }

        private int _iterator { get; set; }

        public int Iterator {
            get => _iterator;
            set {
                _iterator = value;
                if (MyTrades.Count > 0) {
                    _stockWindow = new StockWindow() { DataContext = new StockWindowViewModel(_myModels[_iterator]) };
                    _stockWindow.Show();
                }
            }
        }
    }
}
