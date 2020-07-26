using Logic.Metrics.EntryTests;
using System;
using Logic.Metrics.CoreTests;

namespace Logic.Metrics
{
    public class TestFactory
    {
        public static ITest[] GenerateFixedBarExitTest(int start, int end, Strategy strat, Market market)
        {
            if(start > end) throw new Exception();

            ITest[] retval = new ITest[end-start];

            for (int i = start; i < end; i++)
            {
                retval[i-start] = new FixedBarExitTest(i);
                retval[i-start].Run(market.RawData, strat.Entries);
            }

            return retval;
        }

        public static ITest[] GenerateFixedStopTargetExitTest(int lowestStop, int highestStop, int lowestTarget, int highestTarget, Strategy strat, Market market)
        {

            ITest[] retval = new ITest[(lowestStop - highestStop) * (highestTarget - lowestTarget)];

            for (int i = highestStop; i < lowestStop; i++)
            {
                for (int j = lowestTarget; j < highestTarget; j++)
                {
                    retval[(i-highestStop) * (highestTarget - lowestTarget) + (j - lowestTarget)] = new FixedStopTargetExitTest(j,i,false);
                    retval[(i - highestStop) * (highestTarget - lowestTarget) + (j - lowestTarget)].Run(market.RawData, strat.Entries);

                }
            }

            return retval;
        }

        public static ITest[] GenerateRandomExitTests(double mean, double standDev, int iterations, Strategy strat, Market market)
        {
            ITest[] retval = new ITest[iterations];

            for (int i = 0; i < iterations; i++)
            {
                retval[i] = new RandomExitTest(mean, standDev); 
                retval[i].Run(market.RawData, strat.Entries);

            }

            return retval;
        }

        public static RangeTest GenerateRangeTest(int rangesToTest, Strategy strat, Market market)
        {
            RangeTest retval = new RangeTest(rangesToTest);
            retval.Run(market.RawData, strat);
            return retval;
        }
    }
}
