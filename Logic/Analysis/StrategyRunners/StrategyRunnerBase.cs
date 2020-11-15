using Logic.Utils;
using System.Collections.Generic;
using System.Linq;
using Logic.Analysis.Metrics;
using Logic.Markets;
using Logic.Strategies;
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

            var portfolioStrategyStateBuilder = new StrategyState.StrategyStateFactory(portfolioOptions);
            Runner = new List<StrategyState>() { portfolioStrategyStateBuilder.BuildNextState(_market.RawData[0],false, false) };

            for (int i = 1; i < _market.RawData.Length; i++)
            {
                portfolioStrategyStateBuilder.stop = 1-0.005;
                portfolioStrategyStateBuilder.target= 1 + 0.005;

                Runner.Add(portfolioStrategyStateBuilder.BuildNextState(_market.RawData[i], _strategy.Entries[i - 1], false));
            }

        }
    }
}
