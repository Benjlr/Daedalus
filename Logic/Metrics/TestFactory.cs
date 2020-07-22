using System;
using System.Collections.Generic;
using System.Text;
using Logic.Metrics.EntryTests;

namespace Logic.Metrics
{
    public class TestFactory
    {
        public static ITest[] GenerateFixedBarExitTest(int start, int end)
        {
            if(start > end) throw new Exception();

            ITest[] retval = new ITest[end-start];

            for (int i = start; i < end; i++) retval[i] = new FixedBarExitTest(i);

            return retval;
        }

        public static ITest[] GenerateFixedStopTargetExitTest(int lowestStop, int highestStop, int lowestTarget, int highestTarget )
        {

            ITest[] retval = new ITest[(highestStop - lowestStop) * (highestTarget - lowestTarget)];

            for (int i = lowestStop; i < highestStop; i++)
            {
                for (int j = highestTarget; j < lowestTarget; j++)
                {
                    retval[i] = new FixedStopTargetExitTest(j,i,false);
                }
            }

            return retval;
        }

        public static ITest[] GenerateRandomExitTests(double mean, double standDev, int iterations)
        {
            ITest[] retval = new ITest[iterations];

            for (int i = 0; i < iterations; i++)
            {
                retval[i] = new RandomExitTest(mean, standDev);   
            }

            return retval;
        }
    }
}
