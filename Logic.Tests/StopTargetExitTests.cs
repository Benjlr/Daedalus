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
                new DummyEntries(2, FSTETestsBars.DataLong.Length)
            }, market);

            var longSide = new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1).Run(strat, market, MarketSide.Bull);
            var shortSide = new TestFactory.FixedStopTargetExitTestOptions(0.15, 0.15, 0.15, 1).Run(strat, market, MarketSide.Bear);

            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Length; i++)
                myTests.Add(new[]
                {
                    longSide[i], 
                    shortSide[i]
                });
        }

        [Fact]
        public void ShouldGenerateLongResults() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Result, myTests[0][0].Trades[i].Result);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Results, myTests[0][0].Trades[i].Results);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Win, myTests[0][0].Trades[i].Win);
            }

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Result, myTests[3][0].Trades[i].Result);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Results, myTests[3][0].Trades[i].Results);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Win, myTests[3][0].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Result, myTests[0][1].Trades[i].Result);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Results, myTests[0][1].Trades[i].Results);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Win, myTests[0][1].Trades[i].Win);
            }

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Result, myTests[3][1].Trades[i].Result);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Results, myTests[3][1].Trades[i].Results);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Win, myTests[3][1].Trades[i].Win);
            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) 
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Drawdown, myTests[0][0].Trades[i].Drawdown);

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) 
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Drawdown, myTests[3][0].Trades[i].Drawdown);
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++)
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Drawdown, myTests[0][1].Trades[i].Drawdown);

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++)
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Drawdown, myTests[3][1].Trades[i].Drawdown);
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            for (int i = 0; i < FSTETestsBars._longSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].MarketEnd, myTests[0][0].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].MarketStart, myTests[0][0].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._longSmallStopTarget[i].Duration, myTests[0][0].Trades[i].Duration);
            }

            for (int i = 0; i < FSTETestsBars._longLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].MarketEnd, myTests[3][0].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].MarketStart, myTests[3][0].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._longLargerStopTarget[i].Duration, myTests[3][0].Trades[i].Duration);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            for (int i = 0; i < FSTETestsBars._shortSmallStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].MarketEnd, myTests[0][1].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].MarketStart, myTests[0][1].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._shortSmallStopTarget[i].Duration, myTests[0][1].Trades[i].Duration);
            }

            for (int i = 0; i < FSTETestsBars._shortLargerStopTarget.Count; i++) {
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].MarketEnd, myTests[3][1].Trades[i].MarketEnd);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].MarketStart, myTests[3][1].Trades[i].MarketStart);
                Assert.Equal(FSTETestsBars._shortLargerStopTarget[i].Duration, myTests[3][1].Trades[i].Duration);
            }
        }
    }
}
