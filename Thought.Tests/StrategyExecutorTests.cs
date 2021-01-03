using DataStructures;
using Logic;
using RuleSets;
using RuleSets.Entry;
using System;
using System.Linq;
using TestUtils;
using Xunit;

namespace Thought.Tests
{
    public class StrategyExecutorTestsFixture
    {
        private TradingField field { get; set; }
        public SimpleCollator simpCollateOne { get; set; }
        public SimpleCollator simpCollateTwo { get; set; }

        public StrategyExecutorTestsFixture() {
            var market = new Market(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(500), "test");
            var myStrat = new StaticStrategy.StrategyBuilder().CreateStrategy
                (new IRuleSet[1] {new DummyEntries(5, 500)}, market, new StaticStopTarget(new ExitPrices(0.8, 1.2)));
            field = new TradingField(market,myStrat);
            exposure();
            noOverExposure();
        }

        private void exposure() {
            simpCollateOne = new SimpleCollator(true);
            var executer = new LongStrategyExecuter((x, y) => simpCollateOne.AddTrade(y,x,"test"), (x,y)=>simpCollateOne.AddExposureItem(new MarketExposure("test",y), x), simpCollateOne.CanEnter);
            executer.Init(field);
            executer.ExecuteAll();
        }


        private void noOverExposure() {
            simpCollateTwo = new SimpleCollator(false);
            var executer = new LongStrategyExecuter((x, y) => simpCollateTwo.AddTrade(y, x, "test"), (x, y) => simpCollateTwo.AddExposureItem(new MarketExposure("test", y), x), simpCollateTwo.CanEnter);
            executer.Init(field);
            executer.ExecuteAll();
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
            Assert.True(_fixt.simpCollateOne.Results.SelectMany(x=>x.Trades).Any());
        }

        [Fact]
        private void ShouldExecuteStrategyWithoutOverExposure() {
            Assert.True(_fixt.simpCollateTwo.Results.SelectMany(x => x.Trades).Any());
        }

        [Fact]
        private void MoreTradesExposedThanNotExposed() {
            Assert.True(_fixt.simpCollateOne.Results.SelectMany(x => x.Trades).Count() > _fixt.simpCollateTwo.Results.SelectMany(x => x.Trades).Count());
        }
    }
}
