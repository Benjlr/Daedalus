using Logic.Analysis.Metrics;
using Logic.Metrics;
using Logic.Strategies;
using Logic.Utils;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Logic.Tests
{
    public class EntryDrilldownTests
    {
        private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


        public EntryDrilldownTests()
        {
            var market = MarketBuilder.CreateMarket(marketData);
            var strat = StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1));
        }

        [Fact]
        private void ShouldGenerateRollingExpectancyOverSmallPeriodAvg() {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpResults.txt",2);
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpData.txt");
            var ten = ExpectancyTools.GetRollingExpectancy(TestData, 10);
            Assert.Equal(resultsLong[0].Select(TestUtils._round), ten.Select(x => TestUtils._round(x.AverageExpectancy)));
        }

        [Fact]
        private void ShouldGenerateRollingExpectancyOverSmallPeriodMed() {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpResults.txt", 2);
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpData.txt");
            var ten = ExpectancyTools.GetRollingExpectancy(TestData, 10);
            Assert.Equal(resultsLong[1].Select(TestUtils._round), ten.Select(x => TestUtils._round(x.MedianExpectancy)));
        }


        [Fact]
        private void ShouldGenerateRollingExpectancyOverLongPeriodAvg() {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpResults.txt",2);
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpData.txt"); 
             var thirteen = ExpectancyTools.GetRollingExpectancy(TestData, 13);
             Assert.Equal(resultsLong[0].Select(TestUtils._round), thirteen.Select(x => TestUtils._round(x.AverageExpectancy)));
        }
        [Fact]
        private void ShouldGenerateRollingExpectancyOverLongPeriodMed() {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpResults.txt",2);
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpData.txt");
            var thirteen = ExpectancyTools.GetRollingExpectancy(TestData, 13);
            Assert.Equal(resultsLong[1].Select(TestUtils._round), thirteen.Select(x => TestUtils._round(x.MedianExpectancy)));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverSmallEpochSplitAvg() {
            var resultsThreePeriod = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt",2);
            var ThreeEpoch = ExpectancyTools.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(ThreeEpoch.Select(x => TestUtils._round(x.AverageExpectancy)), resultsThreePeriod[0].Select(TestUtils._round));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverSmallEpochSplitMed() {
            var resultsThreePeriod = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt",2);
            var ThreeEpoch = ExpectancyTools.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(ThreeEpoch.Select(x => TestUtils._round(x.MedianExpectancy)), resultsThreePeriod[1].Select(TestUtils._round));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverLargerEpochSplitAvg() {
            var resultsTenPeriod = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt",2);
            var tenEpoch = ExpectancyTools.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            Assert.Equal(resultsTenPeriod[0].Select(TestUtils._round), tenEpoch.Select(x=>TestUtils._round(x.AverageExpectancy)));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverLargerEpochSplitMed() {
            var resultsTenPeriod = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt",2);
            var tenEpoch = ExpectancyTools.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            Assert.Equal(resultsTenPeriod[1].Select(TestUtils._round), tenEpoch.Select(x => TestUtils._round(x.MedianExpectancy)));
        }

        [Fact]
        private void GeneratesBoundedStats() {
            var myLIst = new List<double>();
            for (int i = 0; i <= 100; i++) myLIst.Add(i);
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(0, myStat.Minimum);
            Assert.Equal(100, myStat.Maximum);
            Assert.Equal(50, myStat.Average);
            Assert.Equal(50, myStat.Median);
            Assert.Equal(90, myStat.Upper);
            Assert.Equal(10, myStat.Lower);
        }

        [Fact]
        private void ShouldGenerateEpoch() {
            List<double> myList = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, -89, -0.5, 56, 7, 0.1, -0.1};
            var epochOne = EpochGenerator.SplitListIntoEpochs(myList, 5);
            Assert.Equal(5, epochOne.EpochContainer.Count);
            Assert.Equal(new List<double>() {1}, epochOne.EpochContainer[0]);
            Assert.Equal(new List<double>() {23, 2.3, 5}, epochOne.EpochContainer[1]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochOne.EpochContainer[2]);
            Assert.Equal(new List<double>() {-89, -0.5, 56}, epochOne.EpochContainer[3]);
            Assert.Equal(new List<double>() {7, 0.1, -0.1}, epochOne.EpochContainer[4]);
        }

        [Fact]
        private void ShouldGenerateEvenEpoch() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};
            var epochTwo = EpochGenerator.SplitListIntoEpochs(myListTwo, 4);
            Assert.Equal(4, epochTwo.EpochContainer.Count);
            Assert.Equal(new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4}, epochTwo.EpochContainer[0]);
            Assert.Equal(new List<double>() {12, 3, 0.4, -89, -0.5, 56, 12}, epochTwo.EpochContainer[1]);
            Assert.Equal(new List<double>() {3, 0.4, 7, 0.1, -0.1, 12, 3}, epochTwo.EpochContainer[2]);
            Assert.Equal(new List<double>() {0.4, 12, 3, 0.4, 12, 3, 0.4}, epochTwo.EpochContainer[3]);
        }

        [Fact]
        private void ShouldGenerateUnequalEpochs() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};

            var epochThree = EpochGenerator.SplitListIntoEpochs(myListTwo, 9);
            Assert.Equal(9, epochThree.EpochContainer.Count);
            Assert.Equal(new List<double>() {1, 23, 2.3, 5}, epochThree.EpochContainer[0]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[1]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[2]);
            Assert.Equal(new List<double>() {-89, -0.5, 56}, epochThree.EpochContainer[3]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[4]);
            Assert.Equal(new List<double>() {7, 0.1, -0.1}, epochThree.EpochContainer[5]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[6]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[7]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[8]);
        }

        [Fact]
        private void ShouldGenerateMassiveEpoch() {
            List<double> myListThree = new List<double>();
            for (int i = 0; i < 12345; i++) myListThree.Add(0);
            var epochFour = EpochGenerator.SplitListIntoEpochs(myListThree, 29);
            Assert.Equal(myListThree.Count % (29 - 1), epochFour.EpochContainer[0].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[1].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[21].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[3].Count);
            Assert.Equal(12345, epochFour.EpochContainer.Sum(x => x.Count));
        }

        private List<double> _testList = new List<double>(){-1,-1,-1,-0.5,-0.5,-0.5,0,0,0,0.5,0.5,0.5,1,1,1};
        private List<double> _testList2 = new List<double>(){1,1,1,0.5,0.5,0.5,0,0,0,0.5,0.5,0.5,1,1,1};
        private List<double> _testList3 = new List<double>() {123, -45, 0.02, 12, 99, -89, 123, 122.4, -450.55, 450, -0.002, 0.003, 0.05, 12, 3, -42};

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


