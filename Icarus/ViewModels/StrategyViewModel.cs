using System.Linq;
using Logic.Analysis.StrategyRunners;
using Logic.Utils;
using OxyPlot;
using OxyPlot.Axes;
using ViewCommon.Charts;
using ViewCommon.Models;
using ViewCommon.Utils;

namespace Icarus.ViewModels
{
    public class StrategyViewModel : ViewModelBase
    {
        public PlotModel MyResults { get; set; }

        public StrategyViewModel()
        {
            FixedStopTargetExitStrategyRunner runner = new FixedStopTargetExitStrategyRunner(ModelSingleton.Instance.Mymarket, ModelSingleton.Instance.MyStrategy);
            runner.ExecuteRunner();

            var resultsm = ExpectancyTools.GetRollingExpectancy(runner.Runner.Last().Market.Returns, 200).Select(x=>x.MedianExpectancy).ToList();
            var resultsp = runner.Runner.Select(x=>x.Portfolio.Stats.MedianExpectancy).ToList();

            var series1 = Series.GenerateExpectanySeries(resultsm, resultsp);
            MyResults = series1;
        }
    }
}
