using LinqStatistics;
using Logic.Utils;
using System.Collections.Generic;
using System.Linq;

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

        public abstract void ExecuteRunner(bool[] marketEntries, bool[] portfolioEntries);



    }

    public class FixedStopTargetExitStrategyRunner : StrategyRunnerBase
    {
        public FixedStopTargetExitStrategyRunner(Market market, Strategy strat) : base(market, strat)
        {
        }

        public override void ExecuteRunner(bool[] marketEntries, bool[] portfolioEntries)
        {
            List<RunnerState> runner = new List<RunnerState>() { new RunnerState() };
            StrategyOptions marketOptions = new StrategyOptions();
            StrategyOptions portfolioOptions = new StrategyOptions()
            {
                ExpectancyCutOff = 1.1,
                SpreadCutOff = 3,
                WinPercentCutOff = 0,
            };

            var marketStrategyStateBuilder = new StrategyState.StrategyStateFactory(marketOptions);
            var portfolioStrategyStateBuilder = new StrategyState.StrategyStateFactory(portfolioOptions);

            for (int i = 1; i < _market.RawData.Length; i++)
            {
                var marketExps = ExpectancyTools.GetRollingExpectancy(runner.Select(x => x.Market.Stats.MedianExpectancy).ToList(), 200);

                var newState = new RunnerState()
                {
                    Market = marketStrategyStateBuilder.BuildNextState(_market.RawData[i], marketExps.Last(), marketEntries[i - 1]),
                    Portfolio = portfolioStrategyStateBuilder.BuildNextState(_market.RawData[i], marketExps.Last(), marketEntries[i - 1])
                };


            }

        }
    }
}
