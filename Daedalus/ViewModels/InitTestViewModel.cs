using Daedalus.Utils;

namespace Daedalus.ViewModels
{
    public class InitTestViewModel : ViewModelBase
    {
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public string Name { get; set; }

        public void UpdateCount() {
            Count++;
            NotifyPropertyChanged("Count");
        }

        public void UpdateNameAndTotal(string name, int total) {
            TotalCount = total;
            Name = name;
            Count= 0;
            NotifyPropertyChanged("TotalCount");
            NotifyPropertyChanged("Count");
            NotifyPropertyChanged("Name");
        }

    }
}
