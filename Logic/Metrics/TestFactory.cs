using Logic.Metrics.EntryTests;
using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Metrics.CoreTests;

namespace Logic.Metrics
{
    public class FixedBarExitTestOptions
    {
        public int MinimumExitPeriod { get;}
        public int MaximumExitPeriod { get; }
        public int Increment { get; }

        public FixedBarExitTestOptions(int minExit, int maxExit, int increment)
        {
            MinimumExitPeriod = minExit;
            MaximumExitPeriod = maxExit;
            Increment = increment;
        }
    }

    public class FixedStopTargetExitTestOptions
    {
        public double MinimumStop { get; }
        public double MinimumTarget { get; }
        public double Increment { get; }
        public double Range { get; }
        private int _divisions { get; }

        public FixedStopTargetExitTestOptions(double minStop, double minTarget, double range, int intervals)
        {
            MinimumStop = minStop;
            MinimumTarget = minTarget;
            Range = range;
            _divisions = intervals;
            Increment = range / (intervals-1);
        }
    }

    public class TestFactory
    {
        public static List<ITest[]> GenerateFixedBarExitTest(Strategy strat, Market market, FixedBarExitTestOptions options)
        {
            if(options.MinimumExitPeriod > options.MaximumExitPeriod)
                throw new Exception();

            List<ITest[]> retval = new List<ITest[]>();
            for (int i = options.MinimumExitPeriod; i < options.MaximumExitPeriod; i+= options.Increment) {
                ITest[] myTest = new ITest[2] {
                     new LongFixedBarExitTest(i),
                     new ShortFixedBarExitTest(i),
                };
                myTest[0].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                myTest[1].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                retval.Add(myTest);
            }

            return retval;
        }

        public static List<ITest[]> GenerateFixedStopTargetExitTest(Strategy strat, Market market, FixedStopTargetExitTestOptions options)
        {
            List<ITest[]> retval = new List<ITest[]>();

            for (double i = 0; i <= options.Range; i += options.Increment) {
                    ITest[] myTest = new ITest[2] {
                        new LongFixedStopTargetExitTest(i  + options.MinimumTarget, options.MinimumStop + i ),
                        new ShortFixedStopTargetExitTest(options.MinimumTarget + i , options.MinimumStop + i )
                    };
                    myTest[0].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                    myTest[1].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                    retval.Add(myTest);
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
