using System;
using System.Collections.Generic;
using DataStructures;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Linq;
using DataStructures.StatsTools;
using Logic;
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
            _universe = new Universe();
            var marketOne = new Market(TradeFlatteningData.longMarket, longMarket);
            var stratOne = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[2] { new DummyEntries(5, 35), new DummyExits(5, 30) }, marketOne,
                new StaticStopTarget(ExitPrices.NoStopTarget()));

            var marketTwo = new Market(TradeFlatteningData.mediumMarket, mediumMarket);
            var stratTwo = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[2] { new DummyEntries(5, 35), new DummyExits(5, 30) }, marketTwo,
                new StaticStopTarget(ExitPrices.NoStopTarget()));
          
            var marketThree = new Market(TradeFlatteningData.shortMarket, shortMarket);
            var stratThree = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[2] { new DummyEntries(5, 35), new DummyExits(5, 30) }, marketThree,
                new StaticStopTarget(ExitPrices.NoStopTarget()));
            
            _universe.AddMarket(marketOne, stratOne);
            _universe.AddMarket(marketTwo, stratTwo);
            _universe.AddMarket(marketThree, stratThree);
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
            Assert.Equal(TradeFlatteningData.mediumMarket.Length, _fixt._universe.GetArrayForReturns(_fixt.mediumMarket).Length);
            Assert.Equal(TradeFlatteningData.shortMarket.Length, _fixt._universe.GetArrayForReturns(_fixt.shortMarket).Length);
        }


        [Fact]
        private void ShouldFlattenOneTradeResults() {
            Universe newUniverse = new Universe();
            var market = new Market( new BidAskData[]
            {
                new BidAskData(new DateTime(),30,10,10,10,10 ),
                new BidAskData(new DateTime(),20,20,20,20,20 ),
                new BidAskData(new DateTime(),20,30,30,30,30 ),
                new BidAskData(new DateTime(),20,40,40,40,40 ),
            }, "test");
            var stratMarket = new StaticStrategy.StrategyBuilder().CreateStrategy
                (new IRuleSet[1] { new DummyEntries(4,1) }, market, new StaticStopTarget(ExitPrices.NoStopTarget()));

            newUniverse.AddMarket(market , stratMarket);
            UniverseTest t = new UniverseTest(newUniverse.GetObject("test"), new TestFactory.FixedBarExitTestOptions(5,7,1));
            var myArray = newUniverse.GetArrayForReturns("test");
            PrintTradesToReturnTimeLine(t.LongTests[0].Trades, myArray);
            var retvals = new double[] { 0, 0, 0.5, 1};


            Assert.Equal(retvals.ToArray(),myArray);
        }

        [Fact]
        private void ShouldFlattenTwoTradeResults() {
            Universe newUniverse = new Universe();
            var market = new Market(new BidAskData[]
            {
                new BidAskData(new DateTime(),30,10,10,10,10 ),
                new BidAskData(new DateTime(),20,20,20,20,20 ),
                new BidAskData(new DateTime(),20,30,30,30,30 ),
                new BidAskData(new DateTime(),20,40,40,40,40 ),
            }, "test");
            var stratMarket = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[1] { new DummyEntries(1,10) },
                market, new StaticStopTarget(ExitPrices.NoStopTarget()));


            newUniverse.AddMarket(market , stratMarket);
            UniverseTest t = new UniverseTest(newUniverse.GetObject("test"), new TestFactory.FixedBarExitTestOptions(5,7, 1));
            var myArray = newUniverse.GetArrayForReturns("test");
            PrintTradesToReturnTimeLine(t.LongTests[0].Trades, myArray);
            var retvals = new double[] { 0, 0,0.5,  (4 / 3.0)  };
            Assert.Equal(retvals.ToArray(), myArray);
        }

        [Fact]
        private void ShouldFlattenLongTradeResults() {
            var longMarketArray = _fixt._universe.GetArrayForReturns(_fixt.longMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.longMarketTrades, longMarketArray);
            Asserters.ArrayDoublesEqual(TradeFlatteningData.longExpectedResults, longMarketArray);
        }

        [Fact]
        private void ShouldFlattenMediumTradeResults() {
            var mediumMarketArray = _fixt._universe.GetArrayForReturns(_fixt.mediumMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.mediumMarketTrades, mediumMarketArray);
            Asserters.ArrayDoublesEqual(TradeFlatteningData.mediumExpectedResults, mediumMarketArray);
        }

        [Fact]
        private void ShouldFlattenShortTradeResults() {
            var shortMarketArray = _fixt._universe.GetArrayForReturns(_fixt.shortMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.shortMarketTrades, shortMarketArray);
            Asserters.ArrayDoublesEqual(TradeFlatteningData.shortExpectedResults, shortMarketArray);
        }

        [Fact]
        private void ShouldCollateLongAndShortMarket() {
            var longMarketArray = _fixt._universe.GetArrayForReturns(_fixt.longMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.longMarketTrades, longMarketArray);
            var shortMarketArray = _fixt._universe.GetArrayForReturns(_fixt.shortMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.shortMarketTrades, shortMarketArray);

            CollateTradesAcrossMarkets(longMarketArray, _fixt._universe.GetObject(_fixt.longMarket).MarketData.PriceData, shortMarketArray, 
                _fixt._universe.GetObject(_fixt.shortMarket).MarketData.PriceData);

            Asserters.ArrayDoublesEqual(TradeFlatteningData.LongAndShortResults, longMarketArray);
        }

        [Fact]
        private void ShouldCollateLongAndMediumMarket() {
            var longMarketArray = _fixt._universe.GetArrayForReturns(_fixt.longMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.longMarketTrades, longMarketArray);
            var mediumMarketArray = _fixt._universe.GetArrayForReturns(_fixt.mediumMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.mediumMarketTrades, mediumMarketArray);

            CollateTradesAcrossMarkets(longMarketArray, _fixt._universe.GetObject(_fixt.longMarket).MarketData.PriceData, mediumMarketArray, 
                _fixt._universe.GetObject(_fixt.mediumMarket).MarketData.PriceData);

            Asserters.ArrayDoublesEqual(TradeFlatteningData.LongAndMediumResults, longMarketArray);
        }

        [Fact]
        private void ShouldCollateMediumAndShortMarket() {
            var mediumMarketArray = _fixt._universe.GetArrayForReturns(_fixt.mediumMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.mediumMarketTrades, mediumMarketArray);
            var shortMarketArray = _fixt._universe.GetArrayForReturns(_fixt.shortMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.shortMarketTrades, shortMarketArray);

            CollateTradesAcrossMarkets(mediumMarketArray, _fixt._universe.GetObject(_fixt.mediumMarket).MarketData.PriceData, shortMarketArray, 
                _fixt._universe.GetObject(_fixt.shortMarket).MarketData.PriceData);

            Asserters.ArrayDoublesEqual(TradeFlatteningData.ShortAndMediumResults, mediumMarketArray);
        }

        [Fact]
        private void ShouldCollateLongShortAndMediumMarket() {
            var longMarketArray = _fixt._universe.GetArrayForReturns(_fixt.longMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.longMarketTrades, longMarketArray);
            var mediumMarketArray = _fixt._universe.GetArrayForReturns(_fixt.mediumMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.mediumMarketTrades, mediumMarketArray);
            var shortMarketArray = _fixt._universe.GetArrayForReturns(_fixt.shortMarket);
            PrintTradesToReturnTimeLine(TradeFlatteningData.shortMarketTrades, shortMarketArray);

            CollateTradesAcrossMarkets(longMarketArray, _fixt._universe.GetObject(_fixt.longMarket).MarketData.PriceData, mediumMarketArray,
                _fixt._universe.GetObject(_fixt.mediumMarket).MarketData.PriceData);
            CollateTradesAcrossMarkets(longMarketArray, _fixt._universe.GetObject(_fixt.longMarket).MarketData.PriceData, shortMarketArray, 
                _fixt._universe.GetObject(_fixt.shortMarket).MarketData.PriceData);

            Asserters.ArrayDoublesEqual(TradeFlatteningData.LongShortAndMediumResults, longMarketArray);
        }

        private void CollateTradesAcrossMarkets(double[] returnsDatum, BidAskData[] dataDatum, double[] returnsToAdd, BidAskData[] dataToReference) {
            for (int i = 0; i < dataToReference.Length; i++) {
                if (returnsToAdd[i] != 0) {
                    var date = dataToReference[i].Close.Ticks;
                    var relevantDatumItem = dataDatum.OrderBy(x => Math.Abs(x.Close.Ticks - date)).First();
                    for (int j = 0; j < dataDatum.Length; j++)
                        if (dataDatum[j].Close.Ticks == relevantDatumItem.Close.Ticks)
                            returnsDatum[j] += returnsToAdd[i];
                }
            }
        }


        private void PrintTradesToReturnTimeLine(List<Trade> test, double[] market) {
            foreach (var t in test)
                for (int j = 0; j < t.ResultTimeline.Length; j++)
                    market[t.MarketStart + j] += t.ResultTimeline[j].Return;
        }
    }
}
