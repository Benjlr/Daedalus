using DataStructures;
using System;
using System.ComponentModel;

namespace Logic.Metrics.EntryTests
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
        protected override void SetResult(BidAskData[] data, int i) {
            GenerateExit(i, data.Length -1);
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                _currentTrade.Add((data[j].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask);
        }
        
        public LongRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

    public class ShortRandomExitTest : RandomExitTest
    {
        protected override void SetResult(BidAskData[] data, int i) {
            GenerateExit(i, data.Length - 1);
            for (int j = i; j <= _endIndex + i && j < data.Length; j++)
                _currentTrade.Add((data[i].Open_Bid - data[i].Open_Ask) / data[i].Open_Bid);
        }

        public ShortRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

}
