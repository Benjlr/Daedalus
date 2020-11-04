using Logic.Metrics.EntryTests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

            var threadSafeDict = new ConcurrentDictionary<int, ITest []>(FixedBarTestsToDictionary(options));
            ExecuteFixedBarTests(strat, market, threadSafeDict);
            return threadSafeDict.Values.ToList();
        }

        private static void ExecuteFixedBarTests(Strategy strat, Market market, ConcurrentDictionary<int, ITest[]> threadSafeDict)
        {
            Parallel.For(0, threadSafeDict.Count, (i) =>
            {
                threadSafeDict.TryGetValue(i, out ITest[] myTests);
                myTests[0].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                myTests[1].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
            });
        }

        private static Dictionary<int, ITest[]> FixedBarTestsToDictionary(FixedBarExitTestOptions options)
        {
            Dictionary<int, ITest[]> retval = new Dictionary<int, ITest[]>();
            for (int i = options.MinimumExitPeriod; i < options.MaximumExitPeriod; i++) 
                retval.Add(i - options.MinimumExitPeriod, FixedBarTestArrayInitiliser(i));
            return retval;
        }

        private static ITest[] FixedBarTestArrayInitiliser(int i)
        {
            ITest[] myTest = new ITest[2]
            {
                new LongFixedBarExitTest(i),
                new ShortFixedBarExitTest(i),
            };
            return myTest;
        }

        public static List<ITest[]> GenerateFixedStopTargetExitTest(Strategy strat, Market market, FixedStopTargetExitTestOptions options) {
            var threadSafeDict = new ConcurrentDictionary<int, ITest[]>(StopTargetTestsToDictionary(options));
            ExecuteFixedBarTests(strat, market, threadSafeDict);
            return threadSafeDict.Values.ToList();
        }


        private static ITest[] StopTargetTestArrayInitiliser(FixedStopTargetExitTestOptions options, int i)
        {
            ITest[] myTest = new ITest[2]
            {
                new LongFixedStopTargetExitTest(i * options.Increment + options.MinimumTarget, options.MinimumStop + i * options.Increment), 
                new ShortFixedStopTargetExitTest(i * options.Increment + options.MinimumTarget, options.MinimumStop + i * options.Increment)
            };
            return myTest;
        }

        private static Dictionary<int, ITest[]> StopTargetTestsToDictionary(FixedStopTargetExitTestOptions options)
        {
            Dictionary<int, ITest[]> retval = new Dictionary<int, ITest[]>();
            for (int i = 0; i <= options.Range / options.Increment; i++)
                retval.Add(i, StopTargetTestArrayInitiliser(options, i));
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
