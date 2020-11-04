using LinqStatistics;
using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        public double[] FBEResults { get; protected set; }
        public double[] FBEDrawdown { get; protected set; }
        public double[] FBEDrawdownWinners { get; protected set; }
        public double[] Durations { get; protected set; }

        public double AverageGain { get; protected set; }
        public double AverageLoss { get; protected set; }
        public double AverageDrawdown { get; protected set; }
        public double AverageDrawdownWinners { get; protected set; }

        public double MedianGain { get; protected set; }
        public double MedianLoss { get; protected set; }
        public double MedianDrawDown { get; protected set; }
        public double MedianDrawDownWinners { get; protected set; }

        public double WinPercentage { get; protected set; }

        public double ExpectancyAverage { get; protected set; }
        public double ExpectancyMedian { get; protected set; }
        protected int _endIndex { get; set; }

        public void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        {
            initLists(data.Length);
            IterateEntries(data, entries);
            if (ValidTest())
                GenerateStats();
        }

        protected void initLists(int length)
        {
            FBEResults = new double[length];
            FBEDrawdown = new double[length];
            FBEDrawdownWinners = new double[length];
            Durations = new double[length];
        }

        protected void IterateEntries(MarketData[] data, bool[] entries)
        {
            for (int i = 1; i < entries.Length - _endIndex; i++) {
                if (entries[i - 1])
                    PerformEntryActions(data, i);
            }
        }

        protected void PerformEntryActions(MarketData[] data, int i)
        {
            SetResult(data, i);
            FindDrawdown(data, i);
        }

        protected abstract void SetResult(MarketData[] data, int i);

        protected bool ValidTest()
        {
            return FBEResults.Count(x => x != 0) > 50;
        }
        
        private void GenerateStats()
        {
            FindWinPercentage();
            FindAverages();
            FindMedians();
            FindExpectancy();
        }

        private void FindWinPercentage()
        {
            WinPercentage = FBEResults.Count(x => x > 0) / (double)FBEResults.Count(x => Math.Abs(x) > 0);
        }

        private void FindAverages()
        {
            FindAverage();
            FindAverageDD();
        }

        private void FindAverage()
        {
            if (FBEResults.Any(x => x > 0))
                AverageGain = FBEResults.Where(x => x > 0).Average();
            if (FBEResults.Any(x => x < 0))
                AverageLoss = FBEResults.Where(x => x < 0).Average();
        }

        private void FindAverageDD()
        {
            if (FBEDrawdown.Any(x => x < 0))
                AverageDrawdown = FBEDrawdown.Where(x => x < 0).Average();
            if (FBEDrawdownWinners.Any(x => x < 0))
                AverageDrawdownWinners = FBEDrawdownWinners.Where(x => x < 0).Average();
        }

        private void FindMedians()
        {
            FindMedian();
            FindMedianDD();
        }

        private void FindMedian()
        {
            if (FBEResults.Any(x => x > 0))
                MedianGain = FBEResults.Where(x => x > 0).Median();
            if (FBEResults.Any(x => x < 0))
                MedianLoss = FBEResults.Where(x => x < 0).Median();
        }

        private void FindMedianDD()
        {
            if (FBEDrawdown.Any(x => x < 0))
                MedianDrawDown = FBEDrawdown.Where(x => x < 0).Median();
            if (FBEDrawdownWinners.Any(x => x < 0))
                MedianDrawDownWinners = FBEDrawdownWinners.Where(x => x < 0).Median();
        }

        protected void FindDrawdown(MarketData[] data, int i)
        {
            IterateTime(data, i);
            if (FBEResults[i] < FBEDrawdown[i])
                FBEDrawdown[i] = FBEResults[i];
            if (FBEResults[i] > 0)
                FBEDrawdownWinners[i] = FBEDrawdown[i];
        }

        protected abstract void IterateTime(MarketData[] data, int i);

        private void FindExpectancy()
        {
            ExpectancyAverage = (AverageGain * WinPercentage) / (-AverageLoss * (1 - WinPercentage));
            ExpectancyMedian = (MedianGain * WinPercentage) / (-MedianLoss * (1 - WinPercentage));
        }

    }
}
