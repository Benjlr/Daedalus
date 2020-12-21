using DataStructures;
using RuleSets;
using RuleSets.Entry;
using System;
using System.Collections.Generic;
using TestUtils;
using Xunit;

namespace Logic.Tests
{

    public class StrategyTests
    {


        [Fact]
        private void ShouldGenerateEntryGivenRuleSetAndBars() {
            var ruleSet = new IRuleSet[1] {new DummyEntries(1, 50)};
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(50);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(ruleSet, new Market(bars, "name"));
            for (int i = 1; i < bars.Length; i++)
                Assert.True(strat.IsEntry(bars[i], i));
        }

        [Fact]
        private void ShouldGenerateExitGivenRuleSetAndBars() {
            var ruleSet = new IRuleSet[1] {new DummyExits(1, 50)};
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(50);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(ruleSet, new Market(bars, "name"));
            for (int i = 1; i < bars.Length; i++)
                Assert.True(strat.IsExit(bars[i], i));
        }

        [Fact]
        private void ShouldGenerateStaggeredEntryGivenRuleSetAndBars() {
            var ruleSet = new IRuleSet[1] {new DummyEntries(5, 50)};
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(50);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(ruleSet, new Market(bars, "name"));
            for (int i = 1; i < bars.Length; i++) {
                if ((i-1) % 5 == 0)
                    Assert.True(strat.IsEntry(bars[i], i));
                else
                    Assert.False(strat.IsEntry(bars[i], i));
            }
        }

        [Fact]
        private void ShouldGenerateStaggeredExitGivenRuleSetAndBars() {
            var ruleSet = new IRuleSet[1] {new DummyExits(5, 50)};
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(50);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(ruleSet, new Market(bars, "name"));
            for (int i = 1; i < bars.Length; i++) {
                if ((i - 1) % 5 == 0)
                    Assert.True(strat.IsExit(bars[i], i));
                else
                    Assert.False(strat.IsExit(bars[i], i));
            }
        }

        [Fact]
        private void ShouldNotGenerateEntries() {
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(50);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[0], new Market(bars, "name"));
            for (int i = 1; i < bars.Length; i++) {
                Assert.False(strat.IsEntry(bars[i], i));
                Assert.False(strat.IsExit(bars[i], i));
            }
        }

        [Fact]
        private void ShouldGenerateInitialStopTarget() {
            var bars = new RandomBars(TimeSpan.FromDays(1)).GenerateRandomMarket(1);
            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[0], new Market(bars, "name"));
            Assert.Equal(ExitPrices.StopOnly(0.98).StopPercentage, strat.AdjustStopTarget(new TradePrices(), new DatedResult()).StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(0.98).TargetPercentage, strat.AdjustStopTarget(new TradePrices(), new DatedResult()).TargetPercentage);
        }

        [Fact]
        private void ShouldIncrementStopLong() {
            BidAskData[] bars = new BidAskData[] {
            new BidAskData(new DateTime(2020, 01, 01), 100, 100, 100, 100, 100, 100, 100, 100, 1),
            new BidAskData(new DateTime(2020, 01, 02), 102, 102, 102, 102, 102, 102, 102, 102, 1),
            new BidAskData(new DateTime(2020, 01, 03), 103, 103, 103, 103, 103, 103, 103, 103, 1),
            new BidAskData(new DateTime(2020, 01, 04), 104, 104, 104, 104, 104, 104, 104, 104, 1)
            };

            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[0], new Market(bars, "name"));
            var longstate = new LongTradeGenerator(0, new TradePrices(strat.AdjustStopTarget(new TradePrices(), new DatedResult()), 100), null);

            longstate.Continue(bars[0]);
            var newExit = strat.AdjustStopTarget(longstate.TradeLimits, longstate.LastResult);
            Assert.Equal(ExitPrices.StopOnly(0.98).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(0.98).TargetPercentage, newExit.TargetPercentage);
            longstate.UpdateExits(newExit);

            longstate.Continue(bars[1]);
             newExit = strat.AdjustStopTarget(longstate.TradeLimits, longstate.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1).TargetPercentage, newExit.TargetPercentage);
            longstate.UpdateExits(newExit);

            longstate.Continue(bars[2]);
            newExit = strat.AdjustStopTarget(longstate.TradeLimits, longstate.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1.01).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1.01).TargetPercentage, newExit.TargetPercentage);
            longstate.UpdateExits(newExit);

            longstate.Continue(bars[3]);
            newExit = strat.AdjustStopTarget(longstate.TradeLimits, longstate.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1.02).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1.02).TargetPercentage, newExit.TargetPercentage);
            longstate.UpdateExits(newExit);

            longstate.Continue(bars[2]);
            newExit = strat.AdjustStopTarget(longstate.TradeLimits, longstate.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1.02).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1.02).TargetPercentage, newExit.TargetPercentage);
            longstate.UpdateExits(newExit);
        }

        [Fact]
        private void ShouldIncrementStopShort() {
            BidAskData[] bars = new BidAskData[] {
            new BidAskData(new DateTime(2020, 01, 01), 100, 100, 100, 100, 100, 100, 100, 100, 1),
            new BidAskData(new DateTime(2020, 01, 02), 98, 98, 98, 98, 98, 98, 98, 98, 1),
            new BidAskData(new DateTime(2020, 01, 03), 97, 97, 97, 97, 97, 97, 97, 97, 1),
            new BidAskData(new DateTime(2020, 01, 04), 96, 96, 96, 96, 96, 96, 96, 96, 1)
            };

            var strat = new StaticStrategy.StrategyBuilder().CreateStrategy(new IRuleSet[0], new Market(bars, "name"));
            var shortState = new ShortTradeGenerator(0, new TradePrices(strat.AdjustStopTarget(new TradePrices(), new DatedResult()), 100), null);

            shortState.Continue(bars[0]);
            var newExit = strat.AdjustStopTarget(shortState.TradeLimits, shortState.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1.02).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1.02).TargetPercentage, newExit.TargetPercentage);
            shortState.UpdateExits(newExit);

            shortState.Continue(bars[1]);
            newExit = strat.AdjustStopTarget(shortState.TradeLimits, shortState.LastResult);
            Assert.Equal(ExitPrices.StopOnly(1).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(1).TargetPercentage, newExit.TargetPercentage);
            shortState.UpdateExits(newExit);

            shortState.Continue(bars[2]);
            newExit = strat.AdjustStopTarget(shortState.TradeLimits, shortState.LastResult);
            Assert.Equal(ExitPrices.StopOnly(0.99).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(0.99).TargetPercentage, newExit.TargetPercentage);
            shortState.UpdateExits(newExit);

            shortState.Continue(bars[3]);
            newExit = strat.AdjustStopTarget(shortState.TradeLimits, shortState.LastResult);
            Assert.Equal(ExitPrices.StopOnly(0.98).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(0.98).TargetPercentage, newExit.TargetPercentage);
            shortState.UpdateExits(newExit);

            shortState.Continue(bars[2]);
            newExit = strat.AdjustStopTarget(shortState.TradeLimits, shortState.LastResult);
            Assert.Equal(ExitPrices.StopOnly(0.98).StopPercentage, newExit.StopPercentage);
            Assert.Equal(ExitPrices.StopOnly(0.98).TargetPercentage, newExit.TargetPercentage);
            shortState.UpdateExits(newExit);
        }
    }
}
