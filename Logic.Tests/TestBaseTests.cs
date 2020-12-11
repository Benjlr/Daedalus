using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Logic.Tests
{

    public class TestBaseFixture
    {
        public List<ITest[]> myTests { get; private set; }

        public TestBaseFixture() {
            var market = Market.MarketBuilder.CreateMarket(FBETestBars.DataLong);
            var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[] {
                new DummyEntries(2, FBETestBars.DataLong.Length)
            }, market);

            PrepareTests(strat, market);
        }

        private void PrepareTests(Strategy strat, Market market) {
            var longSide = new TestFactory.FixedBarExitTestOptions(2, 4, 2).Run(strat, market, MarketSide.Bull);
            var shortSide = new TestFactory.FixedBarExitTestOptions(2, 4, 2).Run(strat, market, MarketSide.Bear);
            collate(longSide, shortSide);
        }

        private void collate(ITest[] longSide, ITest[] shortSide) {
            myTests = new List<ITest[]>();
            for (int i = 0; i < longSide.Length; i++)
                myTests.Add(new[] {longSide[i], shortSide[i]});
        }
    }
    public class TestBaseTests : IClassFixture<TestBaseFixture>
    {
        private readonly TestBaseFixture _fixt;
        public TestBaseTests(TestBaseFixture fixture) {
            _fixt = fixture;
        }



        [Fact]
        public void ShouldGenerateCorrectLongAverages() {
            var avgGainsLong = new List<double>() { 0.2847222222222222, 0.5251322751322752 };
            var avgLossLong = new List<double>() { -0.5, -0.31196581196581197 };
            var avgddLong = new List<double>() { -0.5, -0.5028490028490028 };
            var avgddLongWinners = new List<double>() { -0.125, -0.16666666666666666 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][0].Stats.AvgGain, avgGainsLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.AvgLoss, avgLossLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.AverageDrawdown, avgddLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.AverageDrawdownWinners, avgddLongWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectLongMedians() {
            var medianGainsLong = new List<double>() { 0.3015873015873015, 0.6111111111111112 };
            var medianLossLong = new List<double>() { -0.5, -0.31196581196581197 };
            var medianddLong = new List<double>() { -0.5, -0.5 };
            var medianddLongWinners = new List<double>() { 0, 0 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][0].Stats.MedianGain, medianGainsLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.MedianLoss, medianLossLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.MedianDrawDown, medianddLong[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.MedianDrawDownWinners, medianddLongWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortAverages() {
            var avgGainsShort = new List<double>() { 0.43999999999999995, 0.2282352941176471 };
            var avgLossShort = new List<double>() { -0.4180492709904474, -0.6765543824367354 };
            var avgddShort = new List<double>() { -0.4180492709904474, -0.6103569632981397 };
            var avgddShortWinners = new List<double>() { 0, -0.20588235294117652 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][1].Stats.AvgGain, avgGainsShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.AvgLoss, avgLossShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.AverageDrawdown, avgddShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.AverageDrawdownWinners, avgddShortWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortMedians() {
            var medianGainsShort = new List<double>() { 0.43999999999999995, 0.2282352941176471 };
            var medianLossShort = new List<double>() { -0.4570135746606334, -0.7647058823529411 };
            var medianddShort = new List<double>() { -0.4570135746606334, -0.5882352941176471 };
            var medianddShortWinners = new List<double>() { 0, -0.20588235294117652 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][1].Stats.MedianGain, medianGainsShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.MedianLoss, medianLossShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.MedianDrawDown, medianddShort[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.MedianDrawDownWinners, medianddShortWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateWinRatios() {
            var longRatios = new List<double>() { 0.8, 0.6 };
            var shortRatios = new List<double>() { 0.2, 0.4 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][0].Stats.WinPercent, longRatios[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.WinPercent, shortRatios[i]);
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyLong() {
            var avgExp = new List<double>() { 0.1277777777777778, 0.19029304029304034 };
            var medianExp = new List<double>() { 0.14126984126984124, 0.2418803418803419 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][0].Stats.AverageExpectancy, avgExp[i]);
                Assert.Equal(_fixt.myTests[i][0].Stats.MedianExpectancy, medianExp[i]);
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyShort() {
            var avgExp = new List<double>() { -0.24643941679235795, -0.31463851181498237 };
            var medianExp = new List<double>() { -0.27761085972850674, -0.36752941176470577 };
            for (var i = 0; i < _fixt.myTests.Count; i++) {
                Assert.Equal(_fixt.myTests[i][1].Stats.AverageExpectancy, avgExp[i]);
                Assert.Equal(_fixt.myTests[i][1].Stats.MedianExpectancy, medianExp[i]);
            }
        }
    }
}
