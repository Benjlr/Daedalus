using System.Collections.Generic;
using System.Collections.ObjectModel;
using CommonViews.Utils;
using CommonViews.Views;
using DataStructures;
using Thought;
using ViewCommon;

namespace CommonViews.ViewModels
{
    public class TradeResultViewModel : ViewModelBase
    {
        public ObservableCollection<Trade> MyTrades { get; set; }

        private List<MarketTrade> _myModels { get; set; }
        private TradeContextView _stockWindow { get; set; }


        public TradeResultViewModel(List<Trade> results, List<MarketTrade> models) {
            MyTrades = new ObservableCollection<Trade>(results);
            _myModels = new List<MarketTrade>(models);
            Iterator = 0;
            NotifyPropertyChanged($"MyTrades");
        }

        private int _iterator { get; set; }

        public int Iterator {
            get => _iterator;
            set {
                _iterator = value;
                if (MyTrades.Count > 0) {
                    _stockWindow = new TradeContextView();
                    (_stockWindow.DataContext as StockWindowViewModel).Update(_myModels[_iterator]);
                    _stockWindow.Show();
                    //var newContext = new StockWindowViewModel(_myModels[_iterator]);

                }
            }
        }
    }
}
