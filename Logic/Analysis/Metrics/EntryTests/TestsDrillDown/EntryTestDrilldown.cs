using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using Logic.Utils;

namespace Logic.Analysis.Metrics.EntryTests.TestsDrillDown
{
    public class EntryTestDrilldown
    {

        public static List<DrillDownStats> GetRollingExpectancy(List<double> resultList, int lookbackPeriod)
        {
            return RunThroughResultSet(resultList, lookbackPeriod);            
        }

        public static List<DrillDownStats> GetExpectancyByEpoch(List<double> resultList, int divisions)
        {
            return IterateThroughEpochs(EpochGenerator.SplitListIntoEpochs(resultList,divisions).EpochContainer);
        }


        private static List<DrillDownStats> IterateThroughEpochs(List<List<double>> epochs)
        {
            var retval = new List<DrillDownStats>();
            foreach (var epoch in epochs) {
                var vals = epoch.Where(x => x != 0).ToList();
                retval.Add(IterateExpectancy(vals));
            }
            return retval;
        }

        private static List<DrillDownStats> AddOnes(int initPeriod) {
            var myList = new List<DrillDownStats>();
            for (int i = 0; i < initPeriod; i++)
                myList.Add(new DrillDownStats(new List<double>()));
            return myList;
        }

        private static List<DrillDownStats> RunThroughResultSet(List<double> resultList, int lookbackPeriod)
        {
            var indexThresh = ListTools.GetIndexAtThresholdNonZeroes(lookbackPeriod, resultList);
            var retVal = AddOnes(indexThresh);
            List<double> validResults = new List<double>();

            for (int i = 0; i < indexThresh; i++)
            {
                if(resultList[i] != 0) validResults.Add(resultList[i]);
            }

            for (int i = retVal.Count; i < resultList.Count; i++)  
                if(resultList[i] == 0) retVal.Add(retVal.Last());
                else
                {
                    if(validResults.Count >= lookbackPeriod )validResults.RemoveAt(0);
                    validResults.Add(resultList[i]);
                    retVal.Add(IterateExpectancy(validResults));
                }
            return retVal;
        }

        private static DrillDownStats IterateExpectancy(List<double> resultsList) {
            if (resultsList.Count == 0) return new DrillDownStats(new List<double>());
            return new DrillDownStats(resultsList);
        }
    }

    public class DrillDownStats
    {
        public double WinPercent { get; private set; }
        public double AvgGain { get; private set; }
        public double AvgLoss { get; private set; }
        public double MedianGain { get; private set; }
        public double MedianLoss { get; private set; }
        public double AverageExpectancy { get; private set; } = 1;
        public double MedianExpectancy { get; private set; } = 1;

        public DrillDownStats(List<double> range) {
            CalculateGain(range);
            CalculateLoss(range);
            CalculateWinPercent(range);
            if(range.Count > 0)CalculateExpectancy();
        }

        private void CalculateGain(List<double> range) {
            if (range.Any(x => x > 0)) {
                MedianGain = range.Where(x => x > 0).Median();
                AvgGain = range.Where(x => x > 0).Average();
            }
        }

        private void CalculateLoss(List<double> range) {
            if (range.Any(x => x < 0)) {
                MedianLoss = range.Where(x => x < 0).Median();
                AvgLoss = range.Where(x => x < 0).Average();
            }
        }

        private void CalculateWinPercent(List<double> range) {
            var numerator = range.Count(x => x > 0);
            var denominator = (double) range.Count(x => Math.Abs(x) > 0);
            WinPercent = numerator / denominator;
        }

        private void CalculateExpectancy() {
            AverageExpectancy = this.AvgGain * this.WinPercent/ (-this.AvgLoss * (1 - this.WinPercent));
            if (AverageExpectancy > 3 || double.IsInfinity(AverageExpectancy)) AverageExpectancy = 3.0;
            MedianExpectancy = this.MedianGain * this.WinPercent / (-this.MedianLoss * (1 - this.WinPercent));
            if (MedianExpectancy > 3 || double.IsInfinity(MedianExpectancy)) MedianExpectancy = 3.0;
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
