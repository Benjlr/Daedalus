using DataStructures;
using System.ComponentModel;

namespace Logic.Metrics.EntryTests
{
    abstract class FixedBarExitTest : TestBase
    {
        protected FixedBarExitTest(int bars_to_wait) {
            _endIndex = bars_to_wait;
        }

        protected override void SetResult(BidAskData[] data, int i) {
            IterateBars(data, i);
        }

        protected abstract void IterateBars(BidAskData[] data, int i);

        public static FixedBarExitTest PrepareTest(MarketSide longShort, int waitBars) {
            switch (longShort) {
                case MarketSide.Bull: return new LongFixedBarExitTest(waitBars);
                case MarketSide.Bear: return new ShortFixedBarExitTest(waitBars);
                default: throw new InvalidEnumArgumentException();
            }
        }
    }

    class LongFixedBarExitTest : FixedBarExitTest
    {
        protected override void IterateBars(BidAskData[] data, int i) {
            _currentTrade =
                TradeStateGenerator.Invest(
                    MarketSide.Bull,
                    new TradePrices(ExitPrices.NoStopTarget()), 
                    AddTrade,
                    i);

            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                _currentTrade.Add((data[j].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask);
        }

        public LongFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

    class ShortFixedBarExitTest : FixedBarExitTest
    {
        protected override void IterateBars(BidAskData[] data, int i) {
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                _currentTrade.Add((data[i].Open_Bid - data[j].Open_Ask) / data[i].Open_Bid);
        }

        public ShortFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

}