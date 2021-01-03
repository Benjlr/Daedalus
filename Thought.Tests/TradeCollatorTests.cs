using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using Logic;
using RuleSets;
using RuleSets.Entry;
using TestUtils;
using Xunit;
using Xunit.Sdk;

namespace Thought.Tests
{
    public class ResultsCollatorFixture
    {
        public BackTestSpySlowIteration _backTestSpyOne { get; set; }
        public BackTestSpySlowIteration _backTestSpyTwo { get; set; }

        public Portfolio CollatorOne { get; set; }
        public Portfolio CollatorTwo { get; set; }

        public ResultsCollatorFixture() {
            GenerateBacktestSpies();
        }

        private void GenerateBacktestSpies() {
            SpyOne();
            SpyTwo();
        }

        private void SpyOne() {
            var minuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(5), "shortMarket");
            var fiveminuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(10), "medMarket");
            var tenminuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(15), "longMarket");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            CollatorOne = new Portfolio(10000, 0.05, false);
            _backTestSpyOne = new BackTestSpySlowIteration(univers, CollatorOne);
            _backTestSpyOne.RunBackTestByDates();

        }
        private void SpyTwo() {
            var minuteMarket = new Market(new RandomBars(new TimeSpan(0, 5, 0)).GenerateRandomMarket(10), "shortMarket");
            var fiveminuteMarket = new Market(new RandomBars(new TimeSpan(0, 20, 0)).GenerateRandomMarket(20), "medMarket");
            var tenminuteMarket = new Market(new RandomBars(new TimeSpan(0, 40, 0)).GenerateRandomMarket(30), "longMarket");

            var rules = new IRuleSet[] { new DummyEntries(5, 1000) };
            var exits = ExitPrices.NoStopTarget();
            var minuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, minuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var fiveminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, fiveminuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var tenminuteStrat = new StaticStrategy.StrategyBuilder().CreateStrategy(rules, tenminuteMarket, new TimedExit(exits, MarketSide.Bull, 4));
            var univers = new Universe();
            univers.AddMarket(minuteMarket, minuteStrat);
            univers.AddMarket(fiveminuteMarket, fiveminuteStrat);
            univers.AddMarket(tenminuteMarket, tenminuteStrat);

            CollatorTwo = new Portfolio(10000,0.05,false);
            _backTestSpyTwo = new BackTestSpySlowIteration(univers, CollatorTwo);
            _backTestSpyTwo.RunBackTestByDates();
        }
    }

    public class TradeCollatorTests: IClassFixture<ResultsCollatorFixture>
    {
        private ResultsCollatorFixture _fixture { get; set; }

        public TradeCollatorTests(ResultsCollatorFixture fixture) {
            _fixture = fixture;
        }

        [Fact]
        private void ShouldProperlyCategoriseManyTrades() {
            Assert.Equal(6, _fixture.CollatorOne.Results.SelectMany(x=>x.Trades).Count());
            Assert.Single(_fixture.CollatorOne.Results.First(x=>x.MarketName.Equals("shortMarket")).Trades);
            Assert.Equal(2, _fixture.CollatorOne.Results.First(x=>x.MarketName.Equals("medMarket")).Trades.Count);
            Assert.Equal(3, _fixture.CollatorOne.Results.First(x=>x.MarketName.Equals("longMarket")).Trades.Count);
        }

        [Fact]
        private void ShouldProperlyCategoriseTrades() {
            Assert.Equal(12, _fixture.CollatorTwo.Results.SelectMany(x => x.Trades).Count());
            Assert.Equal(2,_fixture.CollatorTwo.Results.First(x => x.MarketName.Equals("shortMarket")).Trades.Count);
            Assert.Equal(4, _fixture.CollatorTwo.Results.First(x => x.MarketName.Equals("medMarket")).Trades.Count);
            Assert.Equal(6, _fixture.CollatorTwo.Results.First(x => x.MarketName.Equals("longMarket")).Trades.Count);
        }

        [Fact]
        private void ShouldIterateCorrectAmountofOpenTrades() {
            var answers = new List<int>() {0, 0, 0, 1, 3, 3, 3, 3, 4, 5, 5, 5, 5, 6, 6};
            Asserters.ListDoublesEqual(answers, _fixture._backTestSpyOne.TradeCount);
        }

        [Fact]
        private void ShouldIterateCorrectAmountofManyOpenTrades() {
            var answers = new List<int>() {0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 8, 8, 8, 8, 8, 8, 9, 9, 9, 9, 9, 10, 10, 10, 10, 10, 11, 11, 11, 11, 12, 12};
            Asserters.ListDoublesEqual(answers, _fixture._backTestSpyTwo.TradeCount);
        }

        [Fact(Skip = "Random bar generation means exposure will vary between tests")]
        private void ShouldIterateCorrectExposureOfOpenTrades() {
            var answers = new List<double>()
            {
                0,
                -0.038160651,
                0.006313067,
                -0.126652111,
                0,
                0,
                -0.12068198,
                -0.07736808,
                -0.024199214,
                0,
                0,
                -0.066209614,
                -0.045979627,
                0,
                0,

            };
            Assert.Equal(57, _fixture._backTestSpyOne.TradeExposure.Count);
            for (int i = 0; i < _fixture._backTestSpyOne.TradeExposure.Count; i++) {
                //Assert.Equal();
            }
        }

        [Fact(Skip = "Random bar generation means exposure will vary between tests")]
        private void ShouldIterateCorrectExposureofManyOpenTrades() {
            Assert.Equal(57, _fixture._backTestSpyTwo.TradeExposure.Count);
            for (int i = 0; i < _fixture._backTestSpyTwo.TradeExposure.Count; i++) {
                //Assert.Equal();
            }
        }
    }

    public class BackTestSpySlowIteration : Backtest
    {
        private Portfolio _collator { get; set; }
        public List<int> TradeCount { get; set; }
        public List<double> TradeExposure { get; set; }

        public BackTestSpySlowIteration(Universe markets, Portfolio collator) : base(markets, MarketSide.Bull, collator) {
            _collator = collator;
            TradeCount = new List<int>();
            TradeExposure = new List<double>();
        }


        protected override void IterateThroughMarkets() {
            base.IterateThroughMarkets();
            TradeExposure.Add(_collator.CurrentExposure.Values.Sum(x=>x.Exposure.Return));
            TradeCount.Add(_collator.Results.SelectMany(x=>x.Trades).Count());
        }
    }
}
