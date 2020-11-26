using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStructures;
using Logic.Metrics;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {

        private List<ITest[]> myTests { get; set; }

        private List<Trade> longTradesOne = new List<Trade>()
        {
            new Trade(new[] {(10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (12.5 / 9.0) - 1}, 1),
            new Trade(new[] {(10.5 / 13.0) - 1, (11.5 / 13.0) - 1, (6.5 / 13) - 1}, 3),
            new Trade(new[] {(8.5 / 7.0) - 1, (3.5 / 7) - 1, (8.5 / 7.0) - 1}, 5),
            new Trade(new[] {(9.5 / 9.0) - 1, (10.5 / 9.0) - 1, (13.5 / 9.0) - 1}, 7),
            new Trade(new[] {(14.5 / 14.0) - 1}, 9),
        };

        private List<Trade> longTradesTwo = new List<Trade>()
        {
            new Trade(new[] {(10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (11.5 / 9.0) - 1, (6.5 / 9.0) - 1}, 1),
            new Trade(new[] {(10.5 / 13.0) - 1, (11.5 / 13.0) - 1, (8.5 / 13.0) - 1, (3.5 / 13.0) - 1, (8.5 / 13.0) - 1}, 3),
            new Trade(new[] {(8.5 / 7.0) - 1, (3.5 / 7.0) - 1, (9.5 / 7.0) - 1, (10.5 / 7.0) - 1, (13.5 / 7.0) - 1}, 5),
            new Trade(new[] {(9.5 / 9.0) - 1, (10.5 / 9.0) - 1, (14.5 / 9) - 1}, 7),
            new Trade(new[] {(14.5 / 14.0) - 1}, 9),
        };

        private List<Trade> shortTradesOne = new List<Trade>()
        {
            new Trade(new[] {1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (13 / 8.5) }, 1),
            new Trade(new[] {1 - (11 / 12.5), 1 - (12 / 12.5), 1 - (7.0 / 12.5)}, 3),
            new Trade(new[] {1 - (9 / 6.5), 1 - (4.0 / 6.5), 1 - (9 / 6.5) }, 5),
            new Trade(new[] {1 - (10 / 8.5), 1 - (11 / 8.5), 1 - (14 / 8.5) }, 7),
            new Trade(new[] {1 - (15 / 13.5)}, 9),
        };

        private List<Trade> shortTradesTwo = new List<Trade>()
        {
            new Trade(new[] {1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (12 / 8.5), 1 - (7 / 8.5) }, 1),
            new Trade(new[] {1 - (11 / 12.5), 1 - (12 / 12.5), 1 - (9 / 12.5), 1 - (4 / 12.5), 1 - (9 / 12.5) }, 3),
            new Trade(new[] {1 - (9 / 6.5), 1 - (4 / 6.5), 1 - (10 / 6.5), 1 - (11 / 6.5), 1 - (14 / 6.5)}, 5),
            new Trade(new[] {1 - (10 / 8.5), 1 - (11 / 8.5), 1 - (15.0 / 8.5) }, 7),
            new Trade(new[] {1 - (15 / 13.5)}, 9),
        };

        public FixedBarExitTests() {
            var market = Market.MarketBuilder.CreateMarket(FBETestBars.DataLong);
            var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(2, FBETestBars.DataLong.Length)
            }, market);

            PrepareTests(strat, market);
        }

        private void PrepareTests(Strategy strat, Market market) {
            var longSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(2, 4, 2, MarketSide.Bull));
            var shortSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(2, 4, 2, MarketSide.Bear));
            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Count; i++)
                myTests.Add(new[] {longSide[i], shortSide[i]});
        }


        [Fact]
        public void ShouldGenerateLongResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(longTradesOne[i].Result, myTests[0][0].Trades[i].Result);
                Assert.Equal(longTradesOne[i].Results, myTests[0][0].Trades[i].Results);
                Assert.Equal(longTradesOne[i].Win, myTests[0][0].Trades[i].Win);

                Assert.Equal(longTradesTwo[i].Result, myTests[1][0].Trades[i].Result);
                Assert.Equal(longTradesTwo[i].Results, myTests[1][0].Trades[i].Results);
                Assert.Equal(longTradesTwo[i].Win, myTests[1][0].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(shortTradesOne[i].Result, myTests[0][1].Trades[i].Result);
                Assert.Equal(shortTradesOne[i].Results, myTests[0][1].Trades[i].Results);
                Assert.Equal(shortTradesOne[i].Win, myTests[0][1].Trades[i].Win);

                Assert.Equal(shortTradesTwo[i].Result, myTests[1][1].Trades[i].Result);
                Assert.Equal(shortTradesTwo[i].Results, myTests[1][1].Trades[i].Results);
                Assert.Equal(shortTradesTwo[i].Win, myTests[1][1].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(longTradesOne[i].Drawdown, myTests[0][0].Trades[i].Drawdown);
                Assert.Equal(longTradesTwo[i].Drawdown, myTests[1][0].Trades[i].Drawdown);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(shortTradesOne[i].Drawdown, myTests[0][1].Trades[i].Drawdown);
                Assert.Equal(shortTradesTwo[i].Drawdown, myTests[1][1].Trades[i].Drawdown);
            }
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(longTradesOne[i].MarketEnd, myTests[0][0].Trades[i].MarketEnd);
                Assert.Equal(longTradesOne[i].MarketStart, myTests[0][0].Trades[i].MarketStart);
                Assert.Equal(longTradesOne[i].Duration, myTests[0][0].Trades[i].Duration);

                Assert.Equal(longTradesTwo[i].MarketEnd, myTests[1][0].Trades[i].MarketEnd);
                Assert.Equal(longTradesTwo[i].MarketStart, myTests[1][0].Trades[i].MarketStart);
                Assert.Equal(longTradesTwo[i].Duration, myTests[1][0].Trades[i].Duration);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(shortTradesOne[i].MarketEnd, myTests[0][1].Trades[i].MarketEnd);
                Assert.Equal(shortTradesOne[i].MarketStart, myTests[0][1].Trades[i].MarketStart);
                Assert.Equal(shortTradesOne[i].Duration, myTests[0][1].Trades[i].Duration);

                Assert.Equal(shortTradesTwo[i].MarketEnd, myTests[1][1].Trades[i].MarketEnd);
                Assert.Equal(shortTradesTwo[i].MarketStart, myTests[1][1].Trades[i].MarketStart);
                Assert.Equal(shortTradesTwo[i].Duration, myTests[1][1].Trades[i].Duration);
            }
        }
    }
}