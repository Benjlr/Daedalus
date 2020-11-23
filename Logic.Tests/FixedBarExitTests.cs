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
            new Trade(new []{0,(12 - 9) / 9.0,(13 - 9) / 9.0},1),
            new Trade(new []{0, (15 - 13) / 13.0, (7.0 - 13) / 13.0}, 3),
            new Trade(new []{0, (6 - 7) / 7.0, (9 - 7) / 7.0}, 5),
            new Trade(new []{0, (13 - 9) / 9.0, (14 - 9) / 9.0}, 7),
            new Trade(new []{0.0}, 9),
        };
        private List<Trade> longTradesTwo = new List<Trade>()
        {
            new Trade(new []{0,(12 - 9) / 9.0,(13 - 9) / 9.0, (15 - 9) / 9.0, (7 - 9) / 9.0},1),
            new Trade(new []{0, (15 - 13) / 13.0, (7.0 - 13) / 13.0, (6 - 13) / 13.0, (9 - 13) / 13.0}, 3),
            new Trade(new []{0, (6 - 7) / 7.0, (9 - 7) / 7.0, (13 - 7) / 7.0, (14 - 7) / 7.0}, 5),
            new Trade(new []{0, (13 - 9) / 9.0, (14 - 9) / 9.0}, 7),
            new Trade(new []{0.0}, 9),
        };

        private List<Trade> shortTradesOne = new List<Trade>()
        {
            new Trade(new []{0,(9 - 12) / 9.0, (9 - 13) / 9.0},1),
            new Trade(new []{0, (13 - 15) / 13.0, (13.0 - 7) / 13.0}, 3),
            new Trade(new []{0, (7 - 6) / 7.0, (7 - 9) / 7.0}, 5),
            new Trade(new []{0, (9 - 13) / 9.0,(9 - 14) / 9.0}, 7),
            new Trade(new []{0.0}, 9),
        };
        private List<Trade> shortTradesTwo = new List<Trade>()
        {
            new Trade(new []{0, (9 - 12) / 9.0, (9 - 13) / 9.0,(9 - 15) / 9.0,(9 - 7) / 9.0},1),
            new Trade(new []{0, (13 - 15) / 13.0, (13 - 7) / 13.0,(13 - 6) / 13.0,(13 - 9) / 13.0}, 3),
            new Trade(new []{0, (7 - 6) / 7.0, (7 - 9) / 7.0,(7 - 13) / 7.0,(7 - 14) / 7.0}, 5),
            new Trade(new []{0, (9 - 13) / 9.0,(9 - 14) / 9.0}, 7),
            new Trade(new []{0.0}, 9),
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