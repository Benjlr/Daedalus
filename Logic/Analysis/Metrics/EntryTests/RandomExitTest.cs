using Logic.Metrics;
using RuleSets;
using System;
using PriceSeriesCore;

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

        protected void GenerateExit(int i, int maxCount) {
            _endIndex = _randomGenerator.Next(0,_maxLength) + i;
            if (_endIndex > maxCount) _endIndex = 0;
        }
        protected void SetDuration(int i) {
            Durations[i] = _endIndex-i;
            _endIndex = 0;
        }
    }

    public class LongRandomExitTest : RandomExitTest
    {
        protected override void SetResult(MarketData[] data, int i) {
            GenerateExit(i, data.Length -1);
            if (_endIndex != 0) 
                FBEResults[i] = (data[_endIndex].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < _endIndex; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
            SetDuration(_endIndex);
        }
        
        public LongRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

    public class ShortRandomExitTest : RandomExitTest
    {
        protected override void SetResult(MarketData[] data, int i) {
            GenerateExit(i, data.Length - 1);
            if (_endIndex != 0)
                FBEResults[i] = (data[i].Open_Bid - data[_endIndex].Open_Ask) / data[i].Open_Bid;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j < _endIndex; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
            SetDuration(i);
        }

        public ShortRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

}
