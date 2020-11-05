using System;
using Logic.Metrics;

namespace Logic.Analysis.Metrics.EntryTests
{
    public abstract class RandomExitTest : TestBase
    {
        private Random _randomGenerator { get; set; }

        public RandomExitTest() {
            _randomGenerator = new Random();
        }

        protected override void SetResult(MarketData[] data, int i) {
            _endIndex = _randomGenerator.Next(i+1, data.Length-1);
        }
    }

    public class LongRandomExitTest : RandomExitTest
    {
        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
            Durations[i] = _endIndex;
        }
    }

    public class ShortRandomExitTest : RandomExitTest
    {
        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
            Durations[i] = _endIndex;
        }
    }

}
