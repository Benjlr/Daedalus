using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures;
using DataStructures.StatsTools;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using TestUtils;
using Xunit;

namespace Thought.Tests
{

    public class OrganiserTestsFixture
    {
        public readonly string longMarket = "long Market Data";
        public readonly string mediumMarket = "medium Market Data";
        public readonly string shortMarket = "short Market Data";
        public Universe _universe { get; private set; }
        public UniverseTest[] _fbeTests { get; private set; }
        private UniverseTestFactory _testFactory { get; set; }

        public OrganiserTestsFixture() {
            BuildUniverse();
            PrepareAndRunTests();
        }

        private void BuildUniverse() {
            _universe = new Universe(new IRuleSet[2] { new DummyEntries(5, 35), new DummyExits(5, 30) });
            _universe.AddMarket(TradeFlatteningData.longMarket, longMarket);
            _universe.AddMarket(TradeFlatteningData.shorterMarket, mediumMarket);
            _universe.AddMarket(TradeFlatteningData.shortestMarket, shortMarket);
        }

        private void PrepareAndRunTests() {
            _testFactory = new UniverseTestFactory();
            _fbeTests = _testFactory.RunTests(_universe, new TestFactory.FixedBarExitTestOptions(5, 40, 5));
        }
    }


    public class OrganiserTests : IClassFixture<OrganiserTestsFixture>
    {
        private readonly OrganiserTestsFixture _fixt;
        public OrganiserTests(OrganiserTestsFixture fixt) {
            _fixt = fixt;
        }

        [Fact]
        private void ShouldCorrectlySizeOutputToMarket() {
            Assert.Equal(TradeFlatteningData.longMarket.Length, GetArrayForReturns(_fixt._universe.GetObject(_fixt.longMarket).MarketData).Length);
            Assert.Equal(TradeFlatteningData.shorterMarket.Length, GetArrayForReturns(_fixt._universe.GetObject(_fixt.mediumMarket).MarketData).Length);
            Assert.Equal(TradeFlatteningData.shortestMarket.Length, GetArrayForReturns(_fixt._universe.GetObject(_fixt.shortMarket).MarketData).Length);
        }

        [Fact]
        private void ShouldFlattenTradeResults() {
            

            Assert.True(true);
        }

        private double[] GetArrayForReturns(Market myMarket) {
            var market = "Wheat";
            var wheatMarket = _fixt._universe.Elements.FirstOrDefault(x => x.Name == market).MarketData;
            return new double[wheatMarket.RawData.Length];
        }

        private void PrintTradesToReturnTimeLine() {

        }


    }
}
