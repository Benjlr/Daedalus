using Logic.Utils;
using System.Collections.Generic;
using System.Linq;
using Logic.Analysis.Metrics;
using Logic.Analysis.StrategyOptimiser;
using PriceSeriesCore.Calculations;
using RuleSets;
using RuleSets.Entry;

namespace Logic.Analysis.StrategyRunners
{
    public abstract class StrategyRunnerBase
    {
        protected Market _market { get; set; }
        protected Strategy _strategy { get; set; }
        public List<StrategyState> Runner { get; protected set; }

        protected StrategyRunnerBase(Market market, Strategy strat)
        {
            _market = market;
            _strategy = strat;
        }

        public abstract void ExecuteRunner();



    }

    public class FixedStopTargetExitStrategyRunner : StrategyRunnerBase
    {
        public FixedStopTargetExitStrategyRunner(Market market, Strategy strat) : base(market, strat)
        {
        }

        public override void ExecuteRunner()
        {
            StrategyOptions portfolioOptions = new StrategyOptions() {
                ExpectancyCutOff = 0,
                SpreadCutOff = 8,
                WinPercentCutOff = 0,
            };

            var stateBuilder = new StrategyState.StrategyStateFactory(portfolioOptions);
            var optimiser = new FixedStopTargetStrategyOptimiser(_market, _strategy);

            var results = optimiser.Optimise(_market.RawData.Length-1, _market.RawData.Length);
            stateBuilder.stop = 1-results.StopDist;
            stateBuilder.target = results.TargetDist+1;

            Runner = new List<StrategyState>() { stateBuilder.BuildNextState(_market.RawData[0], false, false) };

            for (int i = 1; i < _market.RawData.Length; i++)
                Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], _strategy.Entries[i - 1] && results.Expectancy[i-1].Median > 0, false));
        }
    }
}