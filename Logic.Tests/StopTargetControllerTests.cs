using System;
using System.Collections.Generic;
using DataStructures;
using Xunit;

namespace Logic.Tests
{
    public class PriceExitCalculatorTests
    {
        [Fact]
        private void ShouldSetExits() {
            var Exits = new ExitPrices(0.7,1.3);
            StaticStopTarget testObject = new StaticStopTarget(Exits);

            Assert.Equal(Exits.StopPercentage , testObject.NewExit(new DatedResult(), Exits, new BidAskData[0],0 ).StopPercentage);
            Assert.Equal(Exits.TargetPercentage, testObject.NewExit(new DatedResult(), Exits, new BidAskData[0] ,0).TargetPercentage);
        }

        [Fact]
        private void ShouldIncrementStopLong() {
            BidAskData[] bars = new BidAskData[] {
            new BidAskData(new DateTime(2020, 01, 01), 100, 100, 100, 100, 100, 100, 100, 100, 1),
            new BidAskData(new DateTime(2020, 01, 02), 102, 102, 102, 102, 102, 102, 102, 102, 1),
            new BidAskData(new DateTime(2020, 01, 03), 103, 103, 103, 103, 103, 103, 103, 103, 1),
            new BidAskData(new DateTime(2020, 01, 04), 104, 104, 104, 104, 104, 104, 104, 104, 1)
            };

            var stop = new TrailingStopPercentage(ExitPrices.StopOnly(0.98), 0.02);
            var longstate = new LongTradeGenerator(0, new TradePrices(stop.InitialExit, 100), null, null);

            longstate.Continue(bars[0]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.Status, longstate.StopEntryTarget.CurrentExits, bars, 0));
                
            Assert.Equal(98, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[1]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.Status, longstate.StopEntryTarget.CurrentExits, bars, 1));

            Assert.Equal(100, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);
            
            longstate.Continue(bars[2]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.Status, longstate.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(101, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[3]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.Status, longstate.StopEntryTarget.CurrentExits, bars, 3));

            Assert.Equal(102, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[2]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.Status, longstate.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(102, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);
        }

        [Fact]
        private void ShouldIncrementStopShort() {
            BidAskData[] bars = new BidAskData[] {
            new BidAskData(new DateTime(2020, 01, 01), 100, 100, 100, 100, 100, 100, 100, 100, 1),
            new BidAskData(new DateTime(2020, 01, 02), 98, 98, 98, 98, 98, 98, 98, 98, 1),
            new BidAskData(new DateTime(2020, 01, 03), 97, 97, 97, 97, 97, 97, 97, 97, 1),
            new BidAskData(new DateTime(2020, 01, 04), 96, 96, 96, 96, 96, 96, 96, 96, 1)
            };

            var stop = new TrailingStopPercentage(ExitPrices.StopOnly(1.02), 0.02);
            var shortState = new ShortTradeGenerator(0, new TradePrices(stop.InitialExit, 100), null, null);

            shortState.Continue(bars[0]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 0));

