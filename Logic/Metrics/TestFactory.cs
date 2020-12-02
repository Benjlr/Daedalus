using DataStructures;
using Logic.Metrics.CoreTests;
using Logic.Metrics.EntryTests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Action = System.Action;

namespace Logic.Metrics
{
    public interface TestOption
    {
        public ITest[] Run(Strategy strat, Market market, MarketSide longShort);
    }

    public class TestFactory
    {
        public static Action progress;

        public class FixedBarExitTestOptions : TestOption
        {
            public int MinimumExitPeriod { get; }
            public int MaximumExitPeriod { get; }
            public int Increment { get; }

            public FixedBarExitTestOptions(int minExit, int maxExit, int increment) {
                MinimumExitPeriod = minExit;
                MaximumExitPeriod = maxExit;
                Increment = increment;
            }

            public ITest[] Run(Strategy strat, Market market, MarketSide longShort) {
                return TestFactory.GenerateFixedBarExitTest(strat, market, this, longShort);
            }
        }

        public class FixedStopTargetExitTestOptions : TestOption
        {
            public double MinimumStop { get; }
            public double MinimumTarget { get; }
            public double Increment { get; }
            public double Range { get; }
            public int Divisions { get; }

            public FixedStopTargetExitTestOptions(double minStop, double minTarget, double range, int intervals) {
                MinimumStop = minStop;
                MinimumTarget = minTarget;
                Range = range;
                Divisions = intervals;
                if (intervals != 0.0)
                    Increment = range / (intervals);
                else
                    Increment = 0;
            }
            public ITest[] Run(Strategy strat, Market market, MarketSide longShort) {
                return TestFactory.GenerateFixedStopTargetExitTest(strat, market, this, longShort);
            }
        }

        public class RandomExitTestOptions : TestOption
        {
            public int TestCount { get; } 
            public int MaxBars { get; } 

            public RandomExitTestOptions(int testCount, int maxLength) {
                TestCount = testCount;
                MaxBars = maxLength;
            }
            public ITest[] Run(Strategy strat, Market market, MarketSide longShort) {
                return TestFactory.GenerateRandomExitTests(strat, market, this, longShort);
            }
        }

        private static ITest[] GenerateFixedBarExitTest(Strategy strat, Market market, FixedBarExitTestOptions options, MarketSide longShort) {
            if (options.MinimumExitPeriod > options.MaximumExitPeriod)
                throw new Exception();
            var threadSafeDict = new ConcurrentDictionary<int, ITest>(FixedBarTestsToDictionary(options, longShort));
            ExecuteTests(strat, market, threadSafeDict);
            return threadSafeDict.Values.ToArray();
        }

        private static ITest[] GenerateFixedStopTargetExitTest(Strategy strat, Market market, FixedStopTargetExitTestOptions options, MarketSide longShort)
        {
            var threadSafeDict = new ConcurrentDictionary<int, ITest>(StopTargetTestsToDictionary(options,  longShort));
            ExecuteTests(strat, market, threadSafeDict);
            return threadSafeDict.Values.ToArray();
        }

        private static ITest[] GenerateRandomExitTests( Strategy strat, Market market, RandomExitTestOptions optons, MarketSide longShort)
        {
            var threadSafeDict = new ConcurrentDictionary<int, ITest>(RandomExitTestsToDictionary(optons, longShort));
            ExecuteTests(strat, market, threadSafeDict);
            return threadSafeDict.Values.ToArray();
        }


        private static void ExecuteTests(Strategy strat, Market market, ConcurrentDictionary<int, ITest> threadSafeDict)
        {
            Parallel.For(0, threadSafeDict.Count, (i) =>
            {
                threadSafeDict.TryGetValue(i, out ITest myTests);
                myTests?.Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                progress?.Invoke();
            });
        }

        private static Dictionary<int, ITest> FixedBarTestsToDictionary(FixedBarExitTestOptions options, MarketSide longShort) {
            Dictionary<int, ITest> retval = new Dictionary<int, ITest>();
            for (int i = options.MinimumExitPeriod; i <= options.MaximumExitPeriod; i+= options.Increment) 
                retval.Add(retval.Count, FixedBarExitTest.PrepareTest(longShort, i));
            return retval;
        }

        private static ITest[] RandomExitTestArrayInitialiser(int maxLength) {
            ITest[] myTest = new ITest[2]
            {
                new LongRandomExitTest(maxLength), 
                new ShortRandomExitTest(maxLength)
            };
            return myTest;
        }

        private static Dictionary<int, ITest> StopTargetTestsToDictionary(FixedStopTargetExitTestOptions options, MarketSide longShort) {
            Dictionary<int, ITest> retval = new Dictionary<int, ITest>();
            for (int i = 0; i <= options.Divisions; i++)
            for (int j = 0; j <= options.Divisions; j++)
                retval.Add(retval.Count,
                    FixedStopTargetExitTest.PrepareTest(longShort, 
                        j * options.Increment + options.MinimumTarget, 
                        options.MinimumStop + i * options.Increment));
            return retval;
        }
        private static Dictionary<int, ITest> RandomExitTestsToDictionary(RandomExitTestOptions options, MarketSide longShort) {
            Dictionary<int, ITest> retval = new Dictionary<int, ITest>();
            for (int i = 0; i <= options.TestCount; i++)
                retval.Add(i, RandomExitTest.PrepareTest(longShort, options.MaxBars));
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
