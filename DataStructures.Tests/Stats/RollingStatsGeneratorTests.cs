using System.IO;
using System.Linq;
using DataStructures.StatsTools;
using TestUtils;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class RollingStatsGeneratorTests
    {
        //private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


        public RollingStatsGeneratorTests()
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

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodAvg() {
            var resultsLong = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpResults.txt",2);
            var TestData = Loaders.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpData.txt");
            var ten = RollingStatsGenerator.GetRollingStats(TestData, 10);
            Assert.Equal(resultsLong[0], ten.Select(x => x.AverageExpectancy));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodMed() {
            var resultsLong = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpResults.txt", 2);
            var TestData = Loaders.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10RollingExpData.txt");
            var ten = RollingStatsGenerator.GetRollingStats(TestData, 10);
            Assert.Equal(resultsLong[1], ten.Select(x => x.MedianExpectancy));
        }


        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodAvg() {
            var resultsLong = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpResults.txt",2);
            var TestData = Loaders.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpData.txt"); 
             var thirteen = RollingStatsGenerator.GetRollingStats(TestData, 13);
             Assert.Equal(resultsLong[0], thirteen.Select(x => x.AverageExpectancy));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodMed() {
            var resultsLong = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpResults.txt",2);
            var TestData = Loaders.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\13RollingExpData.txt");
            var thirteen = RollingStatsGenerator.GetRollingStats(TestData, 13);
            Assert.Equal(resultsLong[1], thirteen.Select(x => x.MedianExpectancy));
        }

        [Fact]
        private void ShouldGenerateStatsOverSmallEpochSplitAvg() {
            var resultsThreePeriod = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt",2);
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(ThreeEpoch.Select(x => x.AverageExpectancy), resultsThreePeriod[0]);
        }

        [Fact]
        private void ShouldGenerateStatsOverSmallEpochSplitMed() {
            var resultsThreePeriod = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt",2);
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(ThreeEpoch.Select(x => x.MedianExpectancy), resultsThreePeriod[1]);
        }

        [Fact]
        private void ShouldGenerateStatsOverLargerEpochSplitAvg() {
            var resultsTenPeriod = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt",2);
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            Assert.Equal(resultsTenPeriod[0], tenEpoch.Select(x=>x.AverageExpectancy));
        }

        [Fact]
        private void ShouldGenerateStatsOverLargerEpochSplitMed() {
            var resultsTenPeriod = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt",2);
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            Assert.Equal(resultsTenPeriod[1], tenEpoch.Select(x => x.MedianExpectancy));
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


