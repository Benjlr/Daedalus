using System.ComponentModel;
using DataStructures;
using RuleSets;

namespace Logic.Metrics.EntryTests
{
    abstract class FixedBarExitTest : TestBase
    {
        protected FixedBarExitTest(int bars_to_wait) {
            _endIndex = bars_to_wait;
        }

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

        protected override void SetResult(BidAskData[] data, int i) {
            for (int j = i; j <= _endIndex+i && j < data.Length; j++) 
                FBEResults[j] += (data[j].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(BidAskData[] data, int i) {
            for (int j = i; j <= _endIndex+i && j < data.Length; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < 0)
                    FBEDrawdown[j] += (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
            Durations[i] = _endIndex;
        }

        public LongFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

    class ShortFixedBarExitTest : FixedBarExitTest
    {

        protected override void SetResult(BidAskData[] data, int i) {
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                FBEResults[j] += (data[i].Open_Bid - data[j].Open_Ask) / data[i].Open_Bid;
        }


        protected override void IterateTime(BidAskData[] data, int i) {
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < 0)
                    FBEDrawdown[j] += (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
            Durations[i] = _endIndex;
        }

        public ShortFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        {
        }
    }

}