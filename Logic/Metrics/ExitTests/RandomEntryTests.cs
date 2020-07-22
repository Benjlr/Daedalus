using System;
using Logic.Utils;

namespace Logic.Metrics.ExitTests
{
    public class RandomEntryTests : TestBase
    {

        // Random Entry

        private double _mean { get; }
        private double _sDev { get; }


        public RandomEntryTests(double meanLong, double stdLong)
        {
            _mean= meanLong;
            _sDev = stdLong;
        }

        public void RunRE(MarketData[] data, bool[] exits)
        {
            FBELong= new double[data.Length];
            FBEShort = new double[data.Length];

            for (int i = 0; i < exits.Length - 1; i++)
            {
                if (exits[i])
                {
                    var x = i + 1;

                    double randDist = -1;
                    while (randDist < 0)
                    {
                        randDist = BoxMullerDistribution.Generate(_mean, _sDev);
                    }

                    var fbel = (int)Math.Round(randDist);
                    if ( x-fbel > 0 && x-fbel < data.Length )
                    {
                        double exitPriceBull = data[x].Open_Bid;
                        FBELong[i] = exitPriceBull - data[x - fbel].Open_Ask;

                        double exitPriceBear = data[x].Open_Ask;
                        FBEShort[i] = data[x - fbel].Open_Bid - exitPriceBear;
                    }
                }
            }
        }
    }
}
