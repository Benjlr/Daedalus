using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;
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
            List<RunnerState> runner = new List<RunnerState>() { new RunnerState() };


            for (int i = 1; i < _market.RawData.Length; i++)
            {
                RunnerState portfolioState = new RunnerState();

                if (_strategy.Entries[i - 1] && !runner[i - 1].Portfolio.InvestedState.Invested)
                {
                    if (options.GoodToEnter(runner[i - 1].Portfolio.Stats, _market.RawData[i - 1]))
                    {
                        var portfolioTrade = new TradeState();
                        portfolioTrade.Invested = true;
                        portfolioTrade.EntryPrice = _market.RawData[i].Open_Ask;
                        portfolioTrade.StopPrice = _market.RawData[i].Open_Ask *  (1-0.005);
                        portfolioTrade.TargetPrice = _market.RawData[i].Open_Ask * (1+0.005);
                        portfolioTrade.Return = (_market.RawData[i].Open_Bid - portfolioTrade.EntryPrice) / _market.RawData[i].Open_Ask;

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                        portfolioState.Portfolio = newPortfolioState;
                    }
                }
                else if (runner[i - 1].Portfolio.InvestedState.Invested)
                {

                }


            }


        }
    }
}
