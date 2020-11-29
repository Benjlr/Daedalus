using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {

        private List<ITest[]> myTests { get; set; }

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
                Assert.Equal(FBETestBars.longTradesOne[i].Result, myTests[0][0].Trades[i].Result);
                Assert.Equal(FBETestBars.longTradesOne[i].Results, myTests[0][0].Trades[i].Results);
                Assert.Equal(FBETestBars.longTradesOne[i].Win, myTests[0][0].Trades[i].Win);

                Assert.Equal(FBETestBars.longTradesTwo[i].Result, myTests[1][0].Trades[i].Result);
                Assert.Equal(FBETestBars.longTradesTwo[i].Results, myTests[1][0].Trades[i].Results);
                Assert.Equal(FBETestBars.longTradesTwo[i].Win, myTests[1][0].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].Result, myTests[0][1].Trades[i].Result);
                Assert.Equal(FBETestBars.shortTradesOne[i].Results, myTests[0][1].Trades[i].Results);
                Assert.Equal(FBETestBars.shortTradesOne[i].Win, myTests[0][1].Trades[i].Win);

                Assert.Equal(FBETestBars.shortTradesTwo[i].Result, myTests[1][1].Trades[i].Result);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Results, myTests[1][1].Trades[i].Results);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Win, myTests[1][1].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.longTradesOne[i].Drawdown, myTests[0][0].Trades[i].Drawdown);
                Assert.Equal(FBETestBars.longTradesTwo[i].Drawdown, myTests[1][0].Trades[i].Drawdown);
            }               
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].Drawdown, myTests[0][1].Trades[i].Drawdown);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Drawdown, myTests[1][1].Trades[i].Drawdown);
            }               
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.longTradesOne[i].MarketEnd, myTests[0][0].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.longTradesOne[i].MarketStart, myTests[0][0].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.longTradesOne[i].Duration, myTests[0][0].Trades[i].Duration);

                Assert.Equal(FBETestBars.longTradesTwo[i].MarketEnd, myTests[1][0].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.longTradesTwo[i].MarketStart, myTests[1][0].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.longTradesTwo[i].Duration, myTests[1][0].Trades[i].Duration);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].MarketEnd, myTests[0][1].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.shortTradesOne[i].MarketStart, myTests[0][1].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.shortTradesOne[i].Duration, myTests[0][1].Trades[i].Duration);

                Assert.Equal(FBETestBars.shortTradesTwo[i].MarketEnd, myTests[1][1].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.shortTradesTwo[i].MarketStart, myTests[1][1].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Duration, myTests[1][1].Trades[i].Duration);
            }
        }
    }
}