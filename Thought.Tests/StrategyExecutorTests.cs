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
        public List<Trade> TradesTrue { get; set; }
        public List<Trade> TradesFalse { get; set; }

        public StrategyExecutorTestsFixture() {
            exposure();
            noOverExposure();
        }

        private void exposure() {
            var executer = new StrategyExecuter(MarketSide.Bull, true);
            var obj = new UniverseObject("test", Market.MarketBuilder.CreateMarket(TradeFlatteningData.longMarket), new IRuleSet[1] {new DummyEntries(5, 40)});
            TradesTrue = executer.Execute(obj);
        }

        private void noOverExposure() {
            var executer = new StrategyExecuter(MarketSide.Bull, false);
            var obj = new UniverseObject("test", Market.MarketBuilder.CreateMarket(TradeFlatteningData.longMarket), new IRuleSet[1] {new DummyEntries(5, 40)});
            TradesFalse = executer.Execute(obj);
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
