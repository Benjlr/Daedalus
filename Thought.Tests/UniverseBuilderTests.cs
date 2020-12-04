using DataStructures.StatsTools;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Linq;
using Xunit;

namespace Thought.Tests
{

    public class UniverseBuilderTestsFixture
    {
        public Universe _universe { get; private set; }
        public UniverseTest[] _fbeTests { get; private set; }
        public UniverseTest[] _fsteTests { get; private set; }
        public UniverseTest[] _randomExitests { get; private set; }
        private UniverseTestFactory _testFactory { get; set; }

        public UniverseBuilderTestsFixture() {
            BuildUniverse();
            PrepareAndRunTests();
        }

        private void BuildUniverse() {
            _universe = new Universe(new IRuleSet[2] { new DummyEntries(40, 1000), new DummyExits(31, 1000) });
            _universe.AddMarket(Markets.futures_wheat_5);
            _universe.AddMarket(Markets.aud_usd_5);
            _universe.AddMarket(Markets.ASX20());
        }

        private void PrepareAndRunTests() {
            _testFactory = new UniverseTestFactory();
            _fbeTests = _testFactory.RunTests(_universe, new TestFactory.FixedBarExitTestOptions(5, 40, 5));
            _fsteTests = _testFactory.RunTests(_universe, new TestFactory.FixedStopTargetExitTestOptions(0.05, 0.05, 0.1, 15));
            _randomExitests = _testFactory.RunTests(_universe, new TestFactory.RandomExitTestOptions(20, 15));
        }
    }

    public class UniverseBuilderTests : IClassFixture<UniverseBuilderTestsFixture>
    {
        private readonly UniverseBuilderTestsFixture _fixture;

        public UniverseBuilderTests(UniverseBuilderTestsFixture fixt) {
            _fixture = fixt;
        }

        [Fact]
        private void ShouldOpenMultipleMarkets() {
            Assert.Equal(22, _fixture._universe.Elements.Count);
        }

        [Fact]
        private void ShouldCalculateMultipleStrategyResults() {
            Assert.True(_fixture._universe.Elements.All(x => x.Strategy.Entries.Any(y => y)));
            Assert.True(_fixture._universe.Elements.All(x => x.Strategy.Exits.Any(y => y)));
        }

        [Fact]
        private void ShouldReturnElement() {
            Assert.True(true);
        }

        [Fact]
        private void ShouldRunTestsOnUniverse() {
            Assert.Equal(22, _fixture._fbeTests.Length);
            Assert.Equal(22, _fixture._randomExitests.Length);
            Assert.Equal(22, _fixture._fsteTests.Length);
        }

        [Fact]
        private void ShouldGenerateLongFixedBarTests() {
            foreach (var t1 in _fixture._fbeTests)
            foreach (var t in t1.LongTests)
                AssertTrades(t);
        }

        [Fact]
        private void ShouldGenerateShortFixedTests() {
            foreach (var t1 in _fixture._fbeTests)
            foreach (var t in t1.ShortTests)
                AssertTrades(t);
        }

        [Fact]
        private void ShouldGenerateLongFixedStopTargetBarTests() {
            foreach (var t1 in _fixture._fsteTests)
            foreach (var t in t1.LongTests)
                AssertTrades(t);
        }

        [Fact]
        private void ShouldGenerateShortFixedStopTargetTests() {
            foreach (var t1 in _fixture._fsteTests)
            foreach (var t in t1.ShortTests)
                AssertTrades(t);
        }

        [Fact]
        private void ShouldGenerateLongRandomExitTests() {
            foreach (var t1 in _fixture._randomExitests)
            foreach (var t in t1.LongTests)
                AssertTrades(t);
        }

        [Fact]
        private void ShouldGenerateShortRandomExitTests() {
            foreach (var t1 in _fixture._randomExitests)
            foreach (var t in t1.ShortTests) 
                AssertTrades(t);
        }

        private void AssertTrades(ITest t) {
            AssertAvgExp(t);
            AssertAvgMed(t);
            AssertResults(t);
        }

        private void AssertResults(ITest t) {
            Assert.True(t.Trades.Count > 0);
            Assert.True(!t.Trades.All(x => x.Result.Equals(0)));
            Assert.True(!double.IsNaN(t.Stats.WinPercent));
        }

        private void AssertAvgMed(ITest t) {
            var medExp = t.Stats.MedianExpectancy != 0 && !double.IsNaN(t.Stats.MedianExpectancy);
            var medGainLoss = !double.IsNaN(t.Stats.MedianExpectancy) && (t.Stats.MedianGain > 0 & t.Stats.MedianLoss < 0);
            Assert.True(medExp || medGainLoss);
        }

        private void AssertAvgExp(ITest t) {
            var avgExp = t.Stats.AverageExpectancy != 0 && !double.IsNaN(t.Stats.AverageExpectancy);
            var avgGainLoss = !double.IsNaN(t.Stats.AverageExpectancy) && (t.Stats.AvgGain > 0 & t.Stats.AvgLoss < 0);
            Assert.True(avgExp || avgGainLoss);
        }
    }
}
