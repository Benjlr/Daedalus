using System.Collections.Generic;
using Logic.Analysis.StrategyRunners;
using Logic.Utils;
using OxyPlot;
using System.Linq;
using LinqStatistics;
using Logic;
using Logic.Analysis;
using Logic.Analysis.Metrics;
using PriceSeriesCore.Calculations;
using RuleSets;
using RuleSets.Entry;
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
            var stopTargetExitOptions = new FixedStopTargetExitTestOptions(0.0015, 0.0045, 0.003, 10, MarketSide.Bull);

            var ashj = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[] { new ATRContraction() }, ModelSingleton.Instance.Mymarket);
            var tttyy = TestFactory.GenerateFixedStopTargetExitTest(ashj, ModelSingleton.Instance.Mymarket, stopTargetExitOptions);
            var myTestsLong22 = new AnalysisBuilder(null);
            myTestsLong22.GenerateFixedBarResults(tttyy);


            var runner = new FixedStopTargetExitStrategyRunner(ModelSingleton.Instance.Mymarket, ModelSingleton.Instance.MyStrategy);
            runner.ExecuteRunner();

            var portfolioRet = runner.Runner.Select(x => x.Return*10).ToList();

            var aaaaaaa = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[] { new ATRContraction() }, ModelSingleton.Instance.Mymarket);
            var tt = TestFactory.GenerateFixedStopTargetExitTest(aaaaaaa, ModelSingleton.Instance.Mymarket, stopTargetExitOptions);
            var myTestsLong = new AnalysisBuilder(null);
            myTestsLong.GenerateFixedBarResults(tt);

            var resultsm = GenerateBoundedStats.Generate(myTestsLong.RollingExpectancy).Select(x => x.Minimum).ToList();
            var resultsp = HistogramTools.MakeCumulative(portfolioRet);
            var tenMa = MovingAverage.ExponentialMovingAverage(resultsm, 5000);
            //var sharpes = ExpectancyTools.GetRollingExpectancy(runner.Runner.Select(x => x.Portfolio.Return ).ToList(), 500).Select(x=>x.SharpeRatio).ToList();


            var series1 = Series.GenerateSeriesHorizontal(new List<List<double>>(){resultsp, resultsp, resultsp});
            MyResults = series1;
        }

    }
}
