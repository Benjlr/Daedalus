using Logic.Analysis.StrategyRunners;
using Logic.Utils;
using OxyPlot;
using System.Linq;
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
            var runner = new FixedStopTargetExitStrategyRunner(ModelSingleton.Instance.Mymarket, ModelSingleton.Instance.MyStrategy);
            runner.ExecuteRunner();

            var portfolioRet = runner.Runner.Select(x => x.Portfolio.Return).ToList();
            var marketRet = runner.Runner.Select(x => x.Market.Return).ToList();


            var resultsm = ExpectancyTools.GetRollingExpectancy(marketRet, 10)
                .Select(x=>x.MedianExpectancy).ToList();
            var resultsp = HistogramTools.MakeCumulative(portfolioRet);

            var series1 = Series.GenerateExpectanySeriesHorizontal(resultsm, resultsp);
            MyResults = series1;
        }
    }
}
