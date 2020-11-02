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
        protected override void SetRuns(int i) {
            var runIndex = new int[_endIndex+1];
            for (int j = i; j <= i + _endIndex; j++) runIndex[j - i] = j;
            RunIndices.Add(runIndex);
        }

        protected override void SetResult(MarketData[] data, int i) {
            var x = i;
            SetStopAndTarget(data, i);
            while (x < data.Length) {
                if (ParseConditionals(data, i, x)) break;
                x++;
            }

            _endIndex = x-i+1;
        }

        private bool ParseConditionals(MarketData[] data, int i, int x) {
            return OpenExceedsStop(data, i, x) ||
                   OpenExceedsTarget(data, i, x) ||
                   StopHit(data, i, x) ||
                   TargetHit(data, i, x);
        }
        protected abstract bool OpenExceedsStop(MarketData[] data, int openIndex, int currentIndex);
        protected abstract bool OpenExceedsTarget(MarketData[] data, int openIndex, int currentIndex);
        protected abstract bool StopHit(MarketData[] data, int openIndex, int currentIndex);
        protected abstract bool TargetHit(MarketData[] data, int openIndex, int currentIndex);
        protected abstract void SetStopAndTarget(MarketData[] data, int i);

    }

    public class LongFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public LongFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance, stop_distance) {
        }

        protected override void SetStopAndTarget(MarketData[] data, int i) {
            _stopPrice = data[i].Open_Ask - data[i].Open_Ask * StopDistance;
            _targetPrice = data[i].Open_Ask + data[i].Open_Ask * TargetDistance;
        }
        protected override bool OpenExceedsStop(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Open_Bid < _stopPrice) 
                FBEResults[openIndex] = (data[currentIndex].Open_Bid - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Open_Bid < _stopPrice;
        }

        protected override bool OpenExceedsTarget(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Open_Bid > _targetPrice) 
                FBEResults[openIndex] = (data[currentIndex].Open_Bid - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Open_Bid > _targetPrice;
        }

        protected override bool StopHit(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Low_Bid < _stopPrice)
                FBEResults[openIndex] = (_stopPrice - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].Low_Bid < _stopPrice;
        }

        protected override bool TargetHit(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].High_Bid > _targetPrice)
                FBEResults[openIndex] = (_targetPrice - data[openIndex].Open_Ask) / data[openIndex].Open_Ask;
            return data[currentIndex].High_Bid > _targetPrice;
        }

        protected override void IterateTime(MarketData[] data, int i) {
            if (_endIndex + i > data.Length) FBEDrawdown[i] = 0;
            else if (FBEResults[i] < 0) FBEDrawdown[i] = FBEResults[i];
            else
                for (int j = i; j < i + _endIndex; j++)
                    if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                        FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
            _endIndex = 0;
        }
    }

    public class ShortFixedStopTargetExitTest : FixedStopTargetExitTest
    {
        public ShortFixedStopTargetExitTest(double target_distance, double stop_distance) : base(target_distance, stop_distance) {
        }
        protected override void SetStopAndTarget(MarketData[] data, int i) {
            _stopPrice = data[i].Open_Bid + data[i].Open_Bid * StopDistance;
            _targetPrice = data[i].Open_Bid - data[i].Open_Bid* TargetDistance;
        }

        protected override bool OpenExceedsStop(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Open_Ask > _stopPrice) 
                FBEResults[openIndex] = (data[openIndex].Open_Bid - data[currentIndex].Open_Ask) / data[openIndex].Open_Bid;
            return data[currentIndex].Open_Ask > _stopPrice;
        }

        protected override bool OpenExceedsTarget(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Open_Ask < _targetPrice) 
                FBEResults[openIndex] = (data[openIndex].Open_Bid - data[currentIndex].Open_Ask) / data[openIndex].Open_Bid;
            return data[currentIndex].Open_Ask < _targetPrice;
        }

        protected override bool StopHit(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].High_Ask > _stopPrice) 
                FBEResults[openIndex] = (data[openIndex].Open_Bid - _stopPrice) / data[openIndex].Open_Bid;
            return data[currentIndex].High_Ask > _stopPrice;
        }

        protected override bool TargetHit(MarketData[] data, int openIndex, int currentIndex)
        {
            if(data[currentIndex].Low_Ask < _targetPrice) 
                FBEResults[openIndex] = (data[openIndex].Open_Bid - _targetPrice) / data[openIndex].Open_Bid;
            return data[currentIndex].Low_Ask < _targetPrice;
        }


        protected override void IterateTime(MarketData[] data, int i)
        {
            if (_endIndex + i > data.Length) FBEDrawdown[i] = 0;
            else if (FBEResults[i] < 0) FBEDrawdown[i] = FBEResults[i];
            else
                for (int j = i; j < i + _endIndex; j++)
                    if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                        FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
            Durations[i] = _endIndex;
            _endIndex = 0;

        }

    }

}