using DataStructures;
using DataStructures.StatsTools;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Linq;
using Xunit;

namespace Thought.Tests
{
    public class UniverseBuilderTests
    {
        private readonly Universe _universe;

        public UniverseBuilderTests() {
            _universe = new Universe(new IRuleSet[2] {new DummyEntries(2, 40), new DummyExits(3, 40)});
            _universe.AddMarket(Markets.futures_wheat_5);
            _universe.AddMarket(Markets.aud_usd_5);
            _universe.AddMarket(Markets.ASX20());
            TestTheUniverse(_universe);
        }

        [Fact]
        private void ShouldOpenMultipleMarkets() {
            Assert.Equal(22, _universe.Elements.Count);
        }

        [Fact]
        private void ShouldCalculateMultipleStrategyResults() {
            Assert.True(_universe.Elements.All(x => x.Strategy.Entries.Any(y => y)));
            Assert.True(_universe.Elements.All(x => x.Strategy.Exits.Any(y => y)));
        }

        [Fact]
        private void ShouldRunTestsOnUniverse() {
            Assert.Equal(22, FBETests.Length);
            Assert.Equal(22, RandomExitests.Length);
            Assert.Equal(22, FSTETests.Length);
        }

        [Fact]
        private void ShouldGenerateLongFixedBarTests() {
            foreach (var t1 in FBETests)
            foreach (var t in t1.LongTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        [Fact]
        private void ShouldGenerateShortFixedTests() {
            foreach (var t1 in FBETests)
            foreach (var t in t1.ShortTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        [Fact]
        private void ShouldGenerateLongFixedStopTargetBarTests() {
            foreach (var t1 in FSTETests)
            foreach (var t in t1.LongTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        [Fact]
        private void ShouldGenerateShortFixedStopTargetTests() {
            foreach (var t1 in FSTETests)
            foreach (var t in t1.ShortTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        [Fact]
        private void ShouldGenerateLongRandomExitTests() {
            foreach (var t1 in RandomExitests)
            foreach (var t in t1.LongTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        [Fact]
        private void ShouldGenerateShortRandomExitTests() {
            foreach (var t1 in RandomExitests)
            foreach (var t in t1.ShortTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
                Assert.True(!double.IsNaN(t.Stats.WinPercent));
            }
        }

        private UniverseTest[] FBETests;
        private UniverseTest[] FSTETests;
        private UniverseTest[] RandomExitests;

        public void TestTheUniverse(Universe myUniverse) {
            RunFixedBarTests(myUniverse);
        }

        private void RunFixedBarTests(Universe myUniverse) {
            FBETests = IterateTests(myUniverse, new TestFactory.FixedBarExitTestOptions(2, 6, 2));
            FSTETests = IterateTests(myUniverse, new TestFactory.FixedStopTargetExitTestOptions(0.04, 0.1, 0.05, 1));
            RandomExitests = IterateTests(myUniverse, new TestFactory.RandomExitTestOptions());

        }

        private UniverseTest[] IterateTests(Universe myUniverse, TestOption options) {
            var results = new UniverseTest[myUniverse.Elements.Count];
            for (int i = 0; i < myUniverse.Elements.Count; i++)
                results[i] = new UniverseTest(myUniverse.Elements[i], options);
            return results;
        }

        public readonly struct UniverseTest
        {
            public string Name { get; }
            public ITest[] LongTests { get; }
            public ITest[] ShortTests { get; }

            public UniverseTest(UniverseObject universeIter, TestOption option) {
                Name = universeIter.Name;
                LongTests = option.Run(universeIter.Strategy, universeIter.MarketData, MarketSide.Bull);
                ShortTests = option.Run(universeIter.Strategy, universeIter.MarketData, MarketSide.Bear);
            }
        }

    }
}
