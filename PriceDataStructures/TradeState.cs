

using System;
using System.Collections.Generic;

namespace DataStructures
{
    public readonly struct TradePrices
    {
        public double EntryPrice { get; }
        public double StopPrice { get; }
        public double TargetPrice { get; }

        public TradePrices(ExitPrices exits, double entryPrice) {
            EntryPrice = entryPrice;
            StopPrice = entryPrice * exits.StopPercentage;
            TargetPrice = entryPrice * exits.TargetPercentage;
        }
    }

    public interface TradeGeneratorInterface
    {
        public TradePrices TradeLimits { get; }
        public double CurrentReturn { get; }
        public void ContinueUpdateExits(BidAskData data, ExitPrices exitPrices);
        public void Continue(BidAskData data);
        public void Exit(double exitPrice);

    }

    public abstract class TradeStateGenerator : TradeGeneratorInterface
    {
        private Action<Trade> onExit { get; }
        protected ArrayBuilder _tradeBuilder { get; set; }
        public TradePrices TradeLimits { get; private set; }
        public double CurrentReturn { get; protected set; }


        protected TradeStateGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) {
            _tradeBuilder = new ArrayBuilder();
            _tradeBuilder.Init(marketIndex);
            TradeLimits = tradeInit;
            _tradeBuilder.AddResult(CurrentReturn);
            onExit = exiting;
        }

        public static TradeGeneratorInterface Invest(MarketSide longShort, TradePrices tradeInit, Action<Trade> exiting, int marketIndex) {
            if (longShort.Equals(MarketSide.Bull)) return new LongTradeGenerator(marketIndex, tradeInit, exiting);
            else return new ShortTradeGenerator(marketIndex, tradeInit, exiting);
        }

        public void Continue(BidAskData data) {
            CheckStopsAndTargets(data);
        }

        public void ContinueUpdateExits(BidAskData data, ExitPrices exitPrices) {
            TradeLimits = new TradePrices(exitPrices, TradeLimits.EntryPrice);
            CheckStopsAndTargets(data);
        }

        public void Exit(double exitPrice) {
            _tradeBuilder.AddResult(calculateReturn(exitPrice));
            onExit?.Invoke(_tradeBuilder.CompileTrade());
        }
        
        protected void AddTradeBuilderStats(double data) {
            CurrentReturn = calculateReturn(data);
            this._tradeBuilder.AddResult(CurrentReturn);
        }

        protected abstract void CheckStopsAndTargets(BidAskData data);

        private double calculateReturn(double data) {
            return (data / this.TradeLimits.EntryPrice) - 1;
        }
    }

    public class LongTradeGenerator : TradeStateGenerator
    {
        public LongTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) : base(marketIndex, tradeInit, exiting) {
        }


        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Close_Bid);
        }

        private bool CheckTargets(BidAskData data) {
            if (data.High_Bid > TradeLimits.TargetPrice) {
                if (data.Open_Bid > TradeLimits.TargetPrice)
                    Exit(data.Open_Bid);
                else Exit(TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }

        private bool CheckStops(BidAskData data) {
            if (data.Low_Bid < TradeLimits.StopPrice) {
                if (data.Open_Bid < TradeLimits.StopPrice)
                    Exit(data.Open_Bid);
                else Exit(TradeLimits.StopPrice);
                return true;
            }

            return false;
        }
    }

    public class ShortTradeGenerator : TradeStateGenerator
    {
        public ShortTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) : base(marketIndex, tradeInit, exiting) {
        }

        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Close_Ask);
        }

        private bool CheckStops(BidAskData data) {
            if (data.High_Ask > TradeLimits.StopPrice) {
                if (data.Open_Ask > TradeLimits.StopPrice)
                    Exit(data.Open_Ask);
                else Exit(TradeLimits.StopPrice);
                return true;
            }

            return false;
        }

        private bool CheckTargets(BidAskData data) {
            if (data.Low_Ask < TradeLimits.TargetPrice) {
                if (data.Open_Ask < TradeLimits.TargetPrice)
                    Exit(data.Open_Ask);
                else Exit(TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }
    }
}

