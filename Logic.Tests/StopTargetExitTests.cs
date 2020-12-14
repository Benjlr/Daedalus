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
    public class StopTargetExitTestsFixture
    {
        private Market _market;
        private Strategy _strat;
        public List<ITest[]> myTests { get; private set; }

        public StopTargetExitTestsFixture() {
            BuildMarket();
            PrepareTests();
        }

        private void PrepareTests() {
            var longSide = new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1).Run(_strat, _market, MarketSide.Bull);
            var shortSide = new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1).Run(_strat, _market, MarketSide.Bear);
            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Length; i++)
                myTests.Add(new[] {longSide[i], shortSide[i]});
        }

        private void BuildMarket() {
            _market = Market.MarketBuilder.CreateMarket(FSTETestsBars.DataLong);
            _strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(2, FSTETestsBars.DataLong.Length)
            }, _market);
        }
    }

    public class StopTargetExitTests : IClassFixture<StopTargetExitTestsFixture>
    {
        private StopTargetExitTestsFixture _fixture;

        public StopTargetExitTests(StopTargetExitTestsFixture fixt) {
            _fixture = fixt;
        }

        [Fact]
        public void ShouldGenerateLongResults() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].FinalResult, _fixture.myTests[0][0].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FSTETestsBars._longSmallStopTarget[i].Results, _fixture.myTests[0][0].Trades[i].Results);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Win, _fixture.myTests[0][0].Trades[i].Win);
            }

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].FinalResult, _fixture.myTests[3][0].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FSTETestsBars._longLargerStopTarget[i].Results, _fixture.myTests[3][0].Trades[i].Results);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Win, _fixture.myTests[3][0].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].FinalResult, _fixture.myTests[0][1].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FSTETestsBars._shortSmallStopTarget[i].Results, _fixture.myTests[0][1].Trades[i].Results);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Win, _fixture.myTests[0][1].Trades[i].Win);
            }

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].FinalResult, _fixture.myTests[3][1].Trades[i].FinalResult);
                Asserters.ArrayDoublesEqual(FSTETestsBars._shortLargerStopTarget[i].Results, _fixture.myTests[3][1].Trades[i].Results);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Win, _fixture.myTests[3][1].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) 
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].FinalDrawdown, _fixture.myTests[0][0].Trades[i].FinalDrawdown);

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) 
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].FinalDrawdown, _fixture.myTests[3][0].Trades[i].FinalDrawdown);
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++)
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].FinalDrawdown, _fixture.myTests[0][1].Trades[i].FinalDrawdown);

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++)
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].FinalDrawdown, _fixture.myTests[3][1].Trades[i].FinalDrawdown);
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].MarketEnd, _fixture.myTests[0][0].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].MarketStart, _fixture.myTests[0][0].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Duration, _fixture.myTests[0][0].Trades[i].Duration);
            }

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].MarketEnd, _fixture.myTests[3][0].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].MarketStart, _fixture.myTests[3][0].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Duration, _fixture.myTests[3][0].Trades[i].Duration);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].MarketEnd, _fixture.myTests[0][1].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].MarketStart, _fixture.myTests[0][1].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Duration, _fixture.myTests[0][1].Trades[i].Duration);
            }

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].MarketEnd, _fixture.myTests[3][1].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].MarketStart, _fixture.myTests[3][1].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Duration, _fixture.myTests[3][1].Trades[i].Duration);
            }
        }
    }
}
