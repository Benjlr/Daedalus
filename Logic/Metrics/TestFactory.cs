using Logic.Metrics.EntryTests;
using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Metrics.CoreTests;

namespace Logic.Metrics
{
    public class TestFactory
    {
        public static List<ITest> GenerateFixedBarExitTest(int start, int end, Strategy strat, Market market, int increment = 1)
        {
            if(start > end) throw new Exception();

            List<ITest> retval = new List<ITest>();
            for (int i = start; i < end; i+=increment)
            {
                var myTest = new FixedBarExitTest(i);
                myTest.Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                retval.Add(myTest);
            }

            return retval;
        }

        public static ITest[] GenerateFixedStopTargetExitTest(int lowestStop, int highestStop, int lowestTarget, int highestTarget, Strategy strat, Market market)
        {
            List<ITest> retval = new List<ITest>();

            for (int i = highestStop; i < lowestStop; i+=3)
            {
                for (int j = lowestTarget; j < highestTarget; j+=3)
                {
                    retval.Add( new FixedStopTargetExitTest(j,i,false));
                    retval.Last().Run(market.RawData, strat.Entries);

                }
            }

            return retval.ToArray();
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
            retval.Run(market.RawData, strat, 10000, 5);
            return retval;
        }

        public static MonteCarloTest GenerateMonteCarloTest(int iteration, Strategy strat, Market market, double initialCapital, double dollarsPerPoint)
        {
            var mt = new MonteCarloTest();
            mt.Run(strat,market,initialCapital,dollarsPerPoint, iteration);
            return mt;
        }
    }
}
