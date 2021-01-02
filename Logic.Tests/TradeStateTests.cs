using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using Xunit;

namespace Logic.Tests
{

    public class TradeStateTests
    {
        [Fact]
        private void ShouldExitAndGenerateTradeLong() {
            List<Trade> trades = new List<Trade>();
            var longstate = new LongTradeGenerator(0, 
                new TradePrices(ExitPrices.NoStopTarget(), 5),
                (x, y) => trades.Add(y), null);
            longstate.Continue(new BidAskData(new DateTime(2020,01,01),4.5,4.5,5,5, 4.5, 4.5, 4.5,4.5,1 ));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 9, 9, 10, 10, 4.5, 4.5, 9, 9, 1));
            longstate.Continue(new BidAskData(new DateTime(2020, 01, 01), 5, 5, 8, 8, 4.5, 4.5,5, 5 , 1));
            longstate.Exit(new DateTime(2020, 01, 01).Ticks,2);

            Assert.Equal(3, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal((2-5)/5.0, trades[0].FinalDrawdown);
            Assert.Equal((2 - 5) / 5.0, trades[0].FinalResult);
            Assert.False(trades[0].Win);
            Assert.Equal(new double[] {(4.5/5.0)-1, (9/5.0)-1, 0, (2 / 5.0) -1}, trades[0].ResultArray);
        }

        [Fact]
        private void ShouldExitAndGenerateTradeShort() {
            List<Trade> trades = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0, 
                new TradePrices(ExitPrices.NoStopTarget(), 11),
                (x, y) => trades.Add(y), null);

            shortstate.Continue(new BidAskData(new DateTime(),13,13,13,13,13,13,13,13,1 ));
            shortstate.Continue(new BidAskData(new DateTime(), 15, 15, 15, 15, 15, 15, 15, 15, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 14, 14, 14, 14, 14, 14, 14, 14, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));
            shortstate.Exit(1,8);

