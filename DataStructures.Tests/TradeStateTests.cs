using System;
using Xunit;

namespace DataStructures.Tests
{
    public class TradeStateTests
    {
        readonly BidAskData data = new BidAskData(time: new DateTime(),
    o_a: 5,
    o_b: 4.5,
    h_a: 7,
    h_b: 6.5,
    l_a: 2,
    l_b: 1.5,
    c_a: 6.2,
    c_b: 6,
    vol: 45);

        readonly BidAskData data2 = new BidAskData(time: new DateTime(),
    o_a: 10,
    o_b: 9.5,
    h_a: 12,
    h_b: 11.5,
    l_a: 8,
    l_b: 7.5,
    c_a: 9,
    c_b: 8.5,
    vol: 29);

        readonly TradeState stateOne = new TradeState();


        [Fact]
        private void ShouldInvestWithNoStop() {
            var state = stateOne.Invest(data, ExitPrices.NoStop(1.1), true);
            state = state.Continue(data2);

            Assert.Equal(double.NaN, state.StopPrice);
            Assert.Equal(1.1*data.Open_Ask, state.TargetPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTarget() {
            var state = stateOne.Invest(data, ExitPrices.NoTarget(1.1), false);
            state = state.Continue(data2);

            Assert.Equal(double.NaN, state.TargetPrice);
            Assert.Equal(1.1 * data.Open_Bid, state.StopPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTargetOrStop() {
            var state = stateOne.Invest(data, ExitPrices.NoStopTarget(), true);
            state = state.Continue(data2);

            Assert.Equal(double.NaN, state.StopPrice);
            Assert.Equal(double.NaN, state.TargetPrice);
        }

        [Fact]
        private void ShouldInvestLong() {
            var state = stateOne.Invest(data, new ExitPrices(0.995, 1.005), true );

            Assert.Equal(5, state.EntryPrice);
            Assert.True(state.Invested);
            Assert.Equal(0.2, state.Return);
            Assert.Equal(5 * 0.995, state.StopPrice);
            Assert.Equal(5 * 1.005, state.TargetPrice);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = stateOne.Invest(data, new ExitPrices(0.995, 1.005), true);
            var nextState = state.Continue(data2);

            Assert.Equal(5, nextState.EntryPrice);
            Assert.True(nextState.Invested);
            Assert.Equal(0.7, nextState.Return);
            Assert.Equal(5 * 0.995, nextState.StopPrice);
            Assert.Equal(5 * 1.005, nextState.TargetPrice);
        }


        [Fact]
        private void ShouldInvestShort() {
            var state = stateOne.Invest(data, new ExitPrices(1.005, 0.995), false);

            Assert.Equal(4.5, state.EntryPrice);
            Assert.True(state.Invested);
            Assert.Equal(-0.3777777777777778, state.Return);
            Assert.Equal(4.5 * 1.005, state.StopPrice);
            Assert.Equal(4.5 * 0.995, state.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = stateOne.Invest(data, new ExitPrices(1.005,0.995), false);
            var nextState = state.Continue(data2);

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

        [Fact]
        private void ShouldChangeStopAndTargets() {
            var longstate = stateOne.Invest(data, ExitPrices.NoStopTarget(), true);
            longstate = longstate.ContinueUpdateExits(data2, new ExitPrices(0.995, 1.005));

            var shortstate = stateOne.Invest(data, ExitPrices.NoStopTarget(), false);
            shortstate = shortstate.ContinueUpdateExits(data2, new ExitPrices(1.005, 0.995));

            Assert.Equal(5 * 0.995, longstate.StopPrice);
            Assert.Equal(5 * 1.005, longstate.TargetPrice); 
            Assert.Equal(4.5 * 1.005, shortstate.StopPrice);
            Assert.Equal(4.5 * 0.995, shortstate.TargetPrice);
        }

  
    }
}
