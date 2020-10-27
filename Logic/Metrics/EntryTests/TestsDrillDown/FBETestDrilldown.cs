using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Metrics.EntryTests.TestsDrillDown
{
    public class EntryTestDrilldown
    {

        public static List<double> GetRollingExpectancy(List<double> resultList, int lookbackPeriod)
        {
            return RunThroughResultSet(resultList.Where(x => x != 0).ToList(), lookbackPeriod);            
        }

        public static List<double> GetExpectancyByEpoch(List<double> resultList, int divisions)
        {
            return IterateThroughEpochs(SplitResultsIntoEpochs(resultList, divisions));
        }

        private static List<List<double>> SplitResultsIntoEpochs(List<double> resultList, int divisions)
        {
            var resultsByEpoch = new List<List<double>>();
            for (int i = 0; i < divisions; i++)
                resultsByEpoch.Add(ListTools.GetNewList(resultList, i * resultList.Count / divisions, (i + 1) * resultList.Count / divisions));
            return resultsByEpoch;
        }

        private static List<double> IterateThroughEpochs(List<List<double>> epochs)
        {
            var retval = new List<double>();
            foreach (var epoch in epochs) {
                var vals = epoch.Where(x => x != 0).ToList();
                retval.Add(IterateExpectancy(vals));
            }
            return retval;
        }

        private static List<double> AddOnes(int initPeriod)
        {
            var myList = new List<double>();
            for (int i = 0; i < initPeriod; i++)
                myList.Add(1);
            return myList;
        }

        private static List<double> RunThroughResultSet(List<double> resultList, int lookbackPeriod)
        {
            var retVal = AddOnes(lookbackPeriod);
            for (int i = lookbackPeriod; i < resultList.Count; i++)
                retVal.Add(IterateExpectancy(resultList.GetRange(i - lookbackPeriod, lookbackPeriod + 1).ToList()));
            return retVal;
        }

        private static double myWinPercent;
        private static double myAvgGain;
        private static double myAvgLoss;

        private static void CalculateRollingStats(List<double> range)
        {
            if (range.Any(x => x > 0))
                myAvgGain = range.Where(x => x > 0).Average();
            if (range.Any(x => x < 0))
                myAvgLoss = range.Where(x => x < 0).Average();

            myWinPercent = range.Count(x => x > 0) / (double)range.Count(x => Math.Abs(x) > 0);
        }

        private static double IterateExpectancy(List<double> resultsList)
        {
            if (resultsList.Count == 0) return 1;

            CalculateRollingStats(resultsList);
            return CalculateExpectancy();
        }

        private static double CalculateExpectancy()
        {
            var expectancy = myAvgGain * myWinPercent / (-myAvgLoss * (1 - myWinPercent));

            if (expectancy > 3) expectancy = 3.0;
            return expectancy;
        }

    }
}
