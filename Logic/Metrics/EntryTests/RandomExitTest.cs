using DataStructures;
using System;
using System.ComponentModel;
using System.Linq;

namespace Logic.Metrics.EntryTests
{
    public abstract class RandomExitTest : TestBase
    {
        protected Random _randomGenerator { get; set; }
        protected int _maxLength { get; set; }
        protected int _randomExit { get; private set; }

        protected RandomExitTest(int maxLength) {
            _randomGenerator = new Random();
            _maxLength = maxLength;
        }

        protected void GenerateExit(int i, int maxCount) {
            _randomExit = _randomGenerator.Next(0,_maxLength);
            if (_randomExit > maxCount) _randomExit = 0;
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
            _currentTrade = new LongTradeGenerator(
                i, new TradePrices(ExitPrices.NoStopTarget(), data[i].Open.Ask), AddTrade);

            for (int j = i; j < _randomExit + i && j < data.Length; j++)
                _currentTrade.Continue(data[j]);

            if (_randomExit + i < data.Length)
                _currentTrade.Exit(data[_randomExit + i].Open.Ticks, data[_randomExit + i].Open.Bid);
            else
                _currentTrade.Exit(data.Last().Close.Ticks, data.Last().Close.Bid);
        }
        
        public LongRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

    public class ShortRandomExitTest : RandomExitTest
    {
        protected override void SetResult(BidAskData[] data, int i) {
            GenerateExit(i, data.Length - 1);
            _currentTrade = new ShortTradeGenerator(
                i, new TradePrices(ExitPrices.NoStopTarget(), data[i].Open.Bid), AddTrade);

            for (int j = i; j < _randomExit + i && j < data.Length; j++)
                _currentTrade.Continue(data[j]);

            if (_randomExit + i < data.Length)
                _currentTrade.Exit(data[_randomExit + i].Open.Ticks, data[_randomExit + i].Open.Bid);
            else
                _currentTrade.Exit(data.Last().Close.Ticks, data.Last().Close.Bid);
        }

        public ShortRandomExitTest(int maxLength) : base(maxLength)
        {
        }
    }

}
