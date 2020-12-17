using DataStructures;
using System.ComponentModel;
using System.Linq;

namespace Logic.Metrics.EntryTests
{
    public abstract class FixedStopTargetExitTest : TestBase
    {
        public double TargetDistance { get; }
        public double StopDistance { get; }

        protected FixedStopTargetExitTest(double target_distance, double stop_distance) {
            TargetDistance = target_distance;
            StopDistance = stop_distance;
        }

        protected override void SetResult(BidAskData[] data, int i) {
            InitialiseTradeGenerator(data[i],i);
            for (int j = i; j < data.Length; j++)
                if (ParseConditionals(data[j]))
                    break;
            if(_currentTrade.isActive) ExitTrade(data.Last());
        }

        private bool ParseConditionals(BidAskData data) {
            _currentTrade.Continue(data);
            return !_currentTrade.isActive;
        }

        protected abstract void InitialiseTradeGenerator(BidAskData data, int i);
        protected abstract void ExitTrade(BidAskData data);
        public static FixedStopTargetExitTest PrepareTest(MarketSide longShort, double target_distance, double stop_distance) {
            switch (longShort) {
                case MarketSide.Bull: return new LongFixedStopTargetExitTest(target_distance, stop_distance);
                case MarketSide.Bear: return new ShortFixedStopTargetExitTest(target_distance, stop_distance);
                default: throw new InvalidEnumArgumentException();
            }
        }
    }

    public class LongFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public LongFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance+1, 1-stop_distance) {
        }

        protected override void InitialiseTradeGenerator(BidAskData data, int i) {
            _currentTrade = new LongTradeGenerator(
                i,
                new TradePrices(new ExitPrices(StopDistance, TargetDistance), data.Open.Ask),
                AddTrade);
        }

        protected override void ExitTrade(BidAskData data) {
            _currentTrade.Exit(data.Close.Ticks, data.Close.Bid);
        }
    }

    public class ShortFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public ShortFixedStopTargetExitTest(double target_distance, double stop_distance) : base(1-target_distance, stop_distance+1) {
        }

        protected override void InitialiseTradeGenerator(BidAskData data, int i) {
            _currentTrade = new ShortTradeGenerator(
                i, 
                new TradePrices(new ExitPrices(StopDistance,TargetDistance),data.Open.Bid ), 
                AddTrade );
        }

        protected override void ExitTrade(BidAskData data) {
            _currentTrade.Exit(data.Close.Ticks, data.Close.Ask);
        }
    }
}