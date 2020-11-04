using Logic.Utils;
using System;
using System.Collections.Generic;
using PriceSeriesCore.FinancialSeries;

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

        //public override void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        //{
        //    FBEResults = new double[data.Length];
        //    for (int i = 0; i < entries.Length - 1; i++)
        //    {
        //        if (entries[i])
        //        {
        //            var x = i + 1;
        //            double randDist = -1;
        //            while (randDist < 0)
        //            {
        //                randDist = BoxMullerDistribution.Generate(_mean, _sDev);
        //            }
        //            var fbel = (int)Math.Round(randDist);

        //            if (x + fbel < data.Length && x + fbel > 0)
        //            {
        //                double entryPriceBull = data[x].Open_Ask;
        //                FBEResults[i] = data[x + fbel].Open_Bid - entryPriceBull;
        //            }

        //        }
        //    }
        //}

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
