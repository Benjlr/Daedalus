using System;
using DataStructures;
using RuleSets;

namespace Logic.StrategyRunners
{
    public class StrategyState
    {
        public TradeStateGenerator InvestedState { get; private set; }
        
        public double Return { get; private set; }

        public StrategyState() {
            Return = 0;
        }

        private StrategyState LastStateWasInvested(BidAskData data, StrategyOptions options, MarketSide side)
        {
            var state = new StrategyState();
            if (options.ShouldExit(state,InvestedState, data))
                state.InvestedState = InvestedState.DoNothing();
            else if (state.CheckStopsAndTargets(data, InvestedState,side))
                state.InvestedState = InvestedState.Continue(data);
            else
                state.InvestedState = InvestedState.DoNothing();

            return state;
        }

        private bool CheckStopsAndTargets(BidAskData data, TradeStateGenerator tradeData, MarketSide longShort)
        {
            return longShort.Equals(MarketSide.Bull) ? 
                LongCheck(data, tradeData) : ShortCheck(data, tradeData);
        }

        private bool LongCheck(BidAskData data, TradeStateGenerator tradeData)
        {
            if (data.Low_Bid < tradeData.TradeState.StopPrice)
            {
                if (data.Open_Bid < tradeData.TradeState.StopPrice)
                    Return = (data.Open_Bid - tradeData.TradeState.EntryPrice) / tradeData.TradeState.EntryPrice;
                else
                    Return = (tradeData.TradeState.StopPrice - tradeData.TradeState.EntryPrice) / tradeData.TradeState.EntryPrice;
                return false;
            }
            else if (data.High_Bid > tradeData.TradeState.TargetPrice)
            {
                if (data.Open_Bid > tradeData.TradeState.TargetPrice)
                    Return = (data.Open_Bid - tradeData.TradeState.EntryPrice) / tradeData.TradeState.EntryPrice;
                else
                    Return = (tradeData.TradeState.TargetPrice - tradeData.TradeState.EntryPrice) / tradeData.TradeState.EntryPrice;
                return false;
            }

            return true;
        }

        private bool ShortCheck(BidAskData data, TradeStateGenerator tradeData) {
            if (data.Low_Ask < tradeData.TradeState.TargetPrice) {
                if (data.Open_Ask < tradeData.TradeState.TargetPrice)
                    Return = (tradeData.TradeState.EntryPrice - data.Open_Ask) / tradeData.TradeState.EntryPrice;
                else
                    Return = (tradeData.TradeState.EntryPrice - tradeData.TradeState.TargetPrice) / tradeData.TradeState.EntryPrice;
                return false;
            }
            else if (data.High_Ask > tradeData.TradeState.StopPrice) {
                if (data.Open_Ask > tradeData.TradeState.StopPrice)
                    Return = (tradeData.TradeState.EntryPrice - data.Open_Ask) / tradeData.TradeState.EntryPrice;
                else
                    Return = (tradeData.TradeState.EntryPrice - tradeData.TradeState.StopPrice) / tradeData.TradeState.EntryPrice;
                return false;
            }

            return true;
        }

        private StrategyState LastStateWasNotInvested(BidAskData data, double stop, double target, bool isLongEntry, bool isShortEntry, bool isGood) {
            var state = new StrategyState();
            if ((isLongEntry || isShortEntry) && isGood)
                state.InvestedState = InvestedState.Invest(data, new ExitPrices(stop,target), isLongEntry);
            else
                state.InvestedState = InvestedState.DoNothing();
            return state;
        }

        public class StrategyStateFactory
        {
            private StrategyOptions _options { get; set; }
            private StrategyState _previousState { get; set; }
            public double target { get; set; }
            public double stop { get; set; }
            public MarketSide LongShort { get;  set; }


            public StrategyStateFactory(StrategyOptions options)
            {
                _options = options;
                _previousState = new StrategyState() { };
            }

            public StrategyState BuildNextState(BidAskData data, bool isEntryLong, bool isEntryShort)
            {
                if (_previousState.InvestedState.TradeState.Invested)
                    _previousState = _previousState.LastStateWasInvested(data, _options, LongShort);
                else 
                    _previousState = _previousState.LastStateWasNotInvested(data, stop, target, isEntryLong, isEntryShort, _options.GoodToEnter(12,100, (int)(data.Open_Ask - data.Open_Bid), DateTime.Now));
                return _previousState;
            }

        }
    }
}
