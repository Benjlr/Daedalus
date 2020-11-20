using System;
using Xunit;

namespace DataStructures.Tests
{
    public class TradeStateTests
    {
        [Fact]
        private void ShouldExitAndGenerateTradeLong() {
            var longstate = TradeStateGenerator.Invest(ExitPrices.NoStopTarget(), MarketSide.Bull, 0, 5, 4.5);
            longstate.Continue(7);
            longstate.Continue(9);
            longstate.Continue(5);
            var longHist = longstate.Exit(2);

            Assert.Equal(4, longHist.MarketEnd);
            Assert.Equal(0, longHist.MarketStart);
            Assert.Equal((2-5)/5.0, longHist.Drawdown);
            Assert.Equal((2 - 5) / 5.0, longHist.Result);
            Assert.False(longHist.Win);
            Assert.Equal(new double[] {(4.5-5)/5.0, (7-5)/5.0, (9 - 5) / 5.0, (5 - 5) / 5.0 , (2 - 5) / 5.0 }, longHist.Results);
        }

        [Fact]
        private void ShouldExitAndGenerateTradeShort() {
            var shortstate = TradeStateGenerator.Invest(ExitPrices.NoStopTarget(), MarketSide.Bear, 0, 11, 12);
            shortstate.Continue(13);
            shortstate.Continue(15);
            shortstate.Continue(14);
            shortstate.Continue(10);
            var shortHist = shortstate.Exit(8);

            Assert.Equal(5, shortHist.MarketEnd);
            Assert.Equal(0, shortHist.MarketStart);
            Assert.Equal((11-15)/11.0, shortHist.Drawdown);
            Assert.Equal((11 - 8) / 11.0, shortHist.Result);
            Assert.True(shortHist.Win);
            Assert.Equal(new double[] { (11 - 12) / 11.0, (11 - 13) / 11.0, (11 - 15) / 11.0, (11 - 14) / 11.0, (11 - 10) / 11.0, (11 - 8) / 11.0 }, shortHist.Results);
        }


        [Fact]
        private void ShouldInvestWithNoStop() {
            var state = TradeStateGenerator.Invest(ExitPrices.NoStop(1.1), MarketSide.Bull, 0, 10 , 9 );
            state.Continue(12);

            Assert.Equal(double.NaN, state.CurrentState.StopPrice);
            Assert.Equal(1.1 * 10, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTarget() {
            var state = TradeStateGenerator.Invest(ExitPrices.NoTarget(1.1), MarketSide.Bear, 0, 14.5, 15);
            state.Continue(12);

            Assert.Equal(double.NaN, state.CurrentState.TargetPrice);
            Assert.Equal(1.1 * 14.5, state.CurrentState.StopPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTargetOrStop() {
            var state = TradeStateGenerator.Invest(ExitPrices.NoStopTarget(), MarketSide.Bull, 0, 10, 9);
            state.Continue(4);

            Assert.Equal(double.NaN, state.CurrentState.StopPrice);
            Assert.Equal(double.NaN, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldInvestLong() {
            var state = TradeStateGenerator.Invest(new ExitPrices(0.995, 1.005), MarketSide.Bull, 0, 11, 10);

            Assert.Equal(11, state.CurrentState.EntryPrice);
            Assert.Equal((10-11)/11.0, state.CurrentState.Return);
            Assert.Equal(11 * 0.995, state.CurrentState.StopPrice);
            Assert.Equal(11 * 1.005, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = TradeStateGenerator.Invest(new ExitPrices(0.995, 1.005), MarketSide.Bull, 0, 15,14);
            state.Continue(18);

            Assert.Equal(15, state.CurrentState.EntryPrice);
            Assert.Equal((18-15)/15.0, state.CurrentState.Return);
            Assert.Equal(15 * 0.995, state.CurrentState.StopPrice);
            Assert.Equal(15 * 1.005, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldInvestShort() {
            var state = TradeStateGenerator.Invest(new ExitPrices(1.005, 0.995), MarketSide.Bear, 0, 100, 102);

            Assert.Equal(100, state.CurrentState.EntryPrice);
            Assert.Equal(-0.02, state.CurrentState.Return);
            Assert.Equal(100 * 1.005, state.CurrentState.StopPrice);
            Assert.Equal(100 * 0.995, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = TradeStateGenerator.Invest(new ExitPrices(1.005, 0.995), MarketSide.Bear, 0, 35,36.5);
            state.Continue(42);

            Assert.Equal(35, state.CurrentState.EntryPrice);
            Assert.Equal((35- 42) /35.0, state.CurrentState.Return);
            Assert.Equal(35 * 1.005, state.CurrentState.StopPrice);
            Assert.Equal(35 * 0.995, state.CurrentState.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargets() {
            var longstate = TradeStateGenerator.Invest(ExitPrices.NoStopTarget(), MarketSide.Bull, 0, 7, 6.5);
            longstate.ContinueUpdateExits(10, new ExitPrices(0.995, 1.005));

            var shortstate = TradeStateGenerator.Invest(ExitPrices.NoStopTarget(), MarketSide.Bear, 0, 6.5, 7);
            shortstate.ContinueUpdateExits(10, new ExitPrices(1.005, 0.995));

            Assert.Equal(7 * 0.995, longstate.CurrentState.StopPrice);
            Assert.Equal(7 * 1.005, longstate.CurrentState.TargetPrice);
            Assert.Equal(6.5 * 1.005, shortstate.CurrentState.StopPrice);
            Assert.Equal(6.5 * 0.995, shortstate.CurrentState.TargetPrice);
        }

    }
}
