using Logic.Analysis.StrategyRunners;
using Logic.Metrics.EntryTests.TestsDrillDown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Analysis.StrategyRunnerBase
{
    public abstract class StrategyRunnerBase
    {
        protected Market _market { get; set; }
        protected Strategy _strategy { get; set; }

        public StrategyRunnerBase(Market market, Strategy strat)
        {
            _market = market;
            _strategy = strat;
        }

        public abstract void ExecuteRunner(StrategyOptions options);

    }

    public class FixedStopTargetExitStrategyRunner : StrategyRunnerBase
    {
        public FixedStopTargetExitStrategyRunner(Market market, Strategy strat) : base(market, strat)
        {
        }

        public override void ExecuteRunner(StrategyOptions options)
        {
            var strategyRunner = new RunnerState();

            for (int i = 0; i < _market.RawData.Length; i++)
            {
                if (_strategy.Entries[i])
                {
                    //if(strategyRunner.)
                }
            }


        }
    }
}
