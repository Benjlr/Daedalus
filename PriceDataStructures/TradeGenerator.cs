using System;

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
        public bool isActive { get; }
        public void Continue(BidAskData data);
        public void Exit(DateTime date, double exitPrice);

    }

    public abstract class TradeStateGenerator : TradeGeneratorInterface
    {
        private Action<Trade> onExit { get; }
        protected ArrayBuilder _tradeBuilder { get; set; }
        public TradePrices TradeLimits { get; private set; }
        public double CurrentReturn { get; protected set; }
        public bool isActive { get; private set; }

        protected TradeStateGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) {
            _tradeBuilder = new ArrayBuilder();
            _tradeBuilder.Init(marketIndex);
            TradeLimits = tradeInit;
            onExit = exiting;
            isActive = true;
        }


        public void Continue(BidAskData data) {
            CheckStopsAndTargets(data);
        }

        public void UpdateExits(ExitPrices exitPrices) {
            TradeLimits = new TradePrices(exitPrices, TradeLimits.EntryPrice);
        }

        public void Exit(DateTime date, double exitPrice) {
            var final = CalculateReturn(exitPrice);
            _tradeBuilder.AddResult(date, final, final);
            onExit?.Invoke(_tradeBuilder.CompileTrade());
            isActive = false;
        }
        
        protected void AddTradeBuilderStats(DateTime date, double data, double low) {
            CurrentReturn = CalculateReturn(data);
            this._tradeBuilder.AddResult(date, CurrentReturn, CalculateReturn(low));
        }

        protected abstract void CheckStopsAndTargets(BidAskData data);

        protected abstract double CalculateReturn(double data);
    }

    public class LongTradeGenerator : TradeStateGenerator
    {
        public LongTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) : base(marketIndex, tradeInit, exiting) {
        }

        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Time, data.Close_Bid, data.Low_Bid);
        }

        private bool CheckTargets(BidAskData data) {
            if (data.High_Bid > TradeLimits.TargetPrice) {
                if (data.Open_Bid > TradeLimits.TargetPrice)
                    Exit(data.Time, data.Open_Bid);
                else Exit(data.Time, TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }

        private bool CheckStops(BidAskData data) {
            if (data.Low_Bid < TradeLimits.StopPrice) {
                if (data.Open_Bid < TradeLimits.StopPrice)
                    Exit(data.Time, data.Open_Bid);
                else Exit(data.Time, TradeLimits.StopPrice);
                return true;
            }

            return false;
        }

        protected override double CalculateReturn(double data) {
            return (data / this.TradeLimits.EntryPrice) - 1;
        }
    }

    public class ShortTradeGenerator : TradeStateGenerator
    {
        public ShortTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Trade> exiting) : base(marketIndex, tradeInit, exiting) {
        }

        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Time, data.Close_Ask, data.High_Ask);
        }

        private bool CheckStops(BidAskData data) {
            if (data.High_Ask > TradeLimits.StopPrice) {
                if (data.Open_Ask > TradeLimits.StopPrice)
                    Exit(data.Time, data.Open_Ask);
                else Exit(data.Time, TradeLimits.StopPrice);
                return true;
            }

            return false;
        }

        private bool CheckTargets(BidAskData data) {
            if (data.Low_Ask < TradeLimits.TargetPrice) {
                if (data.Open_Ask < TradeLimits.TargetPrice)
                    Exit(data.Time, data.Open_Ask);
                else Exit(data.Time, TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }

        protected override double CalculateReturn(double data) {
            return 1- ( data / this.TradeLimits.EntryPrice) ;
        }
    }
}

