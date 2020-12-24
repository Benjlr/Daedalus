using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTestsFixture
    {
        private Market _market;
        private StaticStrategy _strat;
        public List<ITest[]> myTests { get; private set; }

        public FixedBarExitTestsFixture() {
            BuildMarket();
            PrepareTests();
        }

        private void BuildMarket() {
            _market = new Market(FBETestBars.DataLong, "testId");
            _strat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(2, FBETestBars.DataLong.Length)
            }, _market, new StaticStopTarget(ExitPrices.NoStopTarget()));
        }

        private void PrepareTests() {
            var longSide = new TestFactory.FixedBarExitTestOptions(2, 4, 2).Run(_strat, _market, MarketSide.Bull);
            var shortSide = new TestFactory.FixedBarExitTestOptions(2, 4, 2).Run(_strat, _market, MarketSide.Bear);
            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Length; i++)
                myTests.Add(new[] { longSide[i], shortSide[i] });
        }

    }

    public class FixedBarExitTests : IClassFixture<FixedBarExitTestsFixture>
    {
        private readonly FixedBarExitTestsFixture _fixture;
        public FixedBarExitTests(FixedBarExitTestsFixture fixt) {
            _fixture = fixt;
        }


        [Fact]
        public void ShouldGenerateLongResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.longTradesOne[i].FinalResult, _fixture.myTests[0][0].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FBETestBars.longTradesOne[i].ResultTimeline, _fixture.myTests[0][0].Trades[i].ResultTimeline);
                Assert.Equal(FBETestBars.longTradesOne[i].Win, _fixture.myTests[0][0].Trades[i].Win);

                Assert.Equal(FBETestBars.longTradesTwo[i].FinalResult, _fixture.myTests[1][0].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FBETestBars.longTradesTwo[i].ResultTimeline, _fixture.myTests[1][0].Trades[i].ResultTimeline);
                Assert.Equal(FBETestBars.longTradesTwo[i].Win, _fixture.myTests[1][0].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].FinalResult, _fixture.myTests[0][1].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FBETestBars.shortTradesOne[i].ResultTimeline, _fixture.myTests[0][1].Trades[i].ResultTimeline);
                Assert.Equal(FBETestBars.shortTradesOne[i].Win, _fixture.myTests[0][1].Trades[i].Win);

                Assert.Equal(FBETestBars.shortTradesTwo[i].FinalResult, _fixture.myTests[1][1].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FBETestBars.shortTradesTwo[i].ResultTimeline, _fixture.myTests[1][1].Trades[i].ResultTimeline);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Win, _fixture.myTests[1][1].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.longTradesOne[i].FinalDrawdown, _fixture.myTests[0][0].Trades[i].FinalDrawdown);
                Assert.Equal(FBETestBars.longTradesTwo[i].FinalDrawdown, _fixture.myTests[1][0].Trades[i].FinalDrawdown);
            }               
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].FinalDrawdown, _fixture.myTests[0][1].Trades[i].FinalDrawdown);
                Assert.Equal(FBETestBars.shortTradesTwo[i].FinalDrawdown, _fixture.myTests[1][1].Trades[i].FinalDrawdown);
            }               
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.longTradesOne[i].MarketEnd, _fixture.myTests[0][0].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.longTradesOne[i].MarketStart, _fixture.myTests[0][0].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.longTradesOne[i].Duration, _fixture.myTests[0][0].Trades[i].Duration);

                Assert.Equal(FBETestBars.longTradesTwo[i].MarketEnd, _fixture.myTests[1][0].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.longTradesTwo[i].MarketStart, _fixture.myTests[1][0].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.longTradesTwo[i].Duration, _fixture.myTests[1][0].Trades[i].Duration);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            for (int i = 0; i < 5; i++) {
                Assert.Equal(FBETestBars.shortTradesOne[i].MarketEnd, _fixture.myTests[0][1].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.shortTradesOne[i].MarketStart, _fixture.myTests[0][1].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.shortTradesOne[i].Duration, _fixture.myTests[0][1].Trades[i].Duration);

                Assert.Equal(FBETestBars.shortTradesTwo[i].MarketEnd, _fixture.myTests[1][1].Trades[i].MarketEnd);
                Assert.Equal(FBETestBars.shortTradesTwo[i].MarketStart, _fixture.myTests[1][1].Trades[i].MarketStart);
                Assert.Equal(FBETestBars.shortTradesTwo[i].Duration, _fixture.myTests[1][1].Trades[i].Duration);
            }
        }
    }
}