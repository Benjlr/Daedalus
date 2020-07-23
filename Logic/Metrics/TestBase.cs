using System;
using System.Linq;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        protected double[] FBELong { get; set; }
        protected double[] FBEShort { get; set; }

        public double AverageGainLong => FBELong.Any(x => x > 0) ? FBELong.Where(x => x > 0).Average() : 0;
        public double AverageGainShort => FBEShort.Any(x => x > 0) ? FBEShort.Where(x => x > 0).Average() : 0;
        public double AverageLossLong => FBELong.Any(x => x < 0) ? FBELong.Where(x => x < 0).Average() : 0;
        public double AverageLossShort => FBEShort.Any(x => x < 0) ? FBEShort.Where(x => x < 0).Average() : 0;

        public double WinPercentageLong => (double) FBELong.Count(x => x > 0) / (double) FBELong.Count(x => Math.Abs(x) > 0);
        public double WinPercentageShort => (double) FBEShort.Count(x => x > 0) / (double) FBEShort.Count(x => Math.Abs(x) > 0);

        public double ExpectancyLong => (AverageGainLong * WinPercentageLong) / (-AverageLossLong * (1 - WinPercentageLong));
        public double ExpectancyShort => (AverageGainShort * WinPercentageShort) / (-AverageLossShort * (1 - WinPercentageShort));

        public virtual void Run(MarketData[] data, bool[] entries)
        {

        }
    }
}
