using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Metrics.EntryTests.TestsDrillDown
{
    public class EntryTestDrilldown
    {
        private static List<double> myList;
        private static List<double> myWinPercent;
        private static List<double> myAvgGain;
        private static List<double> myAvgLoss;


        public static List<double> GetRollingExpectancy(List<double> resultList, int lookbackPeriod)
        {   
            InitLocalLists(resultList, lookbackPeriod);
            for (int i = lookbackPeriod; i < resultList.Count; i++)
                IterateExpectancy(resultList.GetRange(i - lookbackPeriod, lookbackPeriod + 1).ToList(), lookbackPeriod,i);
            return new List<double>( myList);
        }

        private static void InitLocalLists(List<double> resultList, int initPeriod)
        {
            resultList = resultList.Where(x => x != 0).ToList();
            ClearList();
            AddZeroes(initPeriod);
        }

        private static void ClearList()
        {
            myWinPercent = new List<double>();
            myAvgGain = new List<double>();
            myAvgLoss = new List<double>();
            myList = new List<double>();
        }

        private static void AddZeroes(int initPeriod)
        {
            for (int i = 0; i < initPeriod; i++)
                myWinPercent.Add(0);
                myAvgGain.Add(0);
                myAvgLoss.Add(0);
                myList.Add(0);
        }

        private static void IterateExpectancy(List<double> resultsList, int lookBack, int i)
        {
            CalculateRollingStats(resultsList);
            myList.Add(myAvgGain.Last() * myWinPercent.Last() / (-myAvgLoss.Last() * (1 - myWinPercent.Last())));
            if (myList[i] > 3) myList[i] = 3.0;
        }

        private static void CalculateRollingStats(List<double> range)
        {
            if (range.Any(x => x > 0))
                myAvgGain.Add(range.Where(x => x > 0).Average());
            if (range.Any(x => x < 0))
                myAvgLoss.Add(range.Where(x => x < 0).Average());

            myWinPercent.Add(range.Count(x => x > 0) / (double)range.Count(x => Math.Abs(x) > 0));
        }
    }
}
