using System;
using System.Collections.Generic;
using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Linq;
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
            _fbeTests = _testFactory.RunTests(_universe, new TestFactory.FixedBarExitTestOptions(5, 10, 2));
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
            Assert.Equal(TradeFlatteningData.longMarket.Length, GetArrayForReturns(_fixt.longMarket).Length);
            Assert.Equal(TradeFlatteningData.shorterMarket.Length, GetArrayForReturns(_fixt.mediumMarket).Length);
            Assert.Equal(TradeFlatteningData.shortestMarket.Length, GetArrayForReturns(_fixt.shortMarket).Length);
        }


        [Fact]
        private void ShouldFlattenOneTradeResults() {
            Universe newUniverse = new Universe(new IRuleSet[]{new DummyEntries(4,1) });
            newUniverse.AddMarket(new SessionData[]{new SessionData(new DateTime(),30,10,10,10,10 ),new SessionData(new DateTime(),20,20,20,20,20 )  }, "test");
            UniverseTest t = new UniverseTest(newUniverse.GetObject("test"), new TestFactory.FixedBarExitTestOptions(5,7,1));
            
            var retvals = new List<double>(){};
            
            Assert.Equal(1,1);
        }

        private double[] GetArrayForReturns(string myMarket) {
            var market = _fixt._universe.GetObject(myMarket).MarketData;
            return new double[market.RawData.Length];
        }

        private double[] PrintTradesToReturnTimeLine(List<Trade> test) {
            test.
        }


    }
}
