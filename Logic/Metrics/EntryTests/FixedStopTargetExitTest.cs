using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DataStructures;
using RuleSets;

namespace Logic.Metrics.EntryTests
{
    public abstract class FixedStopTargetExitTest : TestBase
    {
        public double TargetDistance { get; }
        public double StopDistance { get; }
        protected double _stopPrice { get; set; }
        protected double _targetPrice { get; set; }

        protected FixedStopTargetExitTest(double target_distance, double stop_distance) {
            TargetDistance = target_distance;
            StopDistance = stop_distance;
        }

        protected override void SetResult(BidAskData[] data, int i) {
            for (int j = i; i < data.Length; j++)
                if (ParseConditionals(data, j))
                    break;
        }

        private bool ParseConditionals(BidAskData[] data, int x) {
            _currentTrade.Continue(data[x]);
            return !_currentTrade.isActive;
        }

        protected abstract int SetStopAndTarget(BidAskData[] data, int i);

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
        public LongFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance, stop_distance) {
        }

        protected override int SetStopAndTarget(BidAskData[] data, int i) {
            var exitPrices = new ExitPrices(StopDistance, TargetDistance);
            _currentTrade = new LongTradeGenerator(i, new TradePrices(exitPrices, data[i].Open_Ask), AddTrade);
            return i;
        }
    }

    public class ShortFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public ShortFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance, stop_distance) {
        }

        protected override int SetStopAndTarget(BidAskData[] data, int i) {
            var exitPrices = new ExitPrices(StopDistance, TargetDistance);
            _currentTrade = new ShortTradeGenerator(i, new TradePrices(exitPrices, data[i].Open_Ask), AddTrade);
            return i;
        }
    }
}