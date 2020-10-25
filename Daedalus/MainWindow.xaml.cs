using System.Linq;
using System.Windows;
using Daedalus.Models;
using LinqStatistics;
using OxyPlot;

namespace Daedalus
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //var Entries = ModelSingleton.Instance.MyStrategy.Entries.Count(x => x);
            //var Exits = ModelSingleton.Instance.MyStrategy.Exits.Count(x => x);
            //var AverageDuration = ModelSingleton.Instance.MyStrategy.Durations.Where(x => x != 0).Average();
            //var median = ModelSingleton.Instance.MyStrategy.Durations.Where(x => x != 0).Median();

            //AverageDuration = AverageDuration * 5 / 60;
            //median = median * 5 / 60;

            //this.Title = $"Entries: {Entries}   Exits: {Exits} \n Avg. Dur: {AverageDuration:0.0}hrs Med. Dur: {median:0.0}hrs";

        }
    }
}
