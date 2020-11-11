using Logic.Analysis.StrategyRunners;
using RuleSets;
using System;
using Xunit;

namespace Logic.Tests
{
    public class TradeStateTests
    {
        MarketData data = new MarketData(time: new DateTime(),
    o_a: 5,
    o_b: 4.5,
    h_a: 7,
    h_b: 6.5,
    l_a: 2,
    l_b: 1.5,
    c_a: 6.2,
    c_b: 6,
    vol: 45);

        MarketData data2 = new MarketData(time: new DateTime(),
    o_a: 10,
    o_b: 9.5,
    h_a: 12,
    h_b: 11.5,
    l_a: 8,
    l_b: 7.5,
    c_a: 9,
    c_b: 8.5,
    vol: 29);

        TradeState stateOne = new TradeState();

        [Fact]
        private void ShouldInvestLong() {
            var state = stateOne.InvestLong(data);

            Assert.Equal(5, state.EntryPrice);
            Assert.True(state.Invested);
            Assert.Equal(0.2, state.Return);
            Assert.Equal(5 * 0.995, state.StopPrice);
            Assert.Equal(5 * 1.005, state.TargetPrice);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = stateOne.InvestLong(data);
            var nextState = state.ContinueLong(data2);

            Assert.Equal(5, nextState.EntryPrice);
            Assert.True(nextState.Invested);
            Assert.Equal(0.7, nextState.Return);
            Assert.Equal(5 * 0.995, nextState.StopPrice);
            Assert.Equal(5 * 1.005, nextState.TargetPrice);
        }


        [Fact]
        private void ShouldInvestShort() {
            var state = stateOne.InvestShort(data);

            Assert.Equal(4.5, state.EntryPrice);
            Assert.True(state.Invested);
            Assert.Equal(-0.3777777777777778, state.Return);
            Assert.Equal(4.5 * 1.005, state.StopPrice);
            Assert.Equal(4.5 * 0.995, state.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = stateOne.InvestShort(data);
            var nextState = state.ContinueShort(data2);

            Assert.Equal(4.5, nextState.EntryPrice);
            Assert.True(nextState.Invested);
            Assert.Equal(-1, nextState.Return);
            Assert.Equal(4.5 * 1.005, nextState.StopPrice);
            Assert.Equal(4.5 * 0.995, nextState.TargetPrice);
        }

        [Fact]
        private void ShouldDoNothing() {
            var state = stateOne.DoNothing();

            Assert.Equal(0, state.EntryPrice);
            Assert.False(state.Invested);
            Assert.Equal(0, state.Return);
            Assert.Equal(0, state.StopPrice);
            Assert.Equal(0, state.TargetPrice);
        }
    }
}
