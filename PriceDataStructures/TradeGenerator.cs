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
        public void Exit(long date, double exitPrice);

    }

    public abstract class TradeStateGenerator : TradeGeneratorInterface
    {
        private Action<Trade> onExit { get; }
        protected ArrayBuilder _tradeBuilder { get; set; }
        public TradePrices TradeLimits { get; private set; }
        public double CurrentReturn { get; protected set; }
        public double Drawdown { get; protected set; }
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

        public void Exit(long date, double exitPrice) {
            GetReturnAndDrawdown(exitPrice, exitPrice);
            _tradeBuilder.AddResult(date, CurrentReturn, Drawdown);
            onExit?.Invoke(_tradeBuilder.CompileTrade());
            isActive = false;
        }
        
        protected void AddTradeBuilderStats(long date, double data, double low) {
            GetReturnAndDrawdown(data, low);
            this._tradeBuilder.AddResult(date, CurrentReturn, Drawdown);
        }

        private void GetReturnAndDrawdown(double data, double low) {
            CurrentReturn = CalculateReturn(data);
            var dd = CalculateReturn(low);
            if (Drawdown > dd)
                Drawdown = dd;
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
                AddTradeBuilderStats(data.Close.Time.Ticks, data.Close.Bid, data.Low.Bid);
        }

        private bool CheckTargets(BidAskData data) {
            if (data.High.Bid > TradeLimits.TargetPrice) {
                if (data.Open.Bid > TradeLimits.TargetPrice)
                    Exit(data.Open.Time.Ticks, data.Open.Bid);
                else Exit(data.High.Time.Ticks, TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }

        private bool CheckStops(BidAskData data) {
            if (data.Low.Bid < TradeLimits.StopPrice) {
                if (data.Open.Bid < TradeLimits.StopPrice)
                    Exit(data.Open.Time.Ticks, data.Open.Bid);
                else Exit(data.Low.Time.Ticks, TradeLimits.StopPrice);
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
                AddTradeBuilderStats(data.Close.Time.Ticks, data.Close.Ask, data.High.Ask);
        }

        private bool CheckStops(BidAskData data) {
            if (data.High.Ask > TradeLimits.StopPrice) {
                if (data.Open.Ask > TradeLimits.StopPrice)
                    Exit(data.Open.Time.Ticks, data.Open.Ask);
                else Exit(data.High.Time.Ticks, TradeLimits.StopPrice);
                return true;
            }

            return false;
        }

        private bool CheckTargets(BidAskData data) {
            if (data.Low.Ask < TradeLimits.TargetPrice) {
                if (data.Open.Ask < TradeLimits.TargetPrice)
                    Exit(data.Open.Time.Ticks, data.Open.Ask);
                else Exit(data.Low.Time.Ticks, TradeLimits.TargetPrice);
                return true;
            }

            return false;
        }

        protected override double CalculateReturn(double data) {
            return 1- ( data / this.TradeLimits.EntryPrice) ;
        }
    }
}

