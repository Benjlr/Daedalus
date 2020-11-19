using Logic;
using Logic.StrategyRunners;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using DataStructures.StatsTools;
using ViewCommon.Models;
using ViewCommon.Utils;

namespace Icarus.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        public LineSeries mySeries { get; set; }
        public PlotModel MyResults { get; set; }
        private ICommand _clickCommand;
        public ICommand ClickCommand => _clickCommand ??= new Commandler(Start, () => CanExecute);
        public bool CanExecute => true;

        public StrategyViewModel()
        {
            MyResults = new PlotModel();
            MyResults.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 45000

            });
            MyResults.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
            });
            mySeries = new LineSeries()
            {
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline
            };
            MyResults.Series.Add(mySeries);
            MyResults.Annotations.Add(new LineAnnotation() { Type = LineAnnotationType.Vertical, X = 0 });
        }

        public void Update(ResultsContainer container) {
            Application.Current.Dispatcher.Invoke(() => {
                var resultsp = HistogramTools.MakeCumulative(container.Returns);
                for (int i = mySeries.Points.Count; i < resultsp.Count; i++) 
                    mySeries.Points.Add(new DataPoint(mySeries.Points.Count+1, resultsp[i]));
                MyResults.InvalidatePlot(true);
                NotifyPropertyChanged($"MyResults");
            });
        }

        public void Start() {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Dowork));
        }

        private void Dowork(object callback) {
            var stratOne = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[] {new ATRContraction()}, ModelSingleton.Instance.Mymarket);

            var runner = new FixedStopTargetExitStrategyRunner(ModelSingleton.Instance.Mymarket, new List<Strategy>(){stratOne});
            runner.ExecuteRunner(Update);
        }

    }
}
