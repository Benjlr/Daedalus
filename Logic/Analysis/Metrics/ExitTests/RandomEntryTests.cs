using System;
using Logic.Metrics;
using Logic.Utils;
using PriceSeriesCore;

namespace Logic.Analysis.Metrics.ExitTests
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
            FBEResults= new double[data.Length];

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
                        FBEResults[i] = exitPriceBull - data[x - fbel].Open_Ask;

                    }
                }
            }
        }

        protected override void SetResult(MarketData[] data, int i)
        {
            throw new NotImplementedException();
        }


        protected override void IterateTime(MarketData[] data, int i)
        {
            throw new NotImplementedException();
        }
    }
}
