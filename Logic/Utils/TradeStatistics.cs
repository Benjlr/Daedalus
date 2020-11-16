using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LinqStatistics;

namespace Logic.Utils
{
    public class TradeStatistics
    {
        public double WinPercent { get; private set; }
        public double AvgGain { get; private set; }
        public double AvgLoss { get; private set; }
        public double MedianGain { get; private set; }
        public double MedianLoss { get; private set; }
        public double AverageExpectancy { get; private set; }
        public double MedianExpectancy { get; private set; } 
        public double SharpeRatio { get; private set; }


        public TradeStatistics(List<double> range)
        {
            CalculateGain(range);
            CalculateLoss(range);
            CalculateWinPercent(range);
            if (range.Count > 0) CalculateExpectancy();
            if (range.Any(x => x != 0)) CalculateSharpeRatio(range.Where(x=>x!=0).ToList());
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
            var denominator = (double)range.Count(x => Math.Abs(x) > 0);
            WinPercent = numerator / denominator;
        }

        private void CalculateExpectancy() {
            AverageExpectancy = AvgGain * WinPercent + (AvgLoss * (1 - WinPercent));
            MedianExpectancy = MedianGain * WinPercent + (MedianLoss * (1 - WinPercent));
        }

        private void CalculateSharpeRatio(List<double> range) {
            SharpeRatio = range.Sum() / range.StandardDeviation();
        }

    }

    public class ExtendedStats : TradeStatistics
    {
        public double AverageDrawdown { get; private set; }
        public double AverageDrawdownWinners { get; private set; }
        public double MedianDrawDown { get; private set; }
        public double MedianDrawDownWinners { get; private set; }

        public ExtendedStats(List<double> range, List<double> drawdownRange) : base(range)
        {
            CalculateDrawdown(range, drawdownRange);
            CalculateDrawdownWinners(range, drawdownRange);
        }

        private void CalculateDrawdown(List<double> range, List<double> drawdownRange) {
            if (drawdownRange.Any(x => x < 0)) {
                AverageDrawdown = drawdownRange.Where(x => x < 0).Average();
                MedianDrawDown = drawdownRange.Where(x => x < 0).Median();
            }
        }

        private void CalculateDrawdownWinners(List<double> range, List<double> drawdownRange) {
            var drawdowns = GetWinningTradeDrawdowns(range, drawdownRange);
            if (drawdowns.Any(x => x < 0)) {
                AverageDrawdownWinners = drawdowns.Where(x => x < 0).Average();
                MedianDrawDownWinners = drawdowns.Where(x => x < 0).Median();
            }
        }

        private List<double> GetWinningTradeDrawdowns(List<double> range, List<double> drawdownRange) {
            var drawdowns = new List<double>();
            for (int i = 0; i < range.Count; i++)
                if (range[i] > 0) drawdowns.Add(drawdownRange[i]);
            return drawdowns;
        }
    }
}