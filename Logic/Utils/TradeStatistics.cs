using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using LinqStatistics;
using Logic.Analysis.Metrics;

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
        public double Sortino { get; private set; }


        public TradeStatistics(List<Trade> trades) {
            var results = trades.Select(x => x.Results.Last()).ToList();
            CalculateGain(results.Where(x=>x>0).ToList());
            CalculateLoss(results.Where(x => x < 0).ToList());
            CalculateWinPercent(results);
            if (trades.Count > 0) CalculateExpectancy();
            if (results.Count(x => x < 0) > 2) CalculateSharpeRatio(results);
        }

        private void CalculateGain(List<double> results) {
            if (results.Count > 0) {
                MedianGain = results.Median();
                AvgGain = results.Average();
            }
        }

        private void CalculateLoss(List<double> results) {
            if (results.Count > 0) {
                MedianLoss = results.Median();
                AvgLoss = results.Average();
            }
        }

        private void CalculateWinPercent(List<double> results) {
            var numerator = results.Count(x => x > 0);
            var denominator = (double)results.Count(x => Math.Abs(x) > 0);
            WinPercent = numerator / denominator;
        }

        private void CalculateExpectancy() {
            AverageExpectancy = AvgGain * WinPercent + (AvgLoss * (1 - WinPercent));
            MedianExpectancy = MedianGain * WinPercent + (MedianLoss * (1 - WinPercent));
        }

        private void CalculateSharpeRatio(List<double> results) {
            Sortino = results.Sum() / results.Where(x => x < 0).StandardDeviation();
        }

    }

    public class ExtendedStats : TradeStatistics
    {
        public double AverageDrawdown { get; private set; }
        public double AverageDrawdownWinners { get; private set; }
        public double MedianDrawDown { get; private set; }
        public double MedianDrawDownWinners { get; private set; }

        public ExtendedStats(List<Trade> trades) : base(trades)
        {
            var results = trades.SelectMany(x => x.Results).ToList();
            results = results.Where(x => x < 0).ToList();
            CalculateDrawdown(results);
            CalculateDrawdownWinners(trades);
        }

        private void CalculateDrawdown(List<double> results) {
            if (results.Count > 0) {
                AverageDrawdown = results.Average();
                MedianDrawDown = results.Median();
            }
        }

        private void CalculateDrawdownWinners(List<Trade> trades) {
            var drawdowns = GetWinningTradeDrawdowns(trades);
            if (drawdowns.Count > 0) {
                AverageDrawdownWinners = drawdowns.Average();
                MedianDrawDownWinners = drawdowns.Median();
            }
        }

        private List<double> GetWinningTradeDrawdowns(List<Trade> trades) {
            var drawdowns = new List<double>();
            foreach (var t in trades)
                if (t.Results.Last() > 0) drawdowns.AddRange(t.Results.Where(x=>x<0));
            return drawdowns;
        }
    }
}