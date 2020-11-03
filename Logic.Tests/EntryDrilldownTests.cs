using System.Collections.Generic;
using Logic.Metrics.EntryTests.TestsDrillDown;
using System.IO;
using System.Linq;
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
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[] {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1));
        }




        [Fact]
        private void ShouldGenerateRollingExpectancy()
        {
            var resultsLong = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\Rolling8Exp.txt");
            var resultsLongSeventeen = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\Rolling18Exp.txt");
            var eight = EntryTestDrilldown.GetRollingExpectancy(myTests[0][0].FBEResults.ToList(), 8);
            var Seventeen = EntryTestDrilldown.GetRollingExpectancy(myTests[0][0].FBEResults.ToList(), 18);
            Assert.Equal(eight.Select(TestUtils._round).ToList(), resultsLong.Select(TestUtils._round).ToList());
            Assert.Equal(Seventeen.Select(TestUtils._round).ToList(), resultsLongSeventeen.Select(TestUtils._round).ToList());
        }

        [Fact]
        private void ShouldGenerateExpectancyByPeriod()
        {
            var resultsTenPeriod = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\10PeriodExp.txt");
            var resultsThirtyPeriod = TestUtils.LoadDataSingleColumn(Directory.GetCurrentDirectory() + "\\DrilldownData\\30PeriodExp.txt");
            var ten = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 10);
            var thirty = EntryTestDrilldown.GetExpectancyByEpoch(myTests[0][0].FBEResults.ToList(), 3);
            Assert.Equal(resultsTenPeriod.Select(TestUtils._round).ToList(), ten.Select(TestUtils._round).ToList());
            Assert.Equal(resultsThirtyPeriod.Select(TestUtils._round).ToList(), thirty.Select(TestUtils._round).ToList());
        }

        [Fact]
        private void GeneratesBoundedStats()
        {
            var myLIst = new List<double>();
            for (int i = 0; i <= 100; i++)myLIst.Add(i);
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(0, myStat.Minimum);
            Assert.Equal(100, myStat.Maximum);
            Assert.Equal(50, myStat.Average);
            Assert.Equal(50, myStat.Median);
            Assert.Equal(90, myStat.Upper);
            Assert.Equal(10, myStat.Lower);
        }
    }
}
