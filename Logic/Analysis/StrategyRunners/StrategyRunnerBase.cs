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
        protected Strategy _strategy { get; set; }
        public List<StrategyState> Runner { get; protected set; }
        private Action<ResultsContainer> _updateAction { get; set; }
        protected StrategyRunnerBase(Market market, Strategy strat)
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
        public FixedStopTargetExitStrategyRunner(Market market, Strategy strat) : base(market, strat)
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
            var optimiser = new FixedStopTargetStrategyOptimiser(_market, _strategy);

            var results = optimiser.Optimise(_market.RawData.Length - 1, _market.RawData.Length);
            stateBuilder.stop = 1 - results.StopDist;
            stateBuilder.target = results.TargetDist + 1;

            var tenMa = MovingAverage.ExponentialMovingAverage(results.Expectancy.Select(x=>x.Lower).ToList(), 10);

            Runner = new List<StrategyState>() { stateBuilder.BuildNextState(_market.RawData[0], false, false) };

            for (int i = 1; i < _market.RawData.Length; i++)
            {
                //var results = new FixedStopTargetExitOptimisation() { Expectancy = new List<BoundedStat>() };
                //if (_strategy.Entries[i - 1]) results = optimiser.Optimise(i - 1, i);
                //stateBuilder.stop = 1 - results.StopDist;
                //stateBuilder.target = results.TargetDist + 1;
                //Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], _strategy.Entries[i - 1] && results.Expectancy.Last().Average > 0, false));
                Runner.Add(stateBuilder.BuildNextState(_market.RawData[i], _strategy.Entries[i - 1] && results.Expectancy[i - 1].Average > 0, false ));

                if(i%500 == 0)update?.Invoke(new ResultsContainer(Runner.Select(x=>x.Return).ToList(), results.Expectancy[i - 1].Average));
                Trace.WriteLine($"{i} -- {Runner.Sum(x=>x.Return)} -- {results.Expectancy?.LastOrDefault()?.Median ?? 0} -- {Runner.Last().InvestedState.Invested} -- " +
                                $"{Runner.Last().InvestedState.TargetPrice} -- {Runner.Last().InvestedState.StopPrice} -- {_market.RawData[i].Close_Bid}" +
                                $" -- {stateBuilder.target} -- {stateBuilder.stop}");
            }

            update?.Invoke(new ResultsContainer(Runner.Select(x => x.Return).ToList(), results.Expectancy.Last().Average));

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