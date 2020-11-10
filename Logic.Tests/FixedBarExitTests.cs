using Logic.Metrics;
using Logic.Rules.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Logic.Analysis.Metrics;
using Logic.Strategies;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {

        private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


        public FixedBarExitTests()
        {
            var market = MarketBuilder.CreateMarket(marketData);
            var strat = StrategyBuilder.CreateStrategy(new Rules.IRuleSet[]
            {
                new DummyEntries(1, 98)
            }, market);

            myTests = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1));
        }



        [Fact]
        public void ShouldGenerateLongResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\LongData.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEResults.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\ShortData.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEResults.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownData.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShort.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEDrawdown.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownLongWinnersResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownDataWinners.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].FBEDrawdownWinners.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateDrawDownShortWinnersResults()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DrawdownShortWinners.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].FBEDrawdownWinners.Select(TestUtils._round), resultsLong[i].Select(TestUtils._round));
        }

        [Fact]
        public void ShouldGenerateLongDurations()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DurationLong.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][0].Durations, resultsLong[i]);
        }

        [Fact]
        public void ShouldGenerateShortDurations()
        {
            var resultsLong = TestUtils.LoadData(Directory.GetCurrentDirectory() + "\\FBEData\\DurationShort.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].Durations, resultsLong[i]);
        }
    }
}