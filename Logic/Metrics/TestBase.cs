using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        public double[] FBELong { get; protected set; }
        public double[] FBEShort { get; protected set; }
        public double[] FBEDrawdownLong { get; protected set; }
        public double[] FBEDrawdownShort { get; protected set; }
        public double[] FBEDrawdownLongWinners { get; protected set; }
        public double[] FBEDrawdownShortWinners { get; protected set; }
        public List<int[]> RunIndices { get; set; }

        public double AverageGainLong { get; protected set; }
        public double AverageLossLong { get; protected set; }
        public double AverageDrawdownLong { get; protected set; }
        public double AverageDrawdownWinnersLong { get; protected set; }

        public double MedianLossLong { get; protected set; }
        public double MedianGainLong { get; protected set; }
        public double MedianDrawDownLong { get; protected set; }
        public double MedianDrawDownWinnersLong { get; protected set; }

        public double AverageGainShort { get; protected set; }
        public double AverageLossShort { get; protected set; }
        public double AverageDrawdownShort { get; set; }
        public double AverageDrawdownWinnersShort { get; protected set; }

        public double MedianGainShort { get; protected set; }
        public double MedianLossShort { get; protected set; }
        public double MedianDrawDownShort { get; set; }
        public double MedianDrawDownWinnersShort { get; protected set; }

        public double WinPercentageShort { get; protected set; }
        public double WinPercentageLong { get; protected set; }

        public double ExpectancyLongAverage { get; protected set; }
        public double ExpectancyLongMedian { get; protected set; }
        public double ExpectancyShortAverage { get; protected set; }
        public double ExpectancyShortMedian { get; protected set; }
        public double[] ExpectancyByPositionInSeriesLongAverage { get; protected set; }
        public double[] ExpectancyByPositionInSeriesShortAverage { get; protected set; }

        public virtual void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        { }
    }
}
