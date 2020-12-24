using System;
using RuleSets;
using Xunit;



namespace DataStructures.Tests
{
    public class PriceExitCalculatorTests
    {
        [Fact]
        private void ShouldSetExits() {
            var Exits = new ExitPrices(0.7,1.3);
            StaticStopTarget testObject = new StaticStopTarget(Exits);

            Assert.Equal(Exits.StopPercentage , testObject.NewExit(new Trade(), new BidAskData[0],0 ).StopPercentage);
            Assert.Equal(Exits.TargetPercentage, testObject.NewExit(new Trade(), new BidAskData[0] ,0).TargetPercentage);
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
            var longstate = new LongTradeGenerator(0, new TradePrices(stop.InitialExit, 100), null);

            longstate.Continue(bars[0]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.CompileTrade(), bars, 0));
                
            Assert.Equal(98, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[1]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.CompileTrade(), bars, 1));

            Assert.Equal(100, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);
            
            longstate.Continue(bars[2]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.CompileTrade(), bars, 2));

            Assert.Equal(101, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[3]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.CompileTrade(), bars, 3));

            Assert.Equal(102, longstate.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, longstate.StopEntryTarget.TargetPrice);

            longstate.Continue(bars[2]);
            longstate.UpdateExits(stop.NewExit(longstate.TradeBuilder.CompileTrade(), bars, 2));

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
            var shortState = new ShortTradeGenerator(0, new TradePrices(ExitPrices.NoStopTarget(), 100), null);

            shortState.Continue(bars[0]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.CompileTrade(), bars, 0));

            Assert.Equal(102, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[1]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.CompileTrade(), bars, 1));

            Assert.Equal(100, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[2]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.CompileTrade(), bars, 2));

            Assert.Equal(99, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[3]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.CompileTrade(), bars, 3));

            Assert.Equal(98, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

            shortState.Continue(bars[2]);
            shortState.UpdateExits(stop.NewExit(shortState.TradeBuilder.CompileTrade(), bars, 2));

            Assert.Equal(98, shortState.StopEntryTarget.StopPrice);
            Assert.Equal(double.NaN, shortState.StopEntryTarget.TargetPrice);

        }
    }



}
