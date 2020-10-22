using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using PriceSeries.FinancialSeries;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {

        public double[] FBELong { get; set; }
        public double[] AtrsUp { get; set; }
        public double[] AtrsDown { get; set; }
        public double[] FBEDrawdown { get; set; }
        protected double[] FBEShort { get; set; }

        public double AverageDD => FBEDrawdown.Any(x => x < 0) ? FBEDrawdown.Where(x => x < 0).Average(): 0;
        public double MedianDrawDown => FBEDrawdown.Any(x => x < 0) ? FBEDrawdown.Where(x => x < 0).Median():0; 


        public double AverageLossLong { get; set; }
        public double AverageGainLong { get; set; }
        public double MedianLossLong { get; set; }
        public double MedianGainLong { get; set; }


        public double AverageGainShort => FBEShort.Any(x => x > 0) ? FBEShort.Where(x => x > 0).Average() : 0;
        public double AverageLossShort => FBEShort.Any(x => x < 0) ? FBEShort.Where(x => x < 0).Average() : 0;

        public double WinPercentageShort { get; set; }
        public double WinPercentageLong { get; set; }

        public double ExpectancyLongAverage => _ExpectancyLongAverage();
        public double _ExpectancyLongAverage ()
        {
            var tbo = -AverageLossLong * (1 - WinPercentageLong) == 0.0 ?  3.0 : (AverageGainLong * WinPercentageLong) / (-AverageLossLong * (1 - WinPercentageLong)) ;
            if (double.IsNaN(tbo))
            {
                string a = "";
            }

            return tbo;
        }

        public double ExpectancyLongMedian => -MedianLossLong * (1 - WinPercentageLong) == 0.0 ? 3.0 : (MedianGainLong * WinPercentageLong) / (-MedianLossLong * (1 - WinPercentageLong));

        public double ExpectancyShortAverage => (AverageGainShort * WinPercentageShort) / (-AverageLossShort * (1 - WinPercentageShort));


        public virtual void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        {

        }
    }
}
