using System;
using Logic.Utils;
using System.Collections.Generic;
using System.Diagnostics;
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
        protected List<Strategy> _strategy { get; set; }
        public List<StrategyState> Runner { get; protected set; }
        private Action<ResultsContainer> _updateAction { get; set; }
        protected StrategyRunnerBase(Market market, List<Strategy> strat)
        {
            _market = market;
            _strategy = strat;
        }

        public virtual void ExecuteRunner(Action<ResultsContainer> update = null) {
            _updateAction = update;
        }



    }

    public class FixedStopTargetExitStrategyRunner : StrategyRunnerBase
    {
        public FixedStopTargetExitStrategyRunner(Market market, List<Strategy> strat) : base(market, strat)
        {
        }

        public override void ExecuteRunner(Action<ResultsContainer> update = null) {
            base.ExecuteRunner(update);
            StrategyOptions portfolioOptions = new StrategyOptions() {
                ExpectancyCutOff = 0,
                SpreadCutOff = 3,
                WinPercentCutOff = 0,
            };

            var stateBuilder = new StrategyState.StrategyStateFactory(portfolioOptions);
            var optimiserLong = new FixedStopTargetStrategyOptimiser(_market, _strategy[0], MarketSide.Bull);

            var resultsLong = optimiserLong.Optimise(_market.RawData.Length - 1, _market.RawData.Length);
            stateBuilder.stop = 1 - resultsLong.StopDist;
            stateBuilder.target = resultsLong.TargetDist + 1;

            Runner = new List<StrategyState>() { stateBuilder.BuildNextState(_market.RawData[0], false, false) };
            var lb = 2000;
            for (int i = 1; i < _market.RawData.Length; i++)
            {
                //var results = new FixedStopTargetExitOptimisation() { Expectancy = new List<BoundedStat>() };
                //if (_strategy.Entries[i - 1] && i > lb) {
                //    results = optimiser.Optimise(i - 1, lb);
                //    stateBuilder.stop = 1 - results.StopDist;
                //    stateBuilder.target = results.TargetDist + 1;
                //    Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], _strategy.Entries[i - 1] && results.Expectancy.Last().Average > 0, _strategy.Entries[i - 1] && results.Expectancy.Last().Average > 0));
                //}
                //else 
                //    Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], false, false));
                 Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], _strategy[0].Entries[i - 1] && resultsLong.Expectancy[i - 1].Average > 0, false));
                


                Trace.WriteLine($"{i} -- {Runner.Sum(x=>x.Return)} -- {Runner.Last().InvestedState.Invested} -- " +
                                $"{Runner.Last().InvestedState.TargetPrice} -- {Runner.Last().InvestedState.StopPrice} -- {_market.RawData[i].Close_Bid}" +
                                $" -- {stateBuilder.target} -- {stateBuilder.stop}");

                if(i == _market.RawData.Length -1 || i % 500 == 0) update?.Invoke(new ResultsContainer(Runner.Select(x => x.Return).ToList(), 0));
            }

        }
    }

    public struct ResultsContainer
    {
        public List<double> Returns { get; set; }
        public double Expectancy { get; set; }

        public ResultsContainer(List<double> returns, double expectancy) {
            Returns = returns;
            Expectancy = expectancy;
        }
    }
}