            Assert.Equal(4, trades[0].MarketEnd);
            Assert.Equal(0, trades[0].MarketStart);
            Assert.Equal(1- (15/11.0), trades[0].FinalDrawdown);
            Assert.Equal(1-(8 / 11.0) , trades[0].FinalResult);
            Assert.True(trades[0].Win);
            Assert.Equal(new double[] { 1-(13 / 11.0), 1-(15 / 11.0), 1-(14 / 11.0), 1-(10 / 11.0), 1-( 8 / 11.0 )}, trades[0].ResultArray);
        }


        [Fact]
        private void ShouldInvestWithNoStop() {
            var state = new LongTradeGenerator(0,
               new TradePrices(ExitPrices.TargetOnly(1.1),10),
               null, null);
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.StopEntryTarget.StopPrice);
            Assert.Equal(1.1 * 10, state.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTarget() {
            var state = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(1.1), 15),
                null, null);
            state.Continue(new BidAskData(new DateTime(), 12, 12, 12, 12, 12, 12, 12, 12, 1));

            Assert.Equal(double.NaN, state.StopEntryTarget.TargetPrice);
            Assert.Equal(1.1 * 15, state.StopEntryTarget.StopPrice);
        }

        [Fact]
        private void ShouldInvestWithNoTargetOrStop() {
            var state = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 9),
                null, null);

            state.Continue(new BidAskData(new DateTime(), 4, 4, 4, 4, 4, 4, 4, 4, 1));

            Assert.Equal(double.NaN, state.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, state.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldInvestLong() {
            var stop = 0.7;
            var target = 1.3;
            var state = new LongTradeGenerator(0,
                new TradePrices(new ExitPrices(stop, target), 11),
                null, null);
            state.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 10, 10, 10, 10, 1));
            state.Continue(new BidAskData(new DateTime(), 10, 10, 10, 10, 5, 5, 10, 10, 1));
            var trade = state.TradeBuilder.CompileTrade();

            Assert.Equal(11, state.StopEntryTarget.EntryPrice, 6);
            Assert.Equal(-0.3, trade.FinalResult, 6);
            Assert.Equal(-0.3 , trade.FinalDrawdown, 6);
            Assert.Equal(11 * stop, state.StopEntryTarget.StopPrice,6);
            Assert.Equal(11 * target, state.StopEntryTarget.TargetPrice, 6);
        }

        [Fact]
        private void ShouldContinueLong() {
            var state = new LongTradeGenerator(0,
                new TradePrices(new ExitPrices(0.9, 1.1), 15),
                null, null);

            state.Continue(new BidAskData(new DateTime(), 16, 16, 16, 16, 16, 16, 16, 16, 1));
            state.Continue(new BidAskData(new DateTime(), 16, 16, 16, 16, 16, 16, 16, 16, 1));
            var trade = state.TradeBuilder.CompileTrade();

            Assert.Equal(15, state.StopEntryTarget.EntryPrice, 6);
            Assert.Equal((16/15.0)-1, trade.FinalResult, 6);
            Assert.Equal(0 , trade.FinalDrawdown, 6);
            Assert.Equal(15 * 0.9, state.StopEntryTarget.StopPrice, 6);
            Assert.Equal(15 * 1.1, state.StopEntryTarget.TargetPrice, 6);
        }

        [Fact]
        private void ShouldInvestShort() {
            var state = new ShortTradeGenerator(0,
                new TradePrices(new ExitPrices(1.05, 0.95), 100),
                null, null);

            state.Continue(new BidAskData(new DateTime(), 98, 98, 102, 102, 98, 98, 98, 98, 1));
            state.Continue(new BidAskData(new DateTime(), 98, 98, 98, 98, 98, 98, 98, 98, 1));
            var trade = state.TradeBuilder.CompileTrade();

            Assert.Equal(100, state.StopEntryTarget.EntryPrice);
            Assert.Equal(1- (98/100.0), trade.FinalResult);
            Assert.Equal(1 - (102 / 100.0), trade.FinalDrawdown);
            Assert.Equal(100 * 1.05, state.StopEntryTarget.StopPrice);
            Assert.Equal(100 * 0.95, state.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldContinueShort() {
            var state = new ShortTradeGenerator(0,
                new TradePrices(new ExitPrices(1.5, 0.6), 35),
                null, null);

            state.Continue(new BidAskData(new DateTime(), 42, 42, 42, 42, 42, 42, 42, 42, 1));
            var trade = state.TradeBuilder.CompileTrade();

            Assert.Equal(35, state.StopEntryTarget.EntryPrice);
            Assert.Equal(1-(42/ 35.0) , trade.FinalResult);
            Assert.Equal(1 - (42 / 35.0), trade.FinalDrawdown);
            Assert.Equal(35 * 1.5, state.StopEntryTarget.StopPrice);
            Assert.Equal(35 * 0.6, state.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsLong() {
            var longstate = new LongTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 7),
                null, null);

            longstate.UpdateExits( new ExitPrices(0.995, 1.005));
            longstate.Continue(new BidAskData(new DateTime(), 8, 8, 8, 8, 8, 8, 8, 8, 1));
            
            Assert.Equal(7 * 0.995, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(7 * 1.005, longstate.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldChangeStopAndTargetsShort() {
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.NoStopTarget(), 6.5),
                null, null);

            shortstate.UpdateExits(new ExitPrices(1.005, 0.995));
            shortstate.Continue(new BidAskData(new DateTime(), 8, 8, 8, 8, 8, 8, 8, 8, 1));

            Assert.Equal(6.5 * 1.005, shortstate.StopEntryTarget.StopPrice);
            Assert.Equal(6.5 * 0.995, shortstate.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldExitShortStopHit() {
            var myTrade = new List<Trade>();
            var shortstate = new ShortTradeGenerator(0,
                new TradePrices(ExitPrices.StopOnly(1.2), 10),
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

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
                (x, y) => myTrade.Add(y), null);

            shortstate.Continue(new BidAskData(new DateTime(), 9, 9, 9, 9, 9, 9, 9, 9, 1));
            shortstate.Continue(new BidAskData(new DateTime(), 20, 20, 15, 15, 9, 9, 9, 9, 1));

            Assert.Equal(1, myTrade.Last().FinalResult);
            Assert.Equal(2, myTrade.Last().Duration);
        }
    }
}