            Assert.Equal(102, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[1]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 1));

            Assert.Equal(100, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[2]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(99, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[3]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 3));

            Assert.Equal(98, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[2]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(98, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

        }


        [Fact]
        private void ShouldVariableIncreaseStopLong() {
            BidAskData[] bars = new BidAskData[11];
            for (int i = 0; i < bars.Length; i++) 
                bars[i] = new BidAskData(100+i);

            var stop = new VariableTrailingStopPercentage(new ExitPrices(0.95,1.1), 0.05);
            var longState = new LongTradeGenerator(0, new TradePrices(stop.InitialExit, 100), null, null);

            longState.Continue(bars[5]);
            longState.UpdateExits(stop.NewExit(longState.TradeBuilder.Status, longState.StopEntryTarget.CurrentExits, bars, 0));

            Assert.Equal(102.5, longState.StopEntryTarget.StopPrice,6);
            Assert.Equal(110, longState.StopEntryTarget.TargetPrice,6);

            longState.Continue(bars[6]);
            longState.UpdateExits(stop.NewExit(longState.TradeBuilder.Status, longState.StopEntryTarget.CurrentExits, bars, 1));

            Assert.Equal(104, longState.StopEntryTarget.StopPrice,6);
            Assert.Equal(110, longState.StopEntryTarget.TargetPrice,6);

            longState.Continue(bars[7]);
            longState.UpdateExits(stop.NewExit(longState.TradeBuilder.Status, longState.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(105.5, longState.StopEntryTarget.StopPrice,6);
            Assert.Equal(110, longState.StopEntryTarget.TargetPrice,6);

            longState.Continue(bars[1]);
            longState.UpdateExits(stop.NewExit(longState.TradeBuilder.Status, longState.StopEntryTarget.CurrentExits, bars, 3));

            Assert.Equal(105.5, longState.StopEntryTarget.StopPrice,6);
            Assert.Equal(110, longState.StopEntryTarget.TargetPrice,6);
            Assert.False(longState.isActive);

        }

        [Fact]
        private void ShouldVariableIncreaseStopShort() {
            BidAskData[] bars = new BidAskData[11];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = new BidAskData(100 - i);

            var stop = new VariableTrailingStopPercentage(new ExitPrices(1.1, 0.9), 0.05);
            var shortState = new ShortTradeGenerator(0, new TradePrices(stop.InitialExit, 100), null, null);

            shortState.Continue(bars[5]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 0));

            Assert.Equal(97.5, shortState.StopEntryTarget.StopPrice, 6);
            Assert.Equal(90, shortState.StopEntryTarget.TargetPrice, 6);

            shortState.Continue(bars[6]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 1));

            Assert.Equal(96, shortState.StopEntryTarget.StopPrice, 6);
            Assert.Equal(90, shortState.StopEntryTarget.TargetPrice, 6);

            shortState.Continue(bars[7]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 2));

            Assert.Equal(94.5, shortState.StopEntryTarget.StopPrice, 6);
            Assert.Equal(90, shortState.StopEntryTarget.TargetPrice, 6);

            shortState.Continue(bars[1]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.Status, shortState.StopEntryTarget.CurrentExits, bars, 3));

            Assert.Equal(94.5, shortState.StopEntryTarget.StopPrice, 6);
            Assert.Equal(90, shortState.StopEntryTarget.TargetPrice, 6);
            Assert.False(shortState.isActive);

        }

        [Fact]
        private void ShouldExitAfterThreeBarsLong() {
            var Trade = new List<Trade>();
            BidAskData[] bars = new BidAskData[12];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = new BidAskData(100 + i);

            var stop = new TimedExit(ExitPrices.NoStopTarget(), MarketSide.Bull, 3);
            var longTradeGenerator = new LongTradeGenerator(0, new TradePrices(stop.InitialExit, 100), (x,y)=>Trade.Add(y), null);

            for (int i = 0; i < 3; i++) {
                longTradeGenerator.Continue(bars[i]);
                longTradeGenerator.UpdateExits(stop.NewExit(longTradeGenerator.TradeBuilder.Status, longTradeGenerator.StopEntryTarget.CurrentExits, bars, i));
                Assert.True(longTradeGenerator.isActive);
            }

            longTradeGenerator.Continue(bars[3]);
            longTradeGenerator.UpdateExits(stop.NewExit(longTradeGenerator.TradeBuilder.Status, longTradeGenerator.StopEntryTarget.CurrentExits, bars, 3));

            Assert.False(longTradeGenerator.isActive);
            Assert.Equal(4, Trade[0].Duration);

        }

        [Fact]
        private void ShouldExitAfterTenBarsLong() {
            var Trade = new List<Trade>();
            BidAskData[] bars = new BidAskData[14];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = new BidAskData(100 + i);

            var stop = new TimedExit(ExitPrices.NoStopTarget(), MarketSide.Bull, 12);
            var longTradeGenerator = new LongTradeGenerator(0, new TradePrices(stop.InitialExit, 100), (x, y) => Trade.Add(y), null);

            for (int i = 0; i < 12; i++) {
                longTradeGenerator.Continue(bars[i]);
                longTradeGenerator.UpdateExits(stop.NewExit(longTradeGenerator.TradeBuilder.Status, longTradeGenerator.StopEntryTarget.CurrentExits, bars, i));
                Assert.True(longTradeGenerator.isActive);
            }

            longTradeGenerator.Continue(bars[12]);
            longTradeGenerator.UpdateExits(stop.NewExit(longTradeGenerator.TradeBuilder.Status, longTradeGenerator.StopEntryTarget.CurrentExits, bars, 12));

            Assert.False(longTradeGenerator.isActive);
            Assert.Equal(13, Trade[0].Duration);


        }

        [Fact]
        private void ShouldExitAfterThreeBarsShort() {
            var Trade = new List<Trade>();
            BidAskData[] bars = new BidAskData[12];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = new BidAskData(100 + i);

            var stop = new TimedExit(ExitPrices.NoStopTarget(), MarketSide.Bear, 3);
            var shortGen = new ShortTradeGenerator(0, new TradePrices(stop.InitialExit, 100), (x, y) => Trade.Add(y), null);

            for (int i = 0; i < 3; i++) {
                shortGen.Continue(bars[i]);
                shortGen.UpdateExits(stop.NewExit(shortGen.TradeBuilder.Status, shortGen.StopEntryTarget.CurrentExits, bars, i));
                Assert.True(shortGen.isActive);
            }

            shortGen.Continue(bars[3]);
            shortGen.UpdateExits(stop.NewExit(shortGen.TradeBuilder.Status, shortGen.StopEntryTarget.CurrentExits, bars, 3));

            Assert.False(shortGen.isActive);
            Assert.Equal(4, Trade[0].Duration);
        }

        [Fact]
        private void ShouldExitAfterTenBarsShort() {
            var Trade = new List<Trade>();
            BidAskData[] bars = new BidAskData[14];
            for (int i = 0; i < bars.Length; i++)
                bars[i] = new BidAskData(100 + i);

            var stop = new TimedExit(ExitPrices.NoStopTarget(), MarketSide.Bear, 12);
            var shortGen = new ShortTradeGenerator(0, new TradePrices(stop.InitialExit, 100), (x, y) => Trade.Add(y), null);

            for (int i = 0; i < 12; i++) {
                shortGen.Continue(bars[i]);
                shortGen.UpdateExits(stop.NewExit(shortGen.TradeBuilder.Status, shortGen.StopEntryTarget.CurrentExits, bars, i));
                Assert.True(shortGen.isActive);
            }

            shortGen.Continue(bars[12]);
            shortGen.UpdateExits(stop.NewExit(shortGen.TradeBuilder.Status, shortGen.StopEntryTarget.CurrentExits, bars, 12));

            Assert.False(shortGen.isActive);
            Assert.Equal(13, Trade[0].Duration);

        }
    }



}
