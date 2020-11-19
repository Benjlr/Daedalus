using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStructures;
using Logic.Metrics;
using Xunit;

namespace Logic.Tests
{
    public class FixedBarExitTests
    {

        private List<ITest[]> myTests { get; set; }
        //private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


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
            var arrayOne = new double[,]
            {
                {0, 0},
                {0, 0},
                {(12 - 9) / 9.0, (12 - 9) / 9.0},
                {(13 - 9) / 9.0, (13 - 9) / 9.0 + 0},
                {(15 - 13) / 13.0, (15 - 13) / 13.0 + (15 - 9) / 9.0},
                {(7.0 - 13) / 13.0, (7 - 9) / 9.0 + (7 - 13) / 13.0},
                {(6 - 7) / 7.0, (6 - 13) / 13.0 + (6 - 7) / 7.0},
                {(9 - 7) / 7.0, (9 - 13) / 13.0 + (9 - 7) / 7.0 + 0},
                {(13 - 9) / 9.0, (13 - 9) / 9.0 + (13 - 7) / 7.0},
                {(14 - 9) / 9.0, (14 - 7) / 7.0 + (14 - 9) / 9.0}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                //Assert.Equal(arrayOne[i, 0], myTests[0][0].FBEResults[i]);
                //Assert.Equal(arrayOne[i, 1], myTests[1][0].FBEResults[i]);
                Assert.False(true);

            }
        }

        [Fact]
        public void ShouldGenerateShortResults() {
            var arrayOne = new double[,]
            {
                {0, 0},
                {0, 0},
                {(9 - 12) / 9.0, (9 - 12) / 9.0},
                {(9 - 13) / 9.0, (9 - 13) / 9.0 + 0},
                {(13 - 15) / 13.0, (13 - 15) / 13.0 + (9 - 15) / 9.0},
                {(13 - 7) / 13.0, (9 - 7) / 9.0 + (13 - 7) / 13.0},
                {(7 - 6) / 7.0, (13 - 6) / 13.0 + (7 - 6) / 7.0},
                {(7 - 9) / 7.0, (13 - 9) / 13.0 + (7 - 9) / 7.0 + 0},
                {(9 - 13) / 9.0, (9 - 13) / 9.0 + (7 - 13) / 7.0},
                {(9 - 14) / 9.0, (7 - 14) / 7.0 + (9 - 14) / 9.0}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                //Assert.Equal(arrayOne[i, 0], myTests[0][1].FBEResults[i]);
                //Assert.Equal(arrayOne[i, 1], myTests[1][1].FBEResults[i]);
                Assert.False(true);

            }
        }

        [Fact]
        public void ShouldGenerateDrawDownLongResults() {
            var arrayOne = new double[,]
            {
                {0, 0},
                {(8 - 9) / 9.0, (8 - 9) / 9.0},
                {0, 0},
                {0, 0},
                {(9 - 13) / 13.0, (9 - 13) / 13.0},
                {(6 - 13) / 13.0 + (6 - 7) / 7.0, (6 - 9) / 9.0 + (6 - 13) / 13.0 + (6 - 7) / 7.0},
                {(4 - 7) / 7.0, (4 - 13) / 13.0 + (4 - 7) / 7.0},
                {(8 - 9) / 9.0, (8 - 13) / 13.0 + (8 - 9) / 9.0},
                {0, 0},
                {(13 - 14) / 14.0, (13 - 14) / 14.0}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                //Assert.Equal(arrayOne[i, 0], myTests[0][0].FBEDrawdown[i]);
                //Assert.Equal(arrayOne[i, 1], myTests[1][0].FBEDrawdown[i]);
                Assert.False(true);

            }
        }

        [Fact]
        public void ShouldGenerateDrawDownShortResults() {
            var arrayOne = new double[,]
            {
                {0, 0},
                {(9 - 11) / 9.0, (9 - 11) / 9.0},
                {(9 - 14) / 9.0, (9 - 14) / 9.0},
                {(9 - 14) / 9.0 + (13 - 14) / 13.0, (9 - 14) / 9.0 + (13 - 14) / 13.0},
                {(13 - 18) / 13.0, (13 - 18) / 13.0 + (9 - 18) / 9.0},
                {(7 - 9) / 7.0, (7 - 9) / 7.0},
                {0, 0},
                {(7 - 10) / 7.0 + (9 - 10) / 9.0, (7 - 10) / 7.0 + (9 - 10) / 9.0},
                {(9 - 14) / 9.0, (9 - 14) / 9.0 + (7 - 14) / 7.0},
                {(9 - 16) / 9.0 + (14 - 16) / 14.0, (7 - 16) / 7.0 + (9 - 16) / 9.0 + (14 - 16) / 14.0}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                //Assert.Equal(arrayOne[i, 0], myTests[0][1].FBEDrawdown[i]);
                //Assert.Equal(arrayOne[i, 1], myTests[1][1].FBEDrawdown[i]);
                Assert.False(true);
            }
        }

        [Fact]
        public void ShouldGenerateLongDurations() {
            var arrayOne = new double[,]
            {
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                Assert.Equal(arrayOne[i, 0], myTests[0][1].Trades[i].Results.Length );
                Assert.Equal(arrayOne[i, 1], myTests[1][1].Trades[i].Results.Length);
            }
        }

        [Fact]
        public void ShouldGenerateShortDurations() {
            var arrayOne = new double[,]
            {
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4},
                {0, 0},
                {2, 4}
            };

            for (int i = 0; i < arrayOne.GetLength(0); i++) {
                Assert.Equal(arrayOne[i, 0], myTests[0][1].Trades[i].Results.Length);
                Assert.Equal(arrayOne[i, 1], myTests[1][1].Trades[i].Results.Length);
            }
        }
    }
}