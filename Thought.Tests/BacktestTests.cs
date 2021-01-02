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
        public BackTestSpy(Universe markets, ITradeCollator collator) : base(markets, MarketSide.Bull, collator, false)
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
        public BackTestSpy BackTestSpyOne { get; set; }
        public BackTestSpy BackTestSpyTwo { get;  set; }
        public BackTestSpy BackTestSpyThree { get; set; }
        
        public ITradeCollator CollatorOne { get; set; }
        public ITradeCollator CollatorTwo { get; set; }
        public ITradeCollator CollatorThree { get; set; }

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

            var rules = new IRuleSet[] { new DummyEntries(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            CollatorOne = new SimpleCollator();
            BackTestSpyOne = new BackTestSpy(univers, CollatorOne);
        }
        private void SpyTwo() {
            var minuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(50), "shortMarket");
            var fiveminuteMarket = new Market(new RandomBars(new TimeSpan(0, 19, 0)).GenerateRandomMarket(110), "medMarket");
            var tenminuteMarket = new Market(new RandomBars(new TimeSpan(0, 35, 0)).GenerateRandomMarket(200), "longMarket");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new TimedExit(exits, MarketSide.Bull, 3));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            CollatorTwo = new SimpleCollator();
            BackTestSpyTwo = new BackTestSpy(univers, CollatorTwo);
        }
        private void SpyThree() {
            var m1 = new Market(new RandomBars(new TimeSpan(123564)).GenerateRandomMarket(224), "shortMarket");
            var m2 = new Market(new RandomBars(new TimeSpan(345)).GenerateRandomMarket(657), "medMarket");
            var m3 = new Market(new RandomBars(new TimeSpan(3215644)).GenerateRandomMarket(150), "longMarket");
            var m4 = new Market(new RandomBars(new TimeSpan(123354)).GenerateRandomMarket(700), "otherma");
            var m5 = new Market(new RandomBars(new TimeSpan(4561)).GenerateRandomMarket(1000), "anotherM!!");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m1, new TimedExit(exits, MarketSide.Bull, 3));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m2, new TimedExit(exits, MarketSide.Bull, 3));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m3, new TimedExit(exits, MarketSide.Bull, 3));
            var fiftminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m4, new TimedExit(exits, MarketSide.Bull, 3));
            var twentminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, m5, new TimedExit(exits, MarketSide.Bull, 3));
            var univers = new Universe();
            univers.AddMarket(m1, minuteStrat);
            univers.AddMarket(m2, fiveminuteStrat);
            univers.AddMarket(m3, tenminuteStrat);
            univers.AddMarket(m4, fiftminuteStrat);
            univers.AddMarket(m5, twentminuteStrat);

            CollatorThree = new SimpleCollator();
            BackTestSpyThree = new BackTestSpy(univers, CollatorThree);
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
            _fixture.BackTestSpyOne.RunBackTestByDates();
            Assert.Equal(104, _fixture.CollatorOne.Results.SelectMany(x=>x.Trades).Count());
            for(int i =1; i < _fixture.BackTestSpyOne.ExecutionDates.Count; i++){
                Assert.True(_fixture.BackTestSpyOne.ExecutionDates[i-1] <= _fixture.BackTestSpyOne.ExecutionDates[i]);
            }
        }

        [Fact]
        private void ShouldExecuteBackTestsByOutOfSyncDates() {
            _fixture.BackTestSpyTwo.RunBackTestByDates();
            Assert.Equal( 72, _fixture.CollatorTwo.Results.SelectMany(x => x.Trades).Count());
            for (int i = 1; i < _fixture.BackTestSpyTwo.ExecutionDates.Count; i++)
                Assert.True(_fixture.BackTestSpyTwo.ExecutionDates[i - 1] <= _fixture.BackTestSpyTwo.ExecutionDates[i]);
        }

        [Fact]
        private void ShouldExecuteBackTestsByRandomishDates() {
            _fixture.BackTestSpyThree.RunBackTestByDates();
            Assert.Equal(546, _fixture.CollatorThree.Results.SelectMany(x => x.Trades).Count());
            for (int i = 1; i < _fixture.BackTestSpyThree.ExecutionDates.Count; i++) {
                Assert.True(_fixture.BackTestSpyThree.ExecutionDates[i - 1] <= _fixture.BackTestSpyThree.ExecutionDates[i]);
            }
        }


    }
}