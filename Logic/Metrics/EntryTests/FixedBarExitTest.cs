using LinqStatistics;
using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Metrics.EntryTests
{
    public class FixedBarExitTest : TestBase
    {

        protected int Fixed_Bar_exit { get;  }

        protected FixedBarExitTest(int bars_to_wait)
        {
            Fixed_Bar_exit = bars_to_wait;
        }
        
        public override void Run(MarketData[] data, bool[] entries, List<Session> myInputs)
        {
            initLists(data.Length);
            IterateEntries(data, entries);
            if(ValidTest())
                GenerateStats();
        }

        private void initLists(int length)
        {
            FBEResults = new double[length];
            FBEDrawdown = new double[length];
            FBEDrawdownWinners = new double[length];
            RunIndices = new List<int[]>();
        }

        private void IterateEntries(MarketData[] data, bool[] entries)
        {
            for (int i = 1; i < entries.Length - Fixed_Bar_exit; i++)
                if (entries[i - 1])
                    PerformEntryActions(data, i);
        }

        private void PerformEntryActions(MarketData[] data, int i)
        {
            SetResult(data, i);
            SetRuns(i);
            FindDrawdown(data, i);
        }

        protected virtual void SetResult(MarketData[] data, int i)
        { throw new Exception("instantiate long or short version"); }

        private void SetRuns(int i)
        {
            var runIndex = new int[ Fixed_Bar_exit+1];
            for (int j = i; j <= i + Fixed_Bar_exit; j++) runIndex[j - i] = j;
            RunIndices.Add(runIndex);
        }

        private void FindDrawdown(MarketData[] data, int i)
        {
            IterateTime(data, i);
            if (FBEResults[i] < FBEDrawdown[i])
                FBEDrawdown[i] = FBEResults[i];
            if (FBEResults[i] > 0)
                FBEDrawdownWinners[i] = FBEDrawdown[i];
        }

        protected virtual void IterateTime(MarketData[] data, int i)
        { throw new Exception("instantiate long or short version"); }

        private bool ValidTest()
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

        private void FindExpectancy()
        {
            ExpectancyAverage = (AverageGain * WinPercentage) / (-AverageLoss * (1 - WinPercentage));
            ExpectancyMedian = (MedianGain * WinPercentage) / (-MedianLoss * (1 - WinPercentage));
        }
    }
    
    public class LongFixedBarExitTest : FixedBarExitTest
    {
        public LongFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        { }

    protected override void SetResult(MarketData[] data, int i)
        {
            FBEResults[i] = (data[i + Fixed_Bar_exit].Open_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        protected override void IterateTime(MarketData[] data, int i)
        {
            for (int j = i; j < i + Fixed_Bar_exit; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

    }

    public class ShortFixedBarExitTest : FixedBarExitTest
    {
        public ShortFixedBarExitTest(int bars_to_wait) : base(bars_to_wait)
        { }
        protected override void SetResult(MarketData[] data, int i)
        {
            FBEResults[i] = (data[i].Open_Bid - data[i + Fixed_Bar_exit].Open_Ask) / data[i].Open_Bid;
        }


        protected override void IterateTime(MarketData[] data, int i)
        {
            for (int j = i; j < i + Fixed_Bar_exit; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdown[i])
                    FBEDrawdown[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
        }

    }

}
