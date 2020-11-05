using System.Collections.Generic;

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

        public override void ExecuteRunner(StrategyOptions options)
        {
            List<RunnerState> runner = new List<RunnerState>(){new RunnerState()};
            

            for (int i = 1; i < _market.RawData.Length; i++)
            {
                if (_strategy.Entries[i-1])
                {
                    if (options.GoodToEnter(runner[i-1].Portfolio.Stats, _market.RawData[i]))
                    {
                        if (!runner[i - 1].Portfolio.InvestedState.Invested)
                        {
                            var portfolioTrade= new TradeState();
                            portfolioTrade.Invested = true;
                            portfolioTrade.EntryPrice = _market.RawData[i].Open_Ask;
                            portfolioTrade.StopPrice = _market.RawData[i].Open_Ask;
                            portfolioTrade.TargetPrice = _market.RawData[i].Open_Ask;

                            var newPortfolioState= new StrategyState();
                        }
                    }
                    
                }
            }


        }
    }
}
