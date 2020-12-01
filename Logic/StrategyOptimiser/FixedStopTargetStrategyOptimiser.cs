using System.Collections.Generic;
using System.Linq;
using DataStructures;
using DataStructures.StatsTools;
using Logic.Metrics;
using Logic.Metrics.EntryTests;
using RuleSets;

namespace Logic.StrategyOptimiser
{
    public class FixedStopTargetStrategyOptimiser
    {
        public List<ITest> _myTests { get; set; }
        private Market _baseMarket { get; set; }
        private Strategy _baseStrategy { get; set; }
        private MarketSide _side { get; set; }

        private TestFactory.FixedStopTargetExitTestOptions _options { get; set; }

        public FixedStopTargetStrategyOptimiser(Market market, Strategy strat, MarketSide side)
        {
            _baseMarket = market;
            _baseStrategy = strat;
            _options = new TestFactory.FixedStopTargetExitTestOptions(0.001, 0.001, 0.007, 12);
        }

        public FixedStopTargetExitOptimisation Optimise(int lastKnownData, int lookBack)
        {
            var slicedMarket = _baseMarket.Slice(lastKnownData - lookBack+1, lastKnownData);
            var slicedStrat = _baseStrategy.Slice(lastKnownData - lookBack+1, lastKnownData);

            _myTests = _options.Run(slicedStrat,slicedMarket, _side).ToList();

            var topTests = _myTests.Select(x => (FixedStopTargetExitTest) x).OrderByDescending(x => x.Stats.AverageExpectancy).Take((int)(_myTests.Count * 0.05)).ToList();

            List<double> stops = topTests.Select(x => x.StopDistance).ToList();
            List<double> targets = topTests.Select(x => x.TargetDistance).ToList();
            
            var myExps = new List<List<double>>();
            //foreach (var test in topTests.Select(x => x.Trades.Select(y=>y.results).ToList()).ToList())
            //    myExps.Add(RollingStatsGenerator.GetRollingStats(test.ToList(), 6).Select(x => x.AverageExpectancy).ToList());

            var bouned = GenerateBoundedStats.Generate(myExps);

            return new FixedStopTargetExitOptimisation()
            {
                StopDist = stops[0],
                TargetDist = targets[0],
                Expectancy = bouned,
            };
        }
    }


    public struct FixedStopTargetExitOptimisation
    {
        public double StopDist { get; set; }
        public double TargetDist { get; set; }
        public List<BoundedStat> Expectancy { get; set; }
    }
}
