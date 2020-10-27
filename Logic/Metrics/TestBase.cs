using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        public double[] FBEResults { get; protected set; }
        public double[] FBEDrawdown { get; protected set; }
        public double[] FBEDrawdownWinners { get; protected set; }
        public List<int[]> RunIndices { get; protected set; }

        public double AverageGain { get; protected set; }
        public double AverageLoss { get; protected set; }
        public double AverageDrawdown { get; protected set; }
        public double AverageDrawdownWinners { get; protected set; }

        public double MedianGain { get; protected set; }
        public double MedianLoss { get; protected set; }
        public double MedianDrawDown { get; protected set; }
        public double MedianDrawDownWinners { get; protected set; }

        public double WinPercentage { get; protected set; }

        public double ExpectancyAverage { get; protected set; }
        public double ExpectancyMedian { get; protected set; }

        public virtual void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        { }
    }
}
