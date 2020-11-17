using Logic.Metrics;
using RuleSets;
using System;
using System.ComponentModel;
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
            _endIndex = _randomGenerator.Next(0,_maxLength);
            if (_endIndex > maxCount) _endIndex = 0;
        }
        protected void SetDuration(int i) {
            Durations[i] = _endIndex;
            _endIndex = 0;
        }
        public static RandomExitTest PrepareTest(MarketSide longShort, int maxLength) {
            switch (longShort) {
                case MarketSide.Bull: return new LongRandomExitTest(maxLength);
                case MarketSide.Bear: return new ShortRandomExitTest(maxLength);
                default: throw new InvalidEnumArgumentException();
            }
        }
    }

    public class LongRandomExitTest : RandomExitTest
    {
        protected override void SetResult(MarketData[] data, int i) {
            GenerateExit(i, data.Length -1);
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                FBEResults[i] += (data[j].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < 0)
                    FBEDrawdown[j] += (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
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
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                FBEResults[i] += (data[i].Open_Bid - data[i].Open_Ask) / data[i].Open_Bid;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < 0)
                    FBEDrawdown[j] += (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
            SetDuration(i);
        }

        public ShortRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

}
