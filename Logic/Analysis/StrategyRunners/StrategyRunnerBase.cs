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

        public override void ExecuteRunner(StrategyOptions options)
        {
            List<RunnerState> runner = new List<RunnerState>() { new RunnerState() };


            for (int i = 1; i < _market.RawData.Length; i++)
            {
                var marketState = StrategyState.GenerateState(runner[i - 1].Market, _market.RawData[i - 1] , _strategy.Entries[i - 1]);

                if ()
                {
                    if (!runner[i - 1].Portfolio.InvestedState.Invested)
                    {
                        if (options.GoodToEnter(runner[i - 1].Market.Stats, _market.RawData[i - 1]))
                        {
                            var portfolioTrade = TradeState.InvestLong(_market.RawData[i]);
                            var newPortfolioState = new StrategyState();
                            newPortfolioState.InvestedState = portfolioTrade;
                            newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                            newPortfolioState.Stats = new DrillDownStats(new List<double>(newPortfolioState.Returns) { portfolioTrade.Return });

                            portfolioState = newPortfolioState;

                        }
                        else
                        {
                            var portfolioTrade = TradeState.DoNothing();

                            var newPortfolioState = new StrategyState();
                            newPortfolioState.InvestedState = portfolioTrade;
                            newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                            newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                            portfolioState = newPortfolioState;

                        }
                    }
                    else
                    {
                        var portfolioTrade = TradeState.ContinueLong(_market.RawData[i], runner[i - 1].Portfolio.InvestedState);

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(new List<double>(newPortfolioState.Returns) { portfolioTrade.Return });

                        portfolioState = newPortfolioState;
                    }

                    if (!runner[i - 1].Market.InvestedState.Invested)
                    {
                        var portfolioTrade = TradeState.InvestLong(_market.RawData[i]);
                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;

                        List<double> returns = new List<double>();
                        if (newPortfolioState.Returns.Count > 100) returns = newPortfolioState.Returns.Skip(newPortfolioState.Returns.Count - 100).ToList();
                        else returns = newPortfolioState.Returns;

                        newPortfolioState.Stats = new DrillDownStats(returns);

                        marketState = newPortfolioState;

                    }
                    else
                    {
                        var portfolioTrade = TradeState.ContinueLong(_market.RawData[i], runner[i - 1].Portfolio.InvestedState);

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;

                        List<double> returns = new List<double>();
                        if (newPortfolioState.Returns.Count > 100) returns = newPortfolioState.Returns.Skip(newPortfolioState.Returns.Count - 100).ToList();
                        else returns = newPortfolioState.Returns;

                        newPortfolioState.Stats = new DrillDownStats(returns);

                        marketState = newPortfolioState;
                    }
                }
                else
                {
                    if (runner[i - 1].Portfolio.InvestedState.Invested)
                    {
                        var portfolioTrade = new TradeState();
                        portfolioTrade.Invested = true;
                        portfolioTrade.EntryPrice = runner[i - 1].Portfolio.InvestedState.EntryPrice;
                        portfolioTrade.StopPrice = runner[i - 1].Portfolio.InvestedState.StopPrice;
                        portfolioTrade.TargetPrice = runner[i - 1].Portfolio.InvestedState.TargetPrice;
                        portfolioTrade.Return = (_market.RawData[i].Open_Bid - portfolioTrade.EntryPrice) / portfolioTrade.EntryPrice;

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                        portfolioState = newPortfolioState;
                    }
                    else
                    {
                        var portfolioTrade = new TradeState();
                        portfolioTrade.Invested = false;
                        portfolioTrade.EntryPrice = -1;
                        portfolioTrade.StopPrice = -1;
                        portfolioTrade.TargetPrice = -1;
                        portfolioTrade.Return = -1;

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                        portfolioState = newPortfolioState;
                    }

                    if (runner[i - 1].Market.InvestedState.Invested)
                    {
                        var portfolioTrade = new TradeState();
                        portfolioTrade.Invested = true;
                        portfolioTrade.EntryPrice = _market.RawData[i].Open_Ask;
                        portfolioTrade.StopPrice = _market.RawData[i].Open_Ask * (1 - 0.005);
                        portfolioTrade.TargetPrice = _market.RawData[i].Open_Ask * (1 + 0.005);
                        portfolioTrade.Return = (_market.RawData[i].Open_Bid - portfolioTrade.EntryPrice) / portfolioTrade.EntryPrice;

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                        marketState = newPortfolioState;

                    }
                    else
                    {
                        var portfolioTrade = new TradeState();
                        portfolioTrade.Invested = true;
                        portfolioTrade.EntryPrice = _market.RawData[i].Open_Ask;
                        portfolioTrade.StopPrice = _market.RawData[i].Open_Ask * (1 - 0.005);
                        portfolioTrade.TargetPrice = _market.RawData[i].Open_Ask * (1 + 0.005);
                        portfolioTrade.Return = (_market.RawData[i].Open_Bid - portfolioTrade.EntryPrice) / portfolioTrade.EntryPrice;

                        var newPortfolioState = new StrategyState();
                        newPortfolioState.InvestedState = portfolioTrade;
                        newPortfolioState.Returns = runner[i - 1].Portfolio.Returns;
                        newPortfolioState.Stats = new DrillDownStats(newPortfolioState.Returns);

                        marketState = newPortfolioState;
                    }
                }

                runner.Add(new RunnerState() { Market = marketState, Portfolio = portfolioState });


            }


        }
   
        
    }
}
