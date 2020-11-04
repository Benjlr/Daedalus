using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public interface ITest
    {
        public double[] FBEResults { get;  }
        public double[] FBEDrawdown { get;  }
        public double[] FBEDrawdownWinners { get; }
        public double[] Durations { get; }

        public double AverageGain { get; }
        public double AverageLoss { get; }
        public double AverageDrawdown { get; }
        public double AverageDrawdownWinners { get; }
       
        public double MedianGain { get; }
        public double MedianLoss { get; }
        public double MedianDrawDown { get; }
        public double MedianDrawDownWinners { get; }

        public double WinPercentage { get; }

        public double ExpectancyAverage { get;  }
        public double ExpectancyMedian { get;  }

        void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null);
    }
}
