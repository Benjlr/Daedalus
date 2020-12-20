using DataStructures;
using Logic;
using RuleSets;
using RuleSets.Entry;
using System;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Thought.Tests
{
    public class StrategyExecutorTestsFixture
    {
        private TradingField field { get; set; }
        public List<Trade> TradesFalse { get; set; }
        public List<Trade> TradesTrue { get; set; }

        public StrategyExecutorTestsFixture() {
            var market = new Market(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(5000), "test");
            var myStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[1] {new DummyEntries(5, 5000)}, market);
            field = new TradingField(market,myStrat);
            exposure();
            noOverExposure();
        }

        private void exposure() {
            var executer = new LongStrategyExecuter(true);
            TradesTrue = executer.Execute(field);
        }


        private void noOverExposure() {
            var executer = new LongStrategyExecuter(false);
            TradesFalse = executer.Execute(field);
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
