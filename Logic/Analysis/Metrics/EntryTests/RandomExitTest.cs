using System;
using Logic.Metrics;

namespace Logic.Analysis.Metrics.EntryTests
{
    public abstract class RandomExitTest : TestBase
    {
        protected Random _randomGenerator { get; set; }
        protected int _maxLength { get; set; }

        protected RandomExitTest(int maxLength) {
            _randomGenerator = new Random();
            _maxLength = maxLength;
        }

        protected void GenerateExit(int i) {
            _endIndex = _randomGenerator.Next(0,_maxLength) + i;
        }
        protected void SetDuration(int i) {
            Durations[i] = _endIndex;
            _endIndex = 0;
        }
    }

    public class LongRandomExitTest : RandomExitTest
    {
        protected override void SetResult(MarketData[] data, int i) {
            GenerateExit(i);
            if (_endIndex+i < data.Length - 1) 
                FBEResults[i] = (data[i + _endIndex].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
            SetDuration(i);
        }
        
        private void SetDuration(int i)
        {
            Durations[i] = _endIndex;
            _endIndex = 0;
        }

        public LongRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

    public class ShortRandomExitTest : RandomExitTest
    {
        protected override void SetResult(MarketData[] data, int i) {
            GenerateExit(i);
            if (_endIndex < data.Length - 1)
                FBEResults[i] = (data[i].Open_Bid - data[i + _endIndex].Open_Ask) / data[i].Open_Bid;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            if (_endIndex + i > data.Length) FBEDrawdown[i] = 0;
            else IterateDrawdown(data, i);
            SetDuration(i);
        }
        private void IterateDrawdown(MarketData[] data, int i) {
            for (int j = i; j < i + _endIndex; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
        }


        public ShortRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

}
