using DataStructures;
using RuleSets;
using RuleSets.Entry;
using System;
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
    }
}
