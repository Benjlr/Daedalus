using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DataStructures.Tests
{
    public class TradeStateTests
    {
        [Fact]
        private void ShouldExitAndGenerateTradeLong() {
            List<Trade> trades = new List<Trade>();
            var longstate = new LongTradeGenerator(0, 
                new TradePrices(ExitPrices.NoStopTarget(), 5), 
                new Action<Trade>((x) => { trades.Add(x); }));
            longstate.Continue(new BidAskData(new DateTime(2020,01,01),4.5,4.5,5,5, 4.5, 4.5, 4.5,4.5,1 ));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 9, 9, 10, 10, 4.5, 4.5, 9, 9, 1));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 5, 5, 8, 8, 4.5, 4.5,5, 5 , 1));
            longstate.Exit(new DateTime(2020, 01, 01),2);

            Assert.Equal(3, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal((2-5)/5.0, trades[0].Drawdown);
            Assert.Equal((2 - 5) / 5.0, trades[0].FinalResult);
            Assert.False(trades[0].Win);
            Assert.Equal(new double[] {(4.5/5.0)-1, (9/5.0)-1, 0, (2 / 5.0) -1}, trades[0].ResultArray);
        }

        [Fact]
        private void ShouldExitAndGenerateTradeShort() {
            List<Trade> trades = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0, 
                new TradePrices(ExitPrices.NoStopTarget(), 11),
                new Action<Trade>((x) => { trades.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(),13,13,13,13,13,13,13,13,1 ));
            shortstate.Continue(new BidAskData(new DateTime(), 15, 15, 15, 15, 15, 15, 15, 15, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 14, 14, 14, 14, 14, 14, 14, 14, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));
            shortstate.Exit(new DateTime(),8);

            Assert.Equal(4, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal(1- (15/11.0), trades[0].Drawdown);
            Assert.Equal(1-(8 / 11.0) , trades[0].FinalResult);
            Assert.True(trades[0].Win);
            Assert.Equal(new double[] { 1-(13 / 11.0), 1-(15 / 11.0), 1-(14 / 11.0), 1-(10 / 11.0), 1-( 8 / 11.0 )}, trades[0].ResultArray);
        }


        [Fact]
        private void ShouldInvestWithNoStop() {
            var state = new LongTradeGenerator(0,
               new TradePrices(ExitPrices.TargetOnly(1.1),10), 
               new Action<Trade>((x) => { }));
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.TradeLimits.StopPrice);
            Assert.Equal(1.1 * 10, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTarget() {
            var state = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(1.1), 15),
                new Action<Trade>((x) => { }));
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.TradeLimits.TargetPrice);
            Assert.Equal(1.1 * 15, state.TradeLimits.StopPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTargetOrStop() {
            var state = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 9),
                new Action<Trade>((x) => { }) );

            state.Continue(new BidAskData(new DateTime(), 4, 4, 4, 4, 4, 4, 4, 4, 1));

            Assert.Equal(double.NaN, state.TradeLimits.StopPrice);
            Assert.Equal(double.NaN, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestLong() {
            var state = new LongTradeGenerator(0,
                new TradePrices(new ExitPrices(0.7, 1.3), 11),
                new Action<Trade>((x) => { }));
            state.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));

            Assert.Equal(11, state.TradeLimits.EntryPrice);
            Assert.Equal((10/11.0)-1, state.CurrentReturn);
            Assert.Equal(11 * 0.7, state.TradeLimits.StopPrice);
            Assert.Equal(11 * 1.3, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = new LongTradeGenerator(0,
                new TradePrices(new ExitPrices(0.9, 1.1), 15),
                new Action<Trade>((x) => { }));

            state.Continue(new BidAskData(new DateTime(), 16, 16, 16, 16, 16, 16, 16, 16, 1));

            Assert.Equal(15, state.TradeLimits.EntryPrice);
            Assert.Equal((16/15.0)-1, state.CurrentReturn);
            Assert.Equal(15 * 0.9, state.TradeLimits.StopPrice);
            Assert.Equal(15 * 1.1, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldInvestShort() {
            var state = new ShortTradeGenerator(0,
                new TradePrices(new ExitPrices(1.05, 0.95), 100),
                new Action<Trade>((x) => { }));

            state.Continue(new BidAskData(new DateTime(), 98, 98, 98, 98, 98, 98, 98, 98, 1));

            Assert.Equal(100, state.TradeLimits.EntryPrice);
            Assert.Equal(1- (98/100.0), state.CurrentReturn);
            Assert.Equal(100 * 1.05, state.TradeLimits.StopPrice);
            Assert.Equal(100 * 0.95, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = new ShortTradeGenerator(0,
                new TradePrices(new ExitPrices(1.5, 0.6), 35),
                new Action<Trade>((x) => { }));

            state.Continue(new BidAskData(new DateTime(), 42, 42, 42, 42, 42, 42, 42, 42, 1));

            Assert.Equal(35, state.TradeLimits.EntryPrice);
            Assert.Equal(1-(42/ 35.0) , state.CurrentReturn);
            Assert.Equal(35 * 1.5, state.TradeLimits.StopPrice);
            Assert.Equal(35 * 0.6, state.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsLong() {
            var longstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 7),
                new Action<Trade>((x) => { }));

            longstate.UpdateExits( new ExitPrices(0.995, 1.005));
            longstate.Continue(new BidAskData(new DateTime(), 8, 8, 8, 8, 8, 8, 8, 8, 1));
            
            Assert.Equal(7 * 0.995, longstate.TradeLimits.StopPrice);
            Assert.Equal(7 * 1.005, longstate.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsShort() {
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 6.5),
                new Action<Trade>((x) => { }));

            shortstate.UpdateExits(new ExitPrices(1.005, 0.995));
            shortstate.Continue(new BidAskData(new DateTime(), 8, 8, 8, 8, 8, 8, 8, 8, 1));

            Assert.Equal(6.5 * 1.005, shortstate.TradeLimits.StopPrice);
            Assert.Equal(6.5 * 0.995, shortstate.TradeLimits.TargetPrice);
        }

        [Fact]
        private void ShouldExitShortStopHit() {
            var myTrade = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(1.2), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 11, 11, 11, 11, 11, 11, 11, 11, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 10, 10, 13, 13, 9, 7.5, 9, 9, 1));

            Assert.Equal(-0.2, myTrade.Last().FinalResult,5);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitLongStopHit() {
            var myTrade = new List<Trade>();
            var shortstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(0.8), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 9, 9, 9, 9, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 7.5, 7.5, 9, 9, 1));

            Assert.Equal(-0.2, myTrade.Last().FinalResult,5);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitShortTargetHit() {
            var myTrade = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.TargetOnly(0.8), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 11, 11, 11, 11, 11, 11, 11, 11, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 10, 10, 11, 11, 7, 7, 9, 9, 1));

            Assert.Equal(0.2, myTrade.Last().FinalResult,5);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitLongTargetHit() {
            var myTrade = new List<Trade>();
            var shortstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.TargetOnly(1.2), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 9, 9, 9, 9, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 15, 15, 9, 9, 9, 9, 1));

            Assert.Equal(0.2, myTrade.Last().FinalResult,5);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitShortOpenBelowStop() {
            var myTrade = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(1.2), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 11, 11, 11, 11, 11, 11, 11, 11, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 14, 14, 13, 13, 9, 7.5, 9, 9, 1));

            Assert.Equal(-0.4, myTrade.Last().FinalResult,5);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitLongOpenBelowStop() {
            var myTrade = new List<Trade>();
            var shortstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(0.8), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 9, 9, 9, 9, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 5, 5, 9, 9, 7.5, 7.5, 9, 9, 1));

            Assert.Equal(-0.5, myTrade.Last().FinalResult);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitShortOpenBelowTarget() {
            var myTrade = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.TargetOnly(0.8), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 11, 11, 11, 11, 11, 11, 11, 11, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 5, 5, 11, 11, 7, 7, 9, 9, 1));

            Assert.Equal(0.5, myTrade.Last().FinalResult);
            Assert.Equal(2, myTrade.Last().Duration);
        }

        [Fact]
        private void ShouldExitLongOpenAboveTarget() {
            var myTrade = new List<Trade>();
            var shortstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.TargetOnly(1.2), 10),
                new Action<Trade>((x) => { myTrade.Add(x); }));

            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 9, 9, 9, 9, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 20, 20, 15, 15, 9, 9, 9, 9, 1));

            Assert.Equal(1, myTrade.Last().FinalResult);
            Assert.Equal(2, myTrade.Last().Duration);
        }
    }
}
