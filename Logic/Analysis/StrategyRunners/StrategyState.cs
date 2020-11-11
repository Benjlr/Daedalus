using System.Collections.Generic;
using Logic.Utils;

namespace Logic.Analysis.StrategyRunners
{
    public class StrategyState
    {
        public TradeState InvestedState { get; private set; }
        public DrillDownStats Stats { get; private set; }
        public List<double> Returns { get; private set; }


        private StrategyState LastStateWasInvested( MarketData data, StrategyOptions options)
        {
            var state = new StrategyState();
            state.Returns = new List<double>(this.Returns);

            if (options.ShouldExit(state,this.InvestedState, data))
                state.InvestedState = TradeState.DoNothing();
            else if (state.CheckStopsAndTargets(data, this.InvestedState))
                state.InvestedState = TradeState.ContinueLong(data, this.InvestedState);
            else
                state.InvestedState = TradeState.DoNothing();

            state.Stats = new DrillDownStats(Returns);
            return state;
        }

        private bool CheckStopsAndTargets(MarketData data, TradeState tradeData)
        {
            var returns = this.Returns;
            if (data.Low_Bid < tradeData.StopPrice)
            {
                if (data.Open_Bid < tradeData.StopPrice)
                    returns.Add((data.Open_Bid - tradeData.EntryPrice) / tradeData.EntryPrice);
                else
                    returns.Add((tradeData.StopPrice - tradeData.EntryPrice) / tradeData.EntryPrice);
                return false;

            }
            else if (data.High_Bid > tradeData.TargetPrice)
            {
                if (data.Open_Bid > tradeData.TargetPrice)
                    returns.Add((data.Open_Bid - tradeData.EntryPrice) / tradeData.EntryPrice);
                else
                    returns.Add((tradeData.TargetPrice - tradeData.EntryPrice) / tradeData.EntryPrice);
                return false;
            }
            return true;
        }

        private StrategyState LastStateWasNotInvested(MarketData data, bool isEntry)
        {
            var state = new StrategyState();
            state.Returns = new List<double>(this.Returns);
            if (isEntry )
                state.InvestedState = TradeState.InvestLong(data);
            else
                state.InvestedState = TradeState.DoNothing();

            state.Stats = new DrillDownStats(Returns);
            return state;
        }

        public class StrategyStateFactory
        {
            private StrategyOptions _options { get; set; }
            private StrategyState _previousState { get; set; }
             

            public StrategyStateFactory(StrategyOptions options)
            {
                _options = options;
                _previousState = new StrategyState();
            }

            public StrategyState BuildNextState(MarketData data, DrillDownStats benchmark, bool isEntry)
            {
                if (_previousState.InvestedState.Invested)
                    _previousState = _previousState.LastStateWasInvested(data, _options);
                else
                    _previousState = _previousState.LastStateWasNotInvested(data, isEntry && _options.GoodToEnter(benchmark, data));
                return _previousState;
            }

        }
    }
}
