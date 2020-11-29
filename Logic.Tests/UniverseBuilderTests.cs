using DataStructures;
using DataStructures.StatsTools;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.Linq;
using Logic.Metrics;
using Xunit;

namespace Logic.Tests
{
    public class UniverseBuilderTests
    {
        private Universe _universe;
        public UniverseBuilderTests() {
            _universe = new Universe(new IRuleSet[2]{new DummyEntries(2,40), new DummyExits(3,40) });
            _universe.AddMarket(Markets.futures_wheat_5);
            _universe.AddMarket(Markets.aud_usd_5);
            _universe.AddMarket(Markets.ASX20());
        }

        [Fact]
        private void ShouldOpenMultipleMarkets() {
            Assert.Equal(22, _universe.Elements.Count);
        }

        [Fact]
        private void ShouldCalculateMultipleStrategyResults() {
            Assert.True(_universe.Elements.All(x=>x.Strategy.Entries.Any(y => y)));
            Assert.True(_universe.Elements.All(x=>x.Strategy.Exits.Any(y => y)));
        }

        [Fact]
        private void ShouldRunTestsForUnivers() {

            Assert.True(true);
        }

        public class Universe
        {
            public List<UniverseObject> Elements { get; }
            private IRuleSet[] Ruleset { get; }

            public Universe(IRuleSet[] rules) {
                Ruleset = rules;
                Elements = new List<UniverseObject>();
            }

            public void AddMarket(string market) {
                if (!Elements.Any(x => x.Name.Equals(market))) 
                    Elements.Add(new UniverseObject(market, OpenMarket(market), Ruleset));
            }

            public void AddMarket(List<string> markets) {
                for (int i = 0; i < markets.Count; i++)
                    AddMarket(markets[i]);
            }

            private Market OpenMarket(string market) {
                if (DataLoader.CheckDataType(market).Equals(typeof(SessionData)))
                    return Market.MarketBuilder.CreateMarket(DataLoader.LoadConsolidatedData(market));
                else
                    return Market.MarketBuilder.CreateMarket(DataLoader.LoadBidAskData(market));
            }

            public List<ITest[]> RunFSTETests() {
                List<ITest[]> retval = new List<ITest[]>();
                for (int i = 0; i < Elements.Count; i++) 
                    retval.AddRange(TestFactory.GenerateFixedStopTargetExitTest(
                        Elements[i].Strategy, Elements[i].MarketData, 
                        new FixedStopTargetExitTestOptions(0.03, 0.03, 0.2, 15, MarketSide.Bull));
                return retval;
            }

        }

        public readonly struct UniverseObject
        {
            public string Name { get;  }
            public Market MarketData { get;  }
            public Strategy Strategy { get;  }

            public UniverseObject(string name, Market market, IRuleSet[] rules) {
                Name = name;
                MarketData = market;
                Strategy = Strategy.StrategyBuilder.CreateStrategy(rules, market);
            }
        }

        public struct UniverseTestObject
        {
            public string Name { get; }
            public List<ITest[]> TestResults { get; }

            public UniverseTestObject

        }
    }
}
