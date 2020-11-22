using System.Collections.Generic;
using System.IO;
using DataStructures.StatsTools;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class TradeStatsTests
    {
        private List<double> _testList = new List<double>() { -1, -1, -1, -0.5, -0.5, -0.5, 0, 0, 0, 0.5, 0.5, 0.5, 1, 1, 1 };
        private List<double> _testList2 = new List<double>() { 1, 1, 1, 0.5, 0.5, 0.5, 0, 0, 0, 0.5, 0.5, 0.5, 1, 1, 1 };
        private List<double> _testList3 = new List<double>() { 123, -45, 0.02, 12, 99, -89, 123, 122.4, -450.55, 450, -0.002, 0.003, 0.05, 12, 3, -42 };


        [Fact]
        private void ShouldGenerateCorrectAverageGain() {
            Assert.Equal(0.75, new TradeStatistics(_testList).AvgGain);
            Assert.Equal(0.75, new TradeStatistics(_testList2).AvgGain);
            Assert.Equal(85.86118181818182, new TradeStatistics(_testList3).AvgGain);
        }

        [Fact]
        private void ShouldGenerateCorrectMedianGain() {
            Assert.Equal(0.75, new TradeStatistics(_testList).MedianGain);
            Assert.Equal(0.75, new TradeStatistics(_testList2).MedianGain);
            Assert.Equal(12, new TradeStatistics(_testList3).MedianGain);
        }

        [Fact]
        private void ShouldGenerateCorrectAverageLoss() {
            Assert.Equal(-0.75, new TradeStatistics(_testList).AvgLoss);
            Assert.Equal(0, new TradeStatistics(_testList2).AvgLoss);
            Assert.Equal(-125.31039999999999, new TradeStatistics(_testList3).AvgLoss);
        }

        [Fact]
        private void ShouldGenerateCorrectMedianLoss() {
            Assert.Equal(-0.75, new TradeStatistics(_testList).MedianLoss);
            Assert.Equal(0, new TradeStatistics(_testList2).MedianLoss);
            Assert.Equal(-45, new TradeStatistics(_testList3).MedianLoss);
        }

        [Fact]
        private void ShouldGenerateCorrectWinPercentage() {
            Assert.Equal(0.5, new TradeStatistics(_testList).WinPercent);
            Assert.Equal(1, new TradeStatistics(_testList2).WinPercent);
            Assert.Equal(0.6875, new TradeStatistics(_testList3).WinPercent);
        }

        [Fact]
        private void ShouldGenerateCorrectExpectancyAverage()
        {
            Assert.Equal(0, new TradeStatistics(_testList).AverageExpectancy);
            Assert.Equal(0.75, new TradeStatistics(_testList2).AverageExpectancy);
            Assert.Equal(19.870062500000003, new TradeStatistics(_testList3).AverageExpectancy);
        }

        [Fact]
        private void ShouldGenerateCorrectExpectancyMedian() {
            Assert.Equal(0, new TradeStatistics(_testList).MedianExpectancy);
            Assert.Equal(0.75, new TradeStatistics(_testList2).MedianExpectancy);
            Assert.Equal(-5.8125, new TradeStatistics(_testList3).MedianExpectancy);
        }

        [Fact]
        private void ShouldGenerateCorrectSortino() {
            Assert.Equal(0, new TradeStatistics(_testList).Sortino);
            Assert.Equal(0, new TradeStatistics(_testList2).Sortino);
            Assert.Equal(1.722954062637295, new TradeStatistics(_testList3).Sortino);
        }
    }
}