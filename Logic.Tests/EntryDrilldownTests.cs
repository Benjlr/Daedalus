using System.Collections.Generic;
using System.IO;
using System.Linq;
using Logic.Analysis.Metrics;
using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;
using Logic.Metrics;
using Logic.Rules.Entry;
using Logic.Utils;
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
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[]
            {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1));
        }

        [Fact]
        private void ShouldGenerateRollingExpectancyOverSmallPeriod() {
            var resultsLong = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpResults.txt");
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpData.txt");
            var ten = EntryTestDrilldown.GetRollingExpectancy(TestData, 10);
            Assert.Equal(resultsLong.Select(TestUtils._round), ten.Select(TestUtils._round));
        }

        [Fact]
        private void ShouldGenerateRollingExpectancyOverLongPeriod() {
            var resultsLong = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpResults.txt");
            var TestData = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpData.txt"); 
             var thirteen = EntryTestDrilldown.GetRollingExpectancy(TestData, 13);

             resultsLong = resultsLong.Select(TestUtils._round).ToList();
             thirteen = thirteen.Select(TestUtils._round).ToList();

             for (int i = 0; i < thirteen.Count; i++)
             {

                 Assert.Equal(resultsLong[i], thirteen[i]);
             }

            Assert.Equal(resultsLong.Select(TestUtils._round), thirteen.Select(TestUtils._round));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverSmallEpochSplit()        {
            var resultsThreePeriod = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt");
            var ThreeEpoch = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(ThreeEpoch.Select(TestUtils._round), resultsThreePeriod.Select(TestUtils._round));
        }

        [Fact]
        private void ShouldGenerateExpectancyOverLargerEpochSplit()        {
            var resultsTenPeriod = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt");
            var tenEpoch = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            Assert.Equal(resultsTenPeriod.Select(TestUtils._round), tenEpoch.Select(TestUtils._round));
        }

        [Fact]
        private void GeneratesBoundedStats()        {
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
        private void ShouldGenerateEpoch()
        {
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
        private void ShouldGenerateEvenEpoch()
        {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};
            var epochTwo = EpochGenerator.SplitListIntoEpochs(myListTwo, 4);
            Assert.Equal(4, epochTwo.EpochContainer.Count);
            Assert.Equal(new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4}, epochTwo.EpochContainer[0]);
            Assert.Equal(new List<double>() {12, 3, 0.4, -89, -0.5, 56, 12}, epochTwo.EpochContainer[1]);
            Assert.Equal(new List<double>() {3, 0.4, 7, 0.1, -0.1, 12, 3}, epochTwo.EpochContainer[2]);
            Assert.Equal(new List<double>() {0.4, 12, 3, 0.4, 12, 3, 0.4}, epochTwo.EpochContainer[3]);
        }

        [Fact]
        private void ShouldGenerateUnequalEpochs()
        {
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
        private void ShouldGenerateMassiveEpoch()
        {
            List<double> myListThree = new List<double>();
            for (int i = 0; i < 12345; i++) myListThree.Add(0);
            var epochFour = EpochGenerator.SplitListIntoEpochs(myListThree, 29);
            Assert.Equal(myListThree.Count % (29 - 1), epochFour.EpochContainer[0].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[1].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[21].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[3].Count);
            Assert.Equal(12345, epochFour.EpochContainer.Sum(x => x.Count));
        }
    }
}
