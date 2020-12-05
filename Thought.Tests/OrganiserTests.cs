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
            _fbeTests = _testFactory.RunTests(_universe, new TestFactory.FixedBarExitTestOptions(5, 10, 5));
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
            Assert.Equal(TradeFlatteningData.longMarket.Length, _fixt._universe.GetArrayForReturns(_fixt.longMarket).Length);
            Assert.Equal(TradeFlatteningData.shorterMarket.Length, _fixt._universe.GetArrayForReturns(_fixt.mediumMarket).Length);
            Assert.Equal(TradeFlatteningData.shortestMarket.Length, _fixt._universe.GetArrayForReturns(_fixt.shortMarket).Length);
        }


        [Fact]
        private void ShouldFlattenOneTradeResults() {
            Universe newUniverse = new Universe(new IRuleSet[]{new DummyEntries(4,1) });
            newUniverse.AddMarket(new SessionData[]
            {
                new SessionData(new DateTime(),30,10,10,10,10 ),
                new SessionData(new DateTime(),20,20,20,20,20 ),
                new SessionData(new DateTime(),20,30,30,30,30 ),
                new SessionData(new DateTime(),20,40,40,40,40 ),
            }, "test");
            UniverseTest t = new UniverseTest(newUniverse.GetObject("test"), new TestFactory.FixedBarExitTestOptions(5,7,1));
            var myArray = newUniverse.GetArrayForReturns("test");
            PrintTradesToReturnTimeLine(t.LongTests[0].Trades, myArray);
            var retvals = new double[] { 0, 0, 0.5, 1};


            Assert.Equal(retvals.ToArray(),myArray);
        }

        [Fact]
        private void ShouldFlattenTwoTradeResults() {
            Universe newUniverse = new Universe(new IRuleSet[] { new DummyEntries(1, 10) });
            newUniverse.AddMarket(new SessionData[]
            {
                new SessionData(new DateTime(),30,10,10,10,10 ),
                new SessionData(new DateTime(),20,20,20,20,20 ),
                new SessionData(new DateTime(),20,30,30,30,30 ),
                new SessionData(new DateTime(),20,40,40,40,40 ),
            }, "test");
            UniverseTest t = new UniverseTest(newUniverse.GetObject("test"), new TestFactory.FixedBarExitTestOptions(5, 7, 1));
            var myArray = newUniverse.GetArrayForReturns("test");
            PrintTradesToReturnTimeLine(t.LongTests[0].Trades, myArray);
            var retvals = new double[] { 0, 0,0.5,  (4 / 3.0)  };


            Assert.Equal(retvals.ToArray(), myArray);
        }

        [Fact]
        private void ShouldFlattenManyTradeResults() {
            for (int i = 0; i < TradeFlatteningData.longMarketTradesFiveInterval.Count; i++) 
                Assert.Equal(TradeFlatteningData.longMarketTradesFiveInterval[i], _fixt._fbeTests.FirstOrDefault(x => x.Name.Equals(_fixt.longMarket)).LongTests[0].Trades[i]);
            for (int i = 0; i < TradeFlatteningData.longMarketTradesTenInterval.Count; i++) 
                Assert.Equal(TradeFlatteningData.longMarketTradesTenInterval[i], _fixt._fbeTests.FirstOrDefault(x=>x.Name.Equals(_fixt.longMarket)).LongTests[1].Trades[i]);
            for (int i = 0; i < TradeFlatteningData.shortestMarketTradesFiveInterval.Count; i++) 
                Assert.Equal(TradeFlatteningData.shortestMarketTradesFiveInterval[i], _fixt._fbeTests.FirstOrDefault(x=>x.Name.Equals(_fixt.shortMarket)).LongTests[0].Trades[i]);
            for (int i = 0; i < TradeFlatteningData.shortestMarketTradesTenInterval.Count; i++) 
                Assert.Equal(TradeFlatteningData.shortestMarketTradesTenInterval[i], _fixt._fbeTests.FirstOrDefault(x=>x.Name.Equals(_fixt.shortMarket)).LongTests[1].Trades[i]);
        }




        private void PrintTradesToReturnTimeLine(List<Trade> test, double[] timeline) {
            for (int i = 0; i < test.Count; i++) {
                for (int j = 0; j < test[i].Results.Length; j++) {
                    timeline[test[i].MarketStart + j] += test[i].Results[j];
                }
            }
        }


    }
}
