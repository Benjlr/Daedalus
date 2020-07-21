using Logic.Utils;
using System;

namespace Logic.Metrics.EntryTests
{
    public class RandomExitTest : TestBase
    {

        // Random Exit

        private double _mean { get; }
        private double _sDev { get; }

        public RandomExitTest(double mean, double stdev)
        {
            _mean = mean;
            _sDev = stdev;
        }

        public void RunRE(MarketData[] data, bool[] entries)
        {
            FBELong = new double[data.Length];
            FBEShort = new double[data.Length];

            for (int i = 0; i < entries.Length - 1; i++)
            {
                if (entries[i])
                {
                    var x = i + 1;
                    double randDist = -1;
                    while (randDist < 0)
                    {
                        randDist = BoxMullerDistribution.Generate(_mean, _sDev);
                    }
                    var fbel = (int)Math.Round(randDist);

                    if (x + fbel < data.Length && x + fbel > 0)
                    {
                        double entryPriceBull = data[x].Open_Ask;
                        FBELong[i] = data[x + fbel].Open_Bid - entryPriceBull;

                        double entryPriceBear = data[x].Open_Bid;
                        FBEShort[i] = entryPriceBear - data[x + fbel].Open_Ask;
                    }

                }
            }
        }
    }
}
