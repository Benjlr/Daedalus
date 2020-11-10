using Logic.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Logic.Analysis.StrategyRunners
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

        public override void ExecuteRunner(StrategyOptions options, bool[] marketEntries, bool[] portfolioEntries)
        {
            List<RunnerState> runner = new List<RunnerState>() { new RunnerState() };


            for (int i = 1; i < _market.RawData.Length; i++)
            {
                StrategyState marketState = runner[i - 1].Market.BuildNextState(_market.RawData[i], options, portfolioEntries[i]);
                StrategyState portfolioState = runner[i - 1].Portfolio.BuildNextState(_market.RawData[i], options, portfolioEntries[i]);
            }


        }
   
        
    }
}
