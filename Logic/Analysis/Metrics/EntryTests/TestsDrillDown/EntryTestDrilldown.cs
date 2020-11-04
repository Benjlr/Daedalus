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
            return RunThroughResultSet(resultList, lookbackPeriod);            
        }

        public static List<double> GetExpectancyByEpoch(List<double> resultList, int divisions)
        {
            return IterateThroughEpochs(EpochGenerator.SplitListIntoEpochs(resultList,divisions).EpochContainer);
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

        private static List<double> AddOnes(int initPeriod) {
            var myList = new List<double>();
            for (int i = 0; i < initPeriod; i++)
                myList.Add(1);
            return myList;
        }

        private static List<double> RunThroughResultSet(List<double> resultList, int lookbackPeriod) {
            var retVal = AddOnes(ListTools.GetIndexAtThresholdNonZeroes(lookbackPeriod, resultList));
            for (int i = retVal.Count; i < resultList.Count; i++)  
                retVal.Add(IterateExpectancy(ListTools.GetLastNnonZeroValues(lookbackPeriod, i, resultList)));
            return retVal;
        }

        private static double IterateExpectancy(List<double> resultsList) {
            if (resultsList.Count == 0) return 1;
            return new DrillDownStats(resultsList).Expectancy;
        }
    }

    public class DrillDownStats
    {
        public double WinPercent { get; }
        public double AvgGain { get; }
        public double AvgLoss { get; }
        public double Expectancy { get; }

        public DrillDownStats(List<double> range) {
            AvgGain = CalculateAvgGain(range);
            AvgLoss = CalculateAvgLoss(range);
            WinPercent = CalculateWinPercent(range);
            Expectancy = CalculateExpectancy();
        }

        private double CalculateAvgGain(List<double> range) {
            if (range.Any(x => x > 0))
                return range.Where(x => x > 0).Average();
            return 0;
        }

        private double CalculateAvgLoss(List<double> range) {
            if (range.Any(x => x < 0))
                return range.Where(x => x < 0).Average();
            return 0;
        }

        private double CalculateWinPercent(List<double> range) {
            var numerator = range.Count(x => x > 0);
            var denominator = (double) range.Count(x => Math.Abs(x) > 0);
            return  numerator / denominator;
        }

        private double CalculateExpectancy() {
            var expectancy = this.AvgGain * this.WinPercent/ (-this.AvgLoss * (1 - this.WinPercent));
            if (expectancy > 3 || double.IsInfinity(expectancy)) expectancy = 3.0;
            return expectancy;
        }
    }

    public class EpochGenerator {
        public int Period { get; set; }
        public int Remainder { get; set; }
        public List<List<double>> EpochContainer { get; set; }

        private EpochGenerator(int count, int divisions) {
            if (count % divisions == 0)
            {
                Remainder = 0;
                Period = count / divisions;
            }
            else
            {
                Period = count / (divisions - 1);
                Remainder = count % (divisions - 1);
            }
            EpochContainer = new List<List<double>>();
        }

        public static EpochGenerator SplitListIntoEpochs(List<double> list, int divisions)
        {
            var myEpoch = new EpochGenerator(list.Count, divisions);
            myEpoch.GenerateEpochs(list);
            return myEpoch;
        }

        private void GenerateEpochs(List<double> list) {
            InitialiseFirstEpoch(list);
            GenerateRemainingEpochs(list);
        }

        private void InitialiseFirstEpoch(List<double> list)
        {
            if (Remainder > 0)
                EpochContainer.Add(ListTools.GetNewListByStartIndexAndCount(list, 0, Remainder));
        }

        private void GenerateRemainingEpochs(List<double> list) {
            for (int i = Remainder; i < list.Count; i += Period) {
                EpochContainer.Add(ListTools.GetNewListByIndex(list, i, i + Period - 1));
            }
        }

    }
}
