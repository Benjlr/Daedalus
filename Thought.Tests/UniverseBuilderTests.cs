using System.Collections.Generic;
using DataStructures.StatsTools;
using RuleSets;
using RuleSets.Entry;
using System.Linq;
using DataStructures;
using Logic.Metrics;
using Xunit;

namespace Thought.Tests
{
    public class UniverseBuilderTests
    {
        private readonly Universe _universe;
        public UniverseBuilderTests() {
            _universe = new Universe(new IRuleSet[2] { new DummyEntries(2, 40), new DummyExits(3, 40) });
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
            Assert.Equal(22, Tests.Length);
        }

        [Fact]
        private void ShouldGenerateLongTests() {
            foreach (var t1 in Tests)
            foreach (var t in t1.LongTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
            }
        }

        [Fact]
        private void ShouldGenerateShortTests() {
            foreach (var t1 in Tests)
            foreach (var t in t1.ShortTests) {
                Assert.True(t.Stats.AverageExpectancy != 0);
                Assert.True(t.Stats.MedianExpectancy != 0);
            }
        }


        private UniverseTest[] Tests;
        public void TestTheUniverse(Universe myUniverse) {
            Tests = new UniverseTest[myUniverse.Elements.Count];
            var fbeOptionsBull = new FixedBarExitTestOptions(2,6,2, MarketSide.Bull);
            var fbeOptionsBear = new FixedBarExitTestOptions(2,6,2, MarketSide.Bear);
            for (int i = 0; i < myUniverse.Elements.Count; i++) {
                Tests[i] = new UniverseTest(myUniverse.Elements[i].Name,
                        TestFactory.GenerateFixedBarExitTest(
                            myUniverse.Elements[i].Strategy,
                            myUniverse.Elements[i].MarketData,
                            fbeOptionsBull).ToArray(),
                        TestFactory.GenerateFixedBarExitTest(
                            myUniverse.Elements[i].Strategy,
                            myUniverse.Elements[i].MarketData,
                            fbeOptionsBear).ToArray()
                    );
            }
        }

        public readonly struct UniverseTest
        {
            public string Name { get; }
            public ITest[] LongTests { get; }
            public ITest[] ShortTests { get; }

            public UniverseTest(string name, ITest[] longs, ITest[] shorts) {
                Name = name;
                LongTests = longs;
                ShortTests = shorts;
            }
        }

    }
}
