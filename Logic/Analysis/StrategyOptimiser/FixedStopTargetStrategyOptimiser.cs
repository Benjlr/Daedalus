using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Logic.Analysis.Metrics;
using Logic.Utils;
using RuleSets;

namespace Logic.Analysis.StrategyOptimiser
{
    public class FixedStopTargetStrategyOptimiser
    {
        public List<ITest> _myTests { get; set; }
        private  Market _baseMarket { get; set; }
        private  Strategy _baseStrategy { get; set; }
        private  MarketSide _side { get; set; }

        private FixedStopTargetExitTestOptions _options { get; set; }

        public FixedStopTargetStrategyOptimiser(Market market, Strategy strat)
        {
            _baseMarket = market;
            _baseStrategy = strat;
            _options = new FixedStopTargetExitTestOptions(0.001,0.001, 0.01, 20, MarketSide.Bull);
        }

        private int count = 1;
        public void UpdateConsole()
        {
            Debug.WriteLine(count++);
        }

        public void Optimise(int lastKnownData, int lookBack)
        {
            var slicedMarket = _baseMarket.Slice(lastKnownData - lookBack, lastKnownData);
            var slicedStrat = _baseStrategy.Slice(lastKnownData - lookBack, lastKnownData);

            count = 0;
            _myTests = TestFactory.GenerateFixedStopTargetExitTest(slicedStrat, slicedMarket, _options, UpdateConsole);
            var topTests = _myTests.OrderByDescending(x => x.Stats.AverageExpectancy).Take(_myTests.Count / 10).ToList();

            ExpectancyTools.GetRollingExpectancy()



        }

    }
}
