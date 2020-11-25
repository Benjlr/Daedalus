using System;
using System.Collections.Generic;
using Xunit;

namespace DataStructures.Tests
{
    public class TradeStateTests
    {
        [Fact]
        private void ShouldExitAndGenerateTradeLong() {
            List<Trade> trades = new List<Trade>();
            var longstate = TradeStateGenerator.Invest(MarketSide.Bull, 
                new TradePrices(ExitPrices.NoStopTarget(), 5), 
                new Action<Trade>((x) => {
                trades.Add(x);
            }), 0);
            longstate.Continue(new BidAskData(new DateTime(2020,01,01),5,5,5,5, 4.5, 4.5, 4.5,4.5,1 ));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 5, 5, 10, 10, 4.5, 4.5, 9, 9, 1));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 5, 5, 8, 8, 4.5, 4.5,5, 5 , 1));
            longstate.Exit(2);

            Assert.Equal(4, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal((2-5)/5.0, trades[0].Drawdown);
            Assert.Equal((2 - 5) / 5.0, trades[0].Result);
            Assert.False(trades[0].Win);
            Assert.Equal(new double[] {(4.5-5)/5.0, (7-5)/5.0, (9 - 5) / 5.0, (5 - 5) / 5.0 , (2 - 5) / 5.0 }, trades[0].Results);
        }

        [Fact]
        private void ShouldExitAndGenerateTradeShort() {
            List<Trade> trades = new List<Trade>();
            var shortstate = TradeStateGenerator.Invest(MarketSide.Bear, 
                new TradePrices(ExitPrices.NoStopTarget(), 11),
                new Action<Trade>((x) => {
                    trades.Add(x);
                }), 0);
            shortstate.Continue(new BidAskData(new DateTime(),13,13,13,13,13,13,13,13,1 ));
            shortstate.Continue(new BidAskData(new DateTime(), 15, 15, 15, 15, 15, 15, 15, 15, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 14, 14, 14, 14, 14, 14, 14, 14, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));
            shortstate.Exit(8);

            Assert.Equal(5, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal((11-15)/11.0, trades[0].Drawdown);
            Assert.Equal((11 - 8) / 11.0, trades[0].Result);
            Assert.True(trades[0].Win);
            Assert.Equal(new double[] { (11 - 12) / 11.0, (11 - 13) / 11.0, (11 - 15) / 11.0, (11 - 14) / 11.0, (11 - 10) / 11.0, (11 - 8) / 11.0 }, trades[0].Results);
        }


        [Fact]
        private void ShouldInvestWithNoStop() {
            var state = TradeStateGenerator.Invest(MarketSide.Bull,
               new TradePrices(ExitPrices.NoStop(1.1),10), 
               new Action<Trade>((x) => { }), 0);
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.TradeLimits.StopPrice);
            Assert.Equal(1.1 * 10, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTarget() {
            var state = TradeStateGenerator.Invest(MarketSide.Bear,
                new TradePrices(ExitPrices.NoTarget(1.1), 15),
                new Action<Trade>((x) => { }), 0);
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.TradeLimits.TargetPrice);
            Assert.Equal(1.1 * 14.5, state.TradeLimits.StopPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTargetOrStop() {
            var state = TradeStateGenerator.Invest(MarketSide.Bull,
                new TradePrices(ExitPrices.NoStopTarget(), 9),
                new Action<Trade>((x) => { }), 0);

            state.Continue(new BidAskData(new DateTime(), 4, 4, 4, 4, 4, 4, 4, 4, 1));

            Assert.Equal(double.NaN, state.TradeLimits.StopPrice);
            Assert.Equal(double.NaN, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestLong() {
            var state = TradeStateGenerator.Invest(MarketSide.Bull,
                new TradePrices(ExitPrices.NoStopTarget(), 11),
                new Action<Trade>((x) => { }), 0);
            state.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));

            Assert.Equal(11, state.TradeLimits.EntryPrice);
            Assert.Equal((10-11)/11.0, state.CurrentReturn);
            Assert.Equal(11 * 0.995, state.TradeLimits.StopPrice);
            Assert.Equal(11 * 1.005, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = TradeStateGenerator.Invest(MarketSide.Bull,
                new TradePrices(new ExitPrices(0.995, 1.005), 15),
                new Action<Trade>((x) => { }), 0);

            state.Continue(new BidAskData(new DateTime(), 18, 18, 18, 18, 18, 18, 18, 18, 1));

            Assert.Equal(15, state.TradeLimits.EntryPrice);
            Assert.Equal((18-15)/15.0, state.CurrentReturn);
            Assert.Equal(15 * 0.995, state.TradeLimits.StopPrice);
            Assert.Equal(15 * 1.005, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestShort() {
            var state = TradeStateGenerator.Invest(MarketSide.Bear,
                new TradePrices(new ExitPrices(1.005, 0.995), 100),
                new Action<Trade>((x) => { }), 0);

            state.Continue(new BidAskData(new DateTime(), 98, 98, 98, 98, 98, 98, 98, 98, 1));

            Assert.Equal(100, state.TradeLimits.EntryPrice);
            Assert.Equal(-0.02, state.CurrentReturn);
            Assert.Equal(100 * 1.005, state.TradeLimits.StopPrice);
            Assert.Equal(100 * 0.995, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = TradeStateGenerator.Invest(MarketSide.Bear,
                new TradePrices(new ExitPrices(1.005, 0.995), 35),
                new Action<Trade>((x) => { }), 0);

            state.Continue(new BidAskData(new DateTime(), 42, 42, 42, 42, 42, 42, 42, 42, 1));

            Assert.Equal(35, state.TradeLimits.EntryPrice);
            Assert.Equal((35- 42) /35.0, state.CurrentReturn);
            Assert.Equal(35 * 1.005, state.TradeLimits.StopPrice);
            Assert.Equal(35 * 0.995, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsLong() {
            var longstate = TradeStateGenerator.Invest(MarketSide.Bull,
                new TradePrices(ExitPrices.NoStopTarget(), 7),
                new Action<Trade>((x) => { }), 0);

            longstate.ContinueUpdateExits(new BidAskData(new DateTime(),8,8,8,8,8,8,8,8,1), new ExitPrices(0.995, 1.005));
            
            Assert.Equal(7 * 0.995, longstate.TradeLimits.StopPrice);
            Assert.Equal(7 * 1.005, longstate.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsShort() {
            var shortstate = TradeStateGenerator.Invest(MarketSide.Bear,
                new TradePrices(ExitPrices.NoStopTarget(), 6.5),
                new Action<Trade>((x) => { }), 0);

            shortstate.ContinueUpdateExits(new BidAskData(new DateTime(), 8, 8, 8, 8, 8, 8, 8, 8, 1), new ExitPrices(1.005, 0.995));

            Assert.Equal(6.5 * 1.005, shortstate.TradeLimits.StopPrice);
            Assert.Equal(6.5 * 0.995, shortstate.TradeLimits.TargetPrice);
        }

    }
}
