using DataStructures;
using RuleSets;
using RuleSets.Entry;
using System;
using System.Collections.Generic;
using System.Linq;
using Logic;
using TestUtils;
using Xunit;

namespace Thought.Tests
{
    public class BackTestSpy : Backtest
    {
        public BackTestSpy(Universe markets) : base(markets, MarketSide.Bull, false)
        {
        }

        public List<DateTime> ExecutionDates{get;set;} = new List<DateTime>();

        protected override void IterateThroughMarkets(){
            ExecutionDates.Add(new DateTime(_earliestDate));
            base.IterateThroughMarkets();
        }
    }

    public class BackTestFixture
    {
        public BackTestSpy BackTestSpyOne { get; private set; }
        public BackTestSpy BackTestSpyTwo { get; private set; }
        public BackTestSpy BackTestSpyThree { get; private set; }
        
        public BackTestFixture() {
            GenerateBacktestSpies();
        }

        private void GenerateBacktestSpies() {
            SpyOne();
            SpyTwo();
            SpyThree();
        }

        private void SpyOne() {
            var minuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(400), "shortMarket");
            var fiveminuteMarket = new Market(new RandomBars(new TimeSpan(0, 10, 0)).GenerateRandomMarket(80), "medMarket");
            var tenminuteMarket = new Market(new RandomBars(new TimeSpan(0, 15, 0)).GenerateRandomMarket(40), "longMarket");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000), new DummyExits(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new StaticStopTarget(exits));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new StaticStopTarget(exits));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new StaticStopTarget(exits));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            BackTestSpyOne = new BackTestSpy(univers);
        }
        private void SpyTwo() {
            var minuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(50), "shortMarket");
            var fiveminuteMarket = new Market(new RandomBars(new TimeSpan(0, 19, 0)).GenerateRandomMarket(110), "medMarket");
            var tenminuteMarket = new Market(new RandomBars(new TimeSpan(0, 35, 0)).GenerateRandomMarket(200), "longMarket");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000), new DummyExits(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new StaticStopTarget(exits));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new StaticStopTarget(exits));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new StaticStopTarget(exits));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            BackTestSpyTwo = new BackTestSpy(univers);
        }
        private void SpyThree() {
            var m1 = new Market(new RandomBars(new TimeSpan(123564)).GenerateRandomMarket(224), "shortMarket");
            var m2 = new Market(new RandomBars(new TimeSpan(345)).GenerateRandomMarket(657), "medMarket");
            var m3 = new Market(new RandomBars(new TimeSpan(3215644)).GenerateRandomMarket(150), "longMarket");
            var m4 = new Market(new RandomBars(new TimeSpan(123354)).GenerateRandomMarket(700), "otherma");
            var m5 = new Market(new RandomBars(new TimeSpan(4561)).GenerateRandomMarket(1000), "anotherM!!");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000), new DummyExits(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m1, new StaticStopTarget(exits));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m2, new StaticStopTarget(exits));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m3, new StaticStopTarget(exits));
            var fiftminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m4, new StaticStopTarget(exits));
            var twentminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m5, new StaticStopTarget(exits));
            var univers = new Universe();
            univers.AddMarket(m1, minuteStrat);
            univers.AddMarket(m2, fiveminuteStrat);
            univers.AddMarket(m3, tenminuteStrat);
            univers.AddMarket(m4, fiftminuteStrat);
            univers.AddMarket(m5, twentminuteStrat);

            BackTestSpyThree = new BackTestSpy(univers);
        }
    }

    public class BacktestTests : IClassFixture<BackTestFixture>
    {
        private readonly BackTestFixture _fixture;

        public BacktestTests(BackTestFixture fixt) {
            _fixture = fixt;
        }


        [Fact]
        private void ShouldExecuteBackTestsByDates(){
            var trades = _fixture.BackTestSpyOne.RunBackTestByDates();
            Assert.Equal(520/5, trades.Count);
            for(int i =1; i < _fixture.BackTestSpyOne.ExecutionDates.Count; i++){
                Assert.True(_fixture.BackTestSpyOne.ExecutionDates[i-1] <= _fixture.BackTestSpyOne.ExecutionDates[i]);
            }
        }

        [Fact]
        private void ShouldExecuteBackTestsByOutOfSyncDates() {
            var trades = _fixture.BackTestSpyTwo.RunBackTestByDates();
            Assert.Equal( 360/5, trades.Count);
            for (int i = 1; i < _fixture.BackTestSpyTwo.ExecutionDates.Count; i++)
                Assert.True(_fixture.BackTestSpyTwo.ExecutionDates[i - 1] <= _fixture.BackTestSpyTwo.ExecutionDates[i]);
        }

        [Fact]
        private void ShouldExecuteBackTestsByRandomishDates() {
            var trades = _fixture.BackTestSpyThree.RunBackTestByDates();
            Assert.Equal((2731 / 5)+1, trades.Count);
            for (int i = 1; i < _fixture.BackTestSpyThree.ExecutionDates.Count; i++) {
                Assert.True(_fixture.BackTestSpyThree.ExecutionDates[i - 1] <= _fixture.BackTestSpyThree.ExecutionDates[i]);
            }
        }


    }
}