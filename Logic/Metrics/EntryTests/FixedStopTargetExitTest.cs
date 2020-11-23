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

        protected FixedStopTargetExitTest(double target_distance, double stop_distance)
        {
            TargetDistance = target_distance;
            StopDistance = stop_distance;
        }

        protected override void SetResult(BidAskData[] data, int i) {
            I(data, i, SetStopAndTarget(data, i));
        }

        private void I(BidAskData[] data, int i, int x){
            while (x < data.Length) {
                if (ParseConditionals(data, i, x)) break;
                x++;
            }
        }

        private bool ParseConditionals(BidAskData[] data, int i, int x) {
            CalculateReturn(data,i,x);
            return OpenExceedsStop(data, i, x) ||
                   OpenExceedsTarget(data, i, x) ||
                   StopHit(data, i, x) ||
                   TargetHit(data, i, x);
        }

        protected abstract void CalculateReturn(BidAskData[] data, int i, int x);
        protected abstract bool OpenExceedsStop(BidAskData[] data, int openIndex, int currentIndex);
        protected abstract bool OpenExceedsTarget(BidAskData[] data, int openIndex, int currentIndex);
        protected abstract bool StopHit(BidAskData[] data, int openIndex, int currentIndex);
        protected abstract bool TargetHit(BidAskData[] data, int openIndex, int currentIndex);
        protected abstract int SetStopAndTarget(BidAskData[] data, int i);

        public static FixedStopTargetExitTest PrepareTest(MarketSide longShort, double target_distance, double stop_distance)
        {
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
            _stopPrice = data[i].Open_Ask - data[i].Open_Ask * StopDistance;
            _targetPrice = data[i].Open_Ask + data[i].Open_Ask * TargetDistance;
            return i;
        }

        protected override void CalculateReturn(BidAskData[] data, int i, int x) {
                _currentTrade.Add((data[x].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask);
        }
        protected override bool OpenExceedsStop(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Open_Bid < _stopPrice)
                _currentTrade[^0] =(data[currentIndex].Open_Bid - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Open_Bid < _stopPrice;
        }

        protected override bool OpenExceedsTarget(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Open_Bid > _targetPrice)
                _currentTrade[^0] = (data[currentIndex].Open_Bid - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Open_Bid > _targetPrice;
        }

        protected override bool StopHit(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Low_Bid < _stopPrice)
                _currentTrade[^0] = (_stopPrice - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Low_Bid < _stopPrice;
        }

        protected override bool TargetHit(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].High_Bid > _targetPrice)
                _currentTrade[^0] = (_targetPrice - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].High_Bid > _targetPrice;
        }
    }

    public class ShortFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public ShortFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance, stop_distance) {
        }
        protected override int SetStopAndTarget(BidAskData[] data, int i) {
            _stopPrice = data[i].Open_Bid + data[i].Open_Bid * StopDistance;
            _targetPrice = data[i].Open_Bid - data[i].Open_Bid* TargetDistance;
            return i;
        }

        protected override void CalculateReturn(BidAskData[] data, int i, int x) {
            _currentTrade.Add((data[i].Open_Bid - data[x].Open_Ask) / data[i].Open_Bid);
        }

        protected override bool OpenExceedsStop(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Open_Ask > _stopPrice)
                _currentTrade[^0] = (data[openIndex].Open_Bid - data[currentIndex].Open_Ask) / data[openIndex].Open_Bid;
            return data[currentIndex].Open_Ask > _stopPrice;
        }

        protected override bool OpenExceedsTarget(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Open_Ask < _targetPrice)
                _currentTrade[^0] = (data[openIndex].Open_Bid - data[currentIndex].Open_Ask) / data[openIndex].Open_Bid;
            return data[currentIndex].Open_Ask < _targetPrice;
        }

        protected override bool StopHit(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].High_Ask > _stopPrice)
                _currentTrade[^0] = (data[openIndex].Open_Bid - _stopPrice) / data[openIndex].Open_Bid;
            return data[currentIndex].High_Ask > _stopPrice;
        }

        protected override bool TargetHit(BidAskData[] data, int openIndex, int currentIndex) {
            if(data[currentIndex].Low_Ask < _targetPrice)
                _currentTrade[^0] = (data[openIndex].Open_Bid - _targetPrice) / data[openIndex].Open_Bid;
            return data[currentIndex].Low_Ask < _targetPrice;
        }
    }

}