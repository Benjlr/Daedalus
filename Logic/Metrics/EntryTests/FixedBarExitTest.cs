namespace Logic.Metrics.EntryTests
{
    public abstract class FixedBarExitTest : TestBase
    {
        protected FixedBarExitTest(int bars_to_wait) {
            _endIndex = bars_to_wait;
        }

        protected override void SetRuns(int i) {
            var runIndex = new int[_endIndex + 1];
            for (int j = i; j <= i + _endIndex; j++) runIndex[j - i] = j;
            RunIndices.Add(runIndex);
        }
    }

    public class LongFixedBarExitTest : FixedBarExitTest
    {
        public LongFixedBarExitTest(int bars_to_wait) : base(bars_to_wait) {
        }

        protected override void SetResult(MarketData[] data, int i) {
            FBEResults[i] = (data[i + _endIndex].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }
    }

    public class ShortFixedBarExitTest : FixedBarExitTest
    {
        public ShortFixedBarExitTest(int bars_to_wait) : base(bars_to_wait) {
        }

        protected override void SetResult(MarketData[] data, int i) {
            FBEResults[i] = (data[i].Open_Bid - data[i + _endIndex].Open_Ask) / data[i].Open_Bid;
        }


        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
        }

    }

}