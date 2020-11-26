using DataStructures;
using System.ComponentModel;
using System.Linq;

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
                new LongTradeGenerator(i,
                    new TradePrices(ExitPrices.NoStopTarget(),
                    data[i].Open_Ask), 
                    AddTrade);

            for (int j = i; j < (_endIndex) + i && j < data.Length-1; j++)
                _currentTrade.Continue(data[j]);

            if(_endIndex + i < data.Length)
                _currentTrade.Exit(data[_endIndex+i].Open_Bid);
            else 
                _currentTrade.Exit(data.Last().Close_Bid);
        }

        public LongFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

    class ShortFixedBarExitTest : FixedBarExitTest
    {
        protected override void IterateBars(BidAskData[] data, int i) {
            _currentTrade =
                new ShortTradeGenerator(i,
                    new TradePrices(ExitPrices.NoStopTarget(),
                        data[i].Open_Bid),
                    AddTrade);

            for (int j = i; j < (_endIndex) + i && j < data.Length - 1; j++)
                _currentTrade.Continue(data[j]);

            if (_endIndex + i  < data.Length)
                _currentTrade.Exit(data[_endIndex + i].Open_Ask);
            else
                _currentTrade.Exit(data.Last().Close_Ask);
        }

        public ShortFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

}