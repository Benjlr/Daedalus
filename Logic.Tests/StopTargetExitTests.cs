using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class StopTargetExitTests
    {
        private List<ITest[]> myTests { get; set; }

        public StopTargetExitTests()
        {
            var market = Market.MarketBuilder.CreateMarket(FSTETestsBars.DataLong);
            var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[] {
                new DummyEntries(1, 265)
            }, market);

            var longSide = TestFactory.GenerateFixedStopTargetExitTest
                (strat, market, 
                new FixedStopTargetExitTestOptions(0.1, 0.1, 0.1, 2, MarketSide.Bull));
            var shortSide = TestFactory.GenerateFixedStopTargetExitTest
                (strat, market, 
                new FixedStopTargetExitTestOptions(0.1, 0.1, 0.1, 2, MarketSide.Bear));

            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Count; i++)
                myTests.Add(new[] { longSide[i], shortSide[i] });
        }

        [Fact]
        public void ShouldGenerateLongResults() {
            var myTrades = new List<Trade>();
            for (var i = 0; i < myTrades.Count; i++)
                Assert.Equal(myTests[i][0].Trades, myTrades);
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            var myTrades = new List<Trade>();
            for (var i = 0; i < myTrades.Count; i++)
                Assert.Equal(myTests[i][1].Trades, myTrades);
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            var myTrades = new List<Trade>();
            for (var i = 0; i < myTrades.Count; i++)
                Assert.Equal(myTests[i][0].Trades, myTrades);
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            var myTrades = new List<Trade>();
            for (var i = 0; i < myTrades.Count; i++)
                Assert.Equal(myTests[i][1].Trades, myTrades);
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            var resultsLong = Loaders.LoadData(Directory.GetCurrentDirectory() + "\\StopTarget\\DurationLong.txt", 4);
            for (var i = 0; i < resultsLong.Count; i++)
            for (int j = 0; j < resultsLong[i].Count; j++)
                Assert.Equal(myTests[i][0].Trades[j].Duration, resultsLong[i].ToArray()[j]);
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            var resultsLong = new List<int>();
            for (var i = 0; i < resultsLong.Count; i++)
                Assert.Equal(myTests[i][1].Trades[i].Duration, resultsLong[i]);
        }
    }
}
