using System.Collections.Generic;
using PriceSeries.FinancialSeries;

namespace Logic.Metrics
{
    public interface ITest
    {
        public double[] AtrsUp { get; set; }
        public double[] AtrsDown { get; set; }
        public double[] FBELong { get; set; }
        public double[] FBEDrawdown { get; set; }
  

        public double AverageDD { get; }
        public double MedianDrawDown { get; }

        public double AverageLossLong { get; }
        public double AverageGainLong { get; }
        public double MedianLossLong { get; }
        public double MedianGainLong { get; }

        public double WinPercentageLong { get; }
        public double WinPercentageShort { get; }

        public double ExpectancyLongAverage { get; }
        public double ExpectancyLongMedian { get; }

        public double ExpectancyShortAverage { get; }


        void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null);
    }
}
