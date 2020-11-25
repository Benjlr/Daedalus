using System;
using DataStructures;
using RuleSets;

namespace Logic.StrategyRunners
{
    public class StrategyState
    {
        private MarketSide longShort { get; set; }
        public TradeStateGenerator InvestedState { get; private set; }

        private void LastStateWasInvested(BidAskData data, StrategyOptions options, MarketSide side) {
            if (options.ShouldExit(InvestedState, data))
                InvestedState.Exit(longShort.Equals(MarketSide.Bull) ? data.Open_Bid : data.Open_Ask);
            //else if (CheckStopsAndTargets(data))
            //    InvestedState.Continue(longShort.Equals(MarketSide.Bull) ? data.Close_Bid : data.Close_Ask);
        }





        private void LastStateWasNotInvested(BidAskData data, double stop, double target, int marketIndex, bool isLongEntry, bool isShortEntry, bool isGood) {
            var state = new StrategyState();
            //if ((isLongEntry || isShortEntry) && isGood)
            //    InvestedState = TradeStateGenerator.Invest(
            //        new ExitPrices(stop, target),
            //        longShort,
            //        marketIndex,
            //        longShort.Equals(MarketSide.Bull) ? data.Open_Ask : data.Open_Bid,
            //        longShort.Equals(MarketSide.Bull) ? data.Close_Ask : data.Close_Bid);
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

            public StrategyState BuildNextState(BidAskData data, bool isEntryLong, bool isEntryShort) {
                //    if (_previousState.InvestedState.TradeState.Invested)
                //        _previousState = _previousState.LastStateWasInvested(data, _options, LongShort);
                //    else
                //        _previousState = _previousState.LastStateWasNotInvested(data, stop, target, isEntryLong, isEntryShort, _options.GoodToEnter(12, 100, (int)(data.Open_Ask - data.Open_Bid), DateTime.Now));
                return new StrategyState();
            }

        }
    }
}
