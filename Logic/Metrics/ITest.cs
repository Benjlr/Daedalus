using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public interface ITest
    {
        public double[] FBELong { get;  }
        public double[] FBEShort { get;  }
        public double[] FBEDrawdownLong { get;  }
        public double[] FBEDrawdownShort { get;  }
        public double[] FBEDrawdownLongWinners { get;  }
        public double[] FBEDrawdownShortWinners { get;  }

        public double AverageGainLong { get; }
        public double AverageLossLong { get; }
        public double AverageDrawdownLong { get; }
        public double AverageDrawdownWinnersLong { get; }
       
        public double MedianGainLong { get; }
        public double MedianLossLong { get; }
        public double MedianDrawDownLong { get; }
        public double MedianDrawDownWinnersLong { get; }

        public double AverageGainShort { get;  }
        public double AverageLossShort { get;  }
        public double AverageDrawdownShort { get; }
        public double AverageDrawdownWinnersShort { get; }

        public double MedianGainShort { get; }
        public double MedianLossShort { get;  }
        public double MedianDrawDownShort { get; }
        public double MedianDrawDownWinnersShort { get; }

        public double WinPercentageShort { get; }
        public double WinPercentageLong { get; }

        public double ExpectancyLongAverage { get;  }
        public double ExpectancyLongMedian { get;  }
        public double ExpectancyShortAverage { get;  }
        public double ExpectancyShortMedian { get;  }

        void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null);
    }
}
