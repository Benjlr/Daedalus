using Logic.Metrics;
using Logic.Rules.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace Logic.Tests
{
    public class StopTargetExitTests
    {
        private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\StopTarget\\TestMarketData.txt";

        public StopTargetExitTests()
        {
            var market = MarketBuilder.CreateMarket(marketData);
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[] {
                new DummyEntries(1, 265)
            }, market);

            myTests = TestFactory.GenerateFixedStopTargetExitTest(strat, market, 
                new FixedStopTargetExitTestOptions(0.0015, 0.0015,0.0015,2));
        }

        [Fact]
        public void ShouldGenerateLongResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\longResults.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                    Assert.Equal(myTests[i][0].FBEResults.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\shortResults.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEResults.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\DrawdownLong.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\DrawdownShort.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongWinnersResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\DrawdownLongWinners.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEDrawdownWinners.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortWinnersResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\DrawdownShortWinners.txt", 2);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEDrawdownWinners.Select(TestUtils._round),
                    resultsLong[i].Select(TestUtils._round));
        }

        [Fact(Skip = "Return to this when TestData built")]
        private void ShouldGenerateLongRunHistory()
        {
            for (int i = 0; i < myTests.Count; i++)
                for (int j = 0; j < myTests[i][0].RunIndices.Count; j++) {
                    var myindt = new int[i + 1 + 10];
                    for (int k = 0; k < myindt.Length; k++) myindt[k] = 1 + j + k;
                    Assert.Equal(myTests[i][0].RunIndices[j], myindt);
                }
        }

        [Fact(Skip = "Return to this when TestData built")]
        private void ShouldGenerateShortRunHistory()
        {
            for (int i = 0; i < myTests.Count; i++)
                for (int j = 0; j < myTests[i][1].RunIndices.Count; j++) {
                    var myindt = new int[i + 1 + 10];
                    for (int k = 0; k < myindt.Length; k++) myindt[k] = 1 + j + k;
                    Assert.Equal(myTests[i][1].RunIndices[j], myindt);
                }
        }
    }
}