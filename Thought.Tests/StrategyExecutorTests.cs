using System;
using DataStructures;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Thought.Tests
{
    public class StrategyExecutorTestsFixture
    {
        private UniverseObject myUniverse { get; set; }
        public List<Trade> TradesFalse { get; set; }
        public List<Trade> TradesTrue { get; set; }

        public StrategyExecutorTestsFixture() {
            myUniverse = new UniverseObject("test",Market.MarketBuilder.CreateMarket(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(5000)), new IRuleSet[1] { new DummyEntries(5, 10000) });
            exposure();
            noOverExposure();
        }

        private void exposure() {
            var executer = new StrategyExecuter(MarketSide.Bull, true, new ExitPrices(0.9,1.1));
            TradesTrue = executer.Execute(myUniverse);
        }


        private void noOverExposure() {
            var executer = new StrategyExecuter(MarketSide.Bull, false, new ExitPrices(0.9,1.1));
            TradesFalse = executer.Execute(myUniverse);
        }

    }

    public class StrategyExecutorTests : IClassFixture<StrategyExecutorTestsFixture>
    {
        private readonly StrategyExecutorTestsFixture _fixt;

        public StrategyExecutorTests(StrategyExecutorTestsFixture fixture) {
            _fixt = fixture;
        }

        [Fact]
        private void ShouldExecuteStrategy() {
            Assert.True(_fixt.TradesTrue.Count > 0);
        }

        [Fact]
        private void ShouldExecuteStrategyWithoutOverExposure() {
            Assert.True(_fixt.TradesFalse.Count > 0);
        }

        [Fact]
        private void MoreTradesExposedThanNotExposed() {
            Assert.True(_fixt.TradesTrue.Count > _fixt.TradesFalse.Count);
        }
    }
}
