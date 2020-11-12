using LinqStatistics;
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
        public List<RunnerState> Runner { get; protected set; }

        public StrategyRunnerBase(Market market, Strategy strat)
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
            Runner = new List<RunnerState>() { new RunnerState() };
            StrategyOptions marketOptions = new StrategyOptions();
            StrategyOptions portfolioOptions = new StrategyOptions()
            {
                ExpectancyCutOff = 1,
                SpreadCutOff = 4,
                WinPercentCutOff = 0,
            };

            var marketStrategyStateBuilder = new StrategyState.StrategyStateFactory(marketOptions);
            var portfolioStrategyStateBuilder = new StrategyState.StrategyStateFactory(portfolioOptions);

            List<double> marketRets = new List<double>();

            for (int i = 1; i < _market.RawData.Length; i++)
            {
                var returnCount = Runner.Select(x=>x.Market.Return).ToList();
                var marketExps = new DrillDownStats(new List<double>());
                if (marketRets.Count > 15) marketExps = new DrillDownStats(marketRets);
                if (marketExps.MedianExpectancy> 1) {
                    string a = "";
                }
                

                var newState = new RunnerState()
                {
                    Market = marketStrategyStateBuilder.BuildNextState(_market.RawData[i], marketExps, _strategy.Entries[i - 1]),
                    Portfolio = portfolioStrategyStateBuilder.BuildNextState(_market.RawData[i], marketExps, _strategy.Entries[i - 1])
                };

                if (newState.Market.Return != 0) marketRets.Add(newState.Market.Return);
                if (marketRets.Count > 15) marketRets.RemoveAt(0);

                Runner.Add(newState);
            }

        }
    }
}
