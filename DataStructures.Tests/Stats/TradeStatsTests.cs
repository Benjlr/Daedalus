using System.Collections.Generic;
using System.IO;
using DataStructures.StatsTools;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class TradeStatsTests
    {
        //private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";
        
        public TradeStatsTests()
        {
            //var market = Market.MarketBuilder.CreateMarket(marketData);
            //var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            //{
            //    new DummyEntries(1, 98)
            //}, market);

            //var longSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1, MarketSide.Bull));
            //var shortSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1, MarketSide.Bear));


            //myTests = new List<ITest[]>();
            //for (int i = 0; i < longSide.Count; i++)
            //    myTests.Add(new[] { longSide[i], shortSide[i] });

        }

        private List<Trade> _testList = new List<Trade>()
        {
            new Trade(new double[]{5,6,3,-1},8),
            new Trade(new double[]{7,6,3,-1},780),
            new Trade(new double[]{5,67,6,3,-1},5),
            new Trade(new double[]{5,87,3,-0.5},567),
            new Trade(new double[]{87,6,7,-0.5},8),
            new Trade(new double[]{5,6,3,-0.5},9),
            new Trade(new double[]{58,6,63,0},0),
            new Trade(new double[]{5,6,63,0},0),
            new Trade(new double[]{58,6,63,0},0),
            new Trade(new double[]{58,6,63,0.5},0),
            new Trade(new double[]{6,456,63,0.5},65),
            new Trade(new double[]{58,6,63,0.5},0),
            new Trade(new double[]{7,6,63,0},51),
            new Trade(new double[]{58,6,63,0},756),
            new Trade(new double[]{56,6,63,0},1)
        };

        private List<Trade> _testList2 = new List<Trade>()
        {
            new Trade(new double[]{5,6,3,1},8),
            new Trade(new double[]{7,6,3,1},780),
            new Trade(new double[]{5,67,6,3,1},5),
            new Trade(new double[]{5,87,3,0.5},567),
            new Trade(new double[]{87,6,7,0.5},8),
            new Trade(new double[]{5,6,3,0.5},9),
            new Trade(new double[]{87,6,3,0},2),
            new Trade(new double[]{58,6,63,0},0),
            new Trade(new double[]{58,6,63,0},0),
            new Trade(new double[]{58,6,63,0.5},0),
            new Trade(new double[]{5,6,63,0.5},0),
            new Trade(new double[]{58,6,63,0.5},0),
            new Trade(new double[]{58,6,63,1},0),
            new Trade(new double[]{6,456,63,1},65),
            new Trade(new double[]{58,6,63,1},0)
        };

        private List<Trade> _testList3 = new List<Trade>()
        {
            new Trade(new double[]{5,6,3,123},8),
            new Trade(new double[]{7,6,3,-45},780),
            new Trade(new double[]{5,67,6,3,0.02},5),
            new Trade(new double[]{5,87,3,12},567),
            new Trade(new double[]{87,6,7,99},8),
            new Trade(new double[]{5,6,3,-89},9),
            new Trade(new double[]{87,6,3,122.4},2),
            new Trade(new double[]{58,6,63,-450.55},0),
            new Trade(new double[]{58,6,63,450},0),
            new Trade(new double[]{58,6,63, -0.002},0),
            new Trade(new double[]{5,6,63,0.003},0),
            new Trade(new double[]{58,6,63,0.05},0),
            new Trade(new double[]{58,6,63,12},0),
            new Trade(new double[]{6,456,63,3},65),
            new Trade(new double[]{58,6,63,-42},0)
        };

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
    }
}


//var tempavg = tenEpoch.Select(x => x.AverageExpectancy).ToList();
//var tempmed = tenEpoch.Select(x => x.MedianExpectancy).ToList();
//System.Text.StringBuilder t = new System.Text.StringBuilder();
//for (int i = 0; i<tempavg.Count; i++) {
//t.AppendLine($"{tempavg[i]},{tempmed[i]}");
//}
//File.WriteAllText(@"C:\Temp\res.txt", t.ToString());


