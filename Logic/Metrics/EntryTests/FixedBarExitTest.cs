using LinqStatistics;
using PriceSeries.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Utils;

namespace Logic.Metrics.EntryTests
{
    public class FixedBarExitTest : TestBase
    {

        private int Fixed_Bar_exit { get; }

        public FixedBarExitTest(int bars_to_wait)
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
            FBELong = new double[length];
            FBEShort = new double[length];
            FBEDrawdownLong = new double[length];
            FBEDrawdownShort = new double[length];
            FBEDrawdownLongWinners = new double[length];
            FBEDrawdownShortWinners = new double[length];
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
            FindDrawdownLong(data, i);
            FindDrawdownShort(data, i);
        }

        private void SetResult(MarketData[] data, int i)
        {
            FBELong[i] = (data[i + Fixed_Bar_exit].Open_Bid - data[i].Open_Ask)/ data[i].Open_Ask;
            FBEShort[i] = (data[i].Open_Bid - data[i + Fixed_Bar_exit].Open_Ask)/ data[i].Open_Bid;
        }

        private void SetRuns(int i)
        {
            var runIndex = new int[ Fixed_Bar_exit+1];
            for (int j = i; j <= i + Fixed_Bar_exit; j++) runIndex[j - i] = j;
            RunIndices.Add(runIndex);
        }

        private void FindDrawdownLong(MarketData[] data, int i)
        {
            IterateTimeLong(data, i);
            if (FBELong[i] < FBEDrawdownLong[i])
                FBEDrawdownLong[i] = FBELong[i];
            if (FBELong[i] > 0)
                FBEDrawdownLongWinners[i] = FBEDrawdownLong[i];
        }

