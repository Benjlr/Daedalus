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
        public BackTestSpy(Universe markets) : base(markets)
        {
        }

        public List<DateTime> ExecutionDates{get;set;} = new List<DateTime>();

        protected override void IterateThroughMarkets(){
            ExecutionDates.Add(new DateTime(laggardDate));
            base.IterateThroughMarkets();
        }
    }

    public class BackTestFixture
    {
        public Backtest BackTest { get; private set; }
        public BackTestSpy BackTestSpyOne { get; private set; }
        public BackTestSpy BackTestSpyTwo { get; private set; }
        public BackTestSpy BackTestSpyThree { get; private set; }
        public List<Trade> TradesGenerated { get; private set; }

     
        private StaticStrategy _strat { get; set; }
        private StaticStrategy _stratForSpy { get; set; }

        public BackTestFixture() {
            GenerateGeneraleBackTest();
            GenerateBacktestSpies();
        }

        private void GenerateGeneraleBackTest() {
            var universeData = new Universe();
            
            var generateRandomMarket = new Market(new RandomBars(new TimeSpan(0, 20, 0)).GenerateRandomMarket(1000), "longTest");
            var longStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[1] {new DummyEntries(5, 1000)}, generateRandomMarket,
                new StaticStopTarget(new ExitPrices(0.95,1.05)));

            var bidAskDatas = new Market(new RandomBars(new TimeSpan(0, 20, 0)).GenerateRandomMarket(1000), "mediumMarket");
            var mediumStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[1] {new DummyEntries(5, 1000)}, generateRandomMarket,
                new StaticStopTarget(new ExitPrices(0.95, 1.05)));
            
            var randomMarket = new Market(new RandomBars(new TimeSpan(0, 20, 0)).GenerateRandomMarket(1000), "shortMarket");
            var shortStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[1] { new DummyEntries(5, 1000) }, generateRandomMarket,
                new StaticStopTarget(new ExitPrices(0.95, 1.05)));
            
            universeData.AddMarket(generateRandomMarket, longStrat);
            universeData.AddMarket(bidAskDatas, mediumStrat);
            universeData.AddMarket(randomMarket, shortStrat);

            BackTest = new Backtest(universeData);
            TradesGenerated = BackTest.RunBackTest(new LongStrategyExecuter(true));
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

        public List<Trade> GetTrades() {
            return new List<Trade>()
            {
                new Trade(new DatedResult[]
                {
                    new DatedResult(new DateTime(1, 1, 1, 1, 1, 1).Ticks, 0.1, -0.1),
                    new DatedResult(new DateTime(1, 1, 10, 1, 1, 1).Ticks, 0.15, -0.15),
                    new DatedResult(new DateTime(1, 1, 20, 1, 1, 1).Ticks, 0.2, -0.15),
                    new DatedResult(new DateTime(1, 1, 30, 1, 1, 1).Ticks, 0.25, -0.2),
                }, 0),
                new Trade(new DatedResult[]
                {
                    new DatedResult(new DateTime(1, 1, 1, 1, 1, 1).Ticks, 0.1, -0.05),
                    new DatedResult(new DateTime(1, 1, 5, 1, 1, 1).Ticks, 0.05, -0.05),
                    new DatedResult(new DateTime(1, 1, 10, 1, 1, 1).Ticks, -0.05, -0.15),
                    new DatedResult(new DateTime(1, 1, 15, 1, 1, 1).Ticks, 0.05, -0.15),
                    new DatedResult(new DateTime(1, 1, 20, 1, 1, 1).Ticks, 0.1, -0.15),
                }, 0),
                new Trade(new DatedResult[]
                {
                    new DatedResult(new DateTime(1, 1, 1, 1, 1, 1).Ticks, 0.1, -0.05),
                    new DatedResult(new DateTime(1, 1, 2, 1, 1, 1).Ticks, 0.05, -0.05),
                    new DatedResult(new DateTime(1, 1, 3, 1, 1, 1).Ticks, -0.05, -0.1),
                    new DatedResult(new DateTime(1, 1, 4, 1, 1, 1).Ticks, 0.05, -0.11),
                    new DatedResult(new DateTime(1, 1, 5, 1, 1, 1).Ticks, 0.08, -0.12),
                    new DatedResult(new DateTime(1, 1, 6, 1, 1, 1).Ticks, 0.12, -0.13),
                    new DatedResult(new DateTime(1, 1, 7, 1, 1, 1).Ticks, 0.14, -0.14),
                    new DatedResult(new DateTime(1, 1, 8, 1, 1, 1).Ticks, 0.16, -0.15),
                    new DatedResult(new DateTime(1, 1, 9, 1, 1, 1).Ticks, 0.1, -0.16),
                    new DatedResult(new DateTime(1, 1, 10, 1, 1, 1).Ticks, 0.1, -0.2),
                }, 0)
            };

        }
    }

    public class BacktestTests : IClassFixture<BackTestFixture>
    {
        private readonly BackTestFixture _fixture;

        public BacktestTests(BackTestFixture fixt) {
            _fixture = fixt;
        }

        [Fact]
        private void GeneratesResults() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromDays(1), _fixture.TradesGenerated);
            Assert.True(results.Count > 0);
            foreach (var trade in results) {
                Assert.True(trade.Return != 0);
                Assert.True(trade.Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForDays() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromDays(1), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x => x.Drawdown == 0));
            Assert.False(results.All(x => x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(TimeSpan.FromDays(1).Ticks, results[i].Date - results[i-1].Date);
                Assert.True(results[i].Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForHours() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromHours(1), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x => x.Drawdown == 0));
            Assert.False(results.All(x => x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(TimeSpan.FromHours(1).Ticks, results[i].Date - results[i - 1].Date);
                Assert.True(results[i].Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForArbitraryTicks() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromTicks(29556547847), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x=>x.Drawdown == 0));
            Assert.False(results.All(x=>x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(29556547847, results[i].Date - results[i - 1].Date);
                Assert.True(results[i].Date != 0);
            }
        }


        [Fact]
        private void ShouldGenerateResultsInCorrectOrder() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromDays(10), _fixture.GetTrades());
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 11).Ticks, 0.15 - 0.05 + 0.1, (-0.3 + -0.2)/3), results[0]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 21).Ticks, 0.2 + 0.1, -0.15), results[1]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 31).Ticks, 0.25, -0.2), results[2]);
        }

        [Fact]
        private void ShouldGenerateResultsInCorrectOrderOnSmallScale() {
            var results = _fixture.BackTest.ResultsCollater.ParseResults(TimeSpan.FromDays(5), _fixture.GetTrades());
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 6).Ticks, 0.1 + 0.05 + 0.08, (-0.1-0.05 -0.12)/3), results[0]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 11).Ticks, 0.15 - 0.05 + 0.1, (-0.15-0.15 -0.2)/3), results[1]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 16).Ticks, 0.15 + 0.05, (-0.15-0.15 )/2), results[2]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 21).Ticks, 0.2 + 0.1 , (-0.15- 0.15) /2), results[3]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 26).Ticks, 0.2, (-0.15)/1), results[4]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 31).Ticks, 0.25, (-0.2)/1), results[5]);
        }

        [Fact]
        private void ShouldExecuteBackTestsByDates(){
            var trades = _fixture.BackTestSpyOne.RunBackTestByDates(
                new LongStrategyExecuter(false));
            Assert.Equal(520/5, trades.Count);
            for(int i =1; i < _fixture.BackTestSpyOne.ExecutionDates.Count; i++){
                Assert.True(_fixture.BackTestSpyOne.ExecutionDates[i-1] <= _fixture.BackTestSpyOne.ExecutionDates[i]);
            }
        }

        [Fact]
        private void ShouldExecuteBackTestsByOutOfSyncDates() {
            var trades = _fixture.BackTestSpyTwo.RunBackTestByDates(
                new LongStrategyExecuter(false));
            Assert.Equal( 360/5, trades.Count);
            for (int i = 1; i < _fixture.BackTestSpyTwo.ExecutionDates.Count; i++)
                Assert.True(_fixture.BackTestSpyTwo.ExecutionDates[i - 1] <= _fixture.BackTestSpyTwo.ExecutionDates[i]);
        }

        [Fact]
        private void ShouldExecuteBackTestsByRandomishDates() {
            var trades = _fixture.BackTestSpyThree.RunBackTestByDates(new LongStrategyExecuter(false));
            Assert.Equal((2731 / 5)+1, trades.Count);
            for (int i = 1; i < _fixture.BackTestSpyThree.ExecutionDates.Count; i++) {
                Assert.True(_fixture.BackTestSpyThree.ExecutionDates[i - 1] <= _fixture.BackTestSpyThree.ExecutionDates[i]);
            }
        }

        private void AssertDatedResult(DatedResult expected, DatedResult results) {
            Assert.Equal(expected.Date, results.Date);
            Assert.Equal(expected.Return, results.Return,6);
            Assert.Equal(expected.Drawdown, results.Drawdown,6);
        }
    }
}