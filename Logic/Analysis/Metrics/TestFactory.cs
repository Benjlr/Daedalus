using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Analysis.Metrics.EntryTests;
using Logic.Metrics;
using Logic.Metrics.CoreTests;
using Logic.Utils;

namespace Logic.Analysis.Metrics
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
        public int Divisions { get; }

        public FixedStopTargetExitTestOptions(double minStop, double minTarget, double range, int intervals)
        {
            MinimumStop = minStop;
            MinimumTarget = minTarget;
            Range = range;
            Divisions = intervals;
            Increment = range / (intervals);
        }
    }

    public class TestFactory
    {
        public static List<ITest[]> GenerateFixedBarExitTest(Strategy strat, Market market, FixedBarExitTestOptions options, System.Action progress = null)
        {
            IntialisationInformation.ChangeStage($"Executing Fixed Bar Tests");

            if (options.MinimumExitPeriod > options.MaximumExitPeriod)
                throw new Exception();

            var threadSafeDict = new ConcurrentDictionary<int, ITest []>(FixedBarTestsToDictionary(options));
            ExecuteTests(strat, market, threadSafeDict, progress);
            return threadSafeDict.Values.ToList();
        }

        public static List<ITest[]> GenerateFixedStopTargetExitTest(Strategy strat, Market market, FixedStopTargetExitTestOptions options, System.Action progress = null)
        {
            IntialisationInformation.ChangeStage($"Executing Stop Target Tests");

            var threadSafeDict = new ConcurrentDictionary<int, ITest[]>(StopTargetTestsToDictionary(options));
            ExecuteTests(strat, market, threadSafeDict, progress);
            return threadSafeDict.Values.ToList();
        }

        public static List<ITest[]> GenerateRandomExitTests( Strategy strat, Market market, int iterations, int maxLength, System.Action progress=null)
        {
            var threadSafeDict = new ConcurrentDictionary<int, ITest[]>(RandomExitTestsToDictionary(iterations, maxLength));
            ExecuteTests(strat, market, threadSafeDict, progress);
            return threadSafeDict.Values.ToList();
        }


        private static void ExecuteTests(Strategy strat, Market market, ConcurrentDictionary<int, ITest[]> threadSafeDict, System.Action progress)
        {
            IntialisationInformation.ChangeTotalCount(threadSafeDict.Count); 
            Parallel.For(0, threadSafeDict.Count, (i) =>
            {
                threadSafeDict.TryGetValue(i, out ITest[] myTests);
                myTests[0].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                myTests[1].Run(market.RawData, strat.Entries, market.CostanzaData.ToList());
                progress?.Invoke();
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

        private static ITest[] StopTargetTestArrayInitiliser(FixedStopTargetExitTestOptions options, int i,int j) {
            ITest[] myTest = new ITest[2]
            {
                new LongFixedStopTargetExitTest(j * options.Increment + options.MinimumTarget, options.MinimumStop + i * options.Increment), 
                new ShortFixedStopTargetExitTest(j * options.Increment + options.MinimumTarget, options.MinimumStop + i * options.Increment)
            };
            return myTest;
        }

        private static ITest[] RandomExitTestArrayInitiliser(int maxLength) {
            ITest[] myTest = new ITest[2]
            {
                new LongRandomExitTest(maxLength), 
                new ShortRandomExitTest(maxLength)
            };
            return myTest;
        }

        private static Dictionary<int, ITest[]> StopTargetTestsToDictionary(FixedStopTargetExitTestOptions options) {
            Dictionary<int, ITest[]> retval = new Dictionary<int, ITest[]>();
            for (int i = 0; i <= options.Range / options.Increment; i++)
            for (int j = 0; j <= options.Range / options.Increment; j++)
                retval.Add(j + i * ((int)(options.Range / options.Increment)+1), StopTargetTestArrayInitiliser(options, i,j));
            return retval;
        }
        private static Dictionary<int, ITest[]> RandomExitTestsToDictionary(int testCount, int maxLength) {
            Dictionary<int, ITest[]> retval = new Dictionary<int, ITest[]>();
            for (int i = 0; i <= testCount; i++)
                retval.Add(i, RandomExitTestArrayInitiliser(maxLength));
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