        private void IterateTimeLong(MarketData[] data, int i)
        {
            for (int j = i; j < i + Fixed_Bar_exit; j++)
                if ((data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask < FBEDrawdownLong[i])
                    FBEDrawdownLong[i] = (data[j].Low_Bid - data[i].Open_Ask) / data[i].Open_Ask;
        }

        private void FindDrawdownShort(MarketData[] data, int i)
        {
            IterateTimeShort(data, i);
            if (FBEShort[i] < FBEDrawdownShort[i])
                FBEDrawdownShort[i] = FBEShort[i];
            if (FBEShort[i] > 0)
                FBEDrawdownShortWinners[i] = FBEDrawdownShort[i];
        }

        private void IterateTimeShort(MarketData[] data, int i)
        {
            for (int j = i; j < i + Fixed_Bar_exit; j++)
                if ((data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid < FBEDrawdownShort[i])
                    FBEDrawdownShort[i] = (data[i].Open_Bid - data[j].High_Ask) / data[i].Open_Bid;
        }

        private bool ValidTest()
        {
            return FBELong.Count(x => x != 0) > 50;
        }

        private void GenerateStats()
        {
            FindWinPercentage();
            FindAverages();
            FindMedians();
            FindExpectancy();
            FindRollingExpectancy();
        }

        private void FindRollingExpectancy()
        {
            ExpectancyByPositionLong();
            ExpectancyByPositionShort();
        }




        private int _period = 150;
        private List<double> myList;
        private List<double> myWinPercent = new List<double>();
        private List<double> myAvgGain = new List<double>();
        private List<double> myAvgLoss = new List<double>();

        private void ExpectancyByPositionLong()
        {
            InitLocalLists(FBELong);
            ExpectancyByPositionInSeriesLongAverage = new double[myList.Count];

            for (int i = _period; i < myList.Count; i++)
            {
                var range = myList.GetRange(i - _period, _period+1).ToList();
                CalculateRollingStats(range);
                ExpectancyByPositionInSeriesLongAverage[i] = (myAvgGain.Last() * myWinPercent.Last()) / (-myAvgLoss.Last() * (1 - myWinPercent.Last()));
                if (ExpectancyByPositionInSeriesLongAverage[i] > 3) ExpectancyByPositionInSeriesLongAverage[i] = 3.0;
            }
        }

        private void CalculateRollingStats(List<double> range)
        {
            if (range.Any(x => x > 0))
                myAvgGain.Add(range.Where(x => x > 0).Average());
            if (range.Any(x => x < 0))
                myAvgLoss.Add(range.Where(x => x < 0).Average());
            myWinPercent.Add(range.Count(x => x > 0) / (double) range.Count(x => Math.Abs(x) > 0));
        }

        private void InitLocalLists(double[] resultList)
        {
            myList = resultList.Where(x => x != 0).ToList();

            for (int i = 0; i < _period; i++)
            {
                myWinPercent.Add(0);
                myAvgGain.Add(0);
                myAvgLoss.Add(0);
            }
        }

        private void ExpectancyByPositionShort()
        {
            InitLocalLists(FBEShort);
            ExpectancyByPositionInSeriesShortAverage = new double[myList.Count];

            for (int i = _period; i < myList.Count; i++)
            {
                var range = myList.GetRange(i - _period, _period+1).ToList();
                CalculateRollingStats(range);
                ExpectancyByPositionInSeriesShortAverage[i] = (myAvgGain.Last() * myWinPercent.Last()) / (-myAvgLoss.Last() * (1 - myWinPercent.Last()));
                if (ExpectancyByPositionInSeriesShortAverage[i] > 3) ExpectancyByPositionInSeriesShortAverage[i] = 3.0;

            }
        }

        private void FindWinPercentage()
        {
            WinPercentageLong = FBELong.Count(x => x > 0) / (double)FBELong.Count(x => Math.Abs(x) > 0);
            WinPercentageShort = FBEShort.Count(x => x > 0) / (double)FBEShort.Count(x => Math.Abs(x) > 0);
        }

        private void FindAverages()
        {
            FindAverage();
            FindAverageDD();
        }

        private void FindAverage()
        {
            if (FBELong.Any(x => x > 0))
                AverageGainLong = FBELong.Where(x => x > 0).Average();
            if (FBELong.Any(x => x < 0))
                AverageLossLong = FBELong.Where(x => x < 0).Average();
            if (FBEShort.Any(x => x > 0))
                AverageGainShort = FBEShort.Where(x => x > 0).Average();
            if (FBEShort.Any(x => x < 0))
                AverageLossShort = FBEShort.Where(x => x < 0).Average();
        }

        private void FindAverageDD()
        {
            if (FBEDrawdownLong.Any(x => x < 0))
                AverageDrawdownLong = FBEDrawdownLong.Where(x => x < 0).Average();
            if (FBEDrawdownShort.Any(x => x < 0))
                AverageDrawdownShort = FBEDrawdownShort.Where(x => x < 0).Average();
            if (FBEDrawdownLongWinners.Any(x => x < 0))
                AverageDrawdownWinnersLong = FBEDrawdownLongWinners.Where(x => x < 0).Average();
            if (FBEDrawdownShortWinners.Any(x => x < 0))
                AverageDrawdownWinnersShort = FBEDrawdownShortWinners.Where(x => x < 0).Average();
        }

        private void FindMedians()
        {
            FindMedian();
            FindMedianDD();
        }

        private void FindMedian()
        {
            if (FBELong.Any(x => x > 0))
                MedianGainLong = FBELong.Where(x => x > 0).Median();
            if (FBELong.Any(x => x < 0))
                MedianLossLong = FBELong.Where(x => x < 0).Median();
            if (FBEShort.Any(x => x > 0))
                MedianGainShort = FBEShort.Where(x => x > 0).Median();
            if (FBEShort.Any(x => x < 0))
                MedianLossShort = FBEShort.Where(x => x < 0).Median();
        }

        private void FindMedianDD()
        {
            if (FBEDrawdownLong.Any(x => x < 0))
                MedianDrawDownLong = FBEDrawdownLong.Where(x => x < 0).Median();
            if (FBEDrawdownShort.Any(x => x < 0))
                MedianDrawDownShort = FBEDrawdownShort.Where(x => x < 0).Median();
            if (FBEDrawdownLongWinners.Any(x => x < 0))
                MedianDrawDownWinnersLong = FBEDrawdownLongWinners.Where(x => x < 0).Median();
            if (FBEDrawdownShortWinners.Any(x => x < 0))
                MedianDrawDownWinnersShort = FBEDrawdownShortWinners.Where(x => x < 0).Median();
        }

        private void FindExpectancy()
        {
            ExpectancyLongAverage = (AverageGainLong * WinPercentageLong) / (-AverageLossLong * (1 - WinPercentageLong));
            ExpectancyLongMedian = (MedianGainLong * WinPercentageLong) / (-MedianLossLong * (1 - WinPercentageLong));
            ExpectancyShortAverage = (AverageGainShort * WinPercentageShort) / (-AverageLossShort * (1 - WinPercentageShort));
            ExpectancyShortMedian = (MedianGainShort * WinPercentageShort) / (-MedianLossShort * (1 - WinPercentageShort));
        }
    }
}
