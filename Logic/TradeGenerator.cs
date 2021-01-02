using System;
using DataStructures;

namespace Logic
{
    public interface TradeGeneratorInterface
    {
        public TradeCompiler TradeBuilder { get; }
        public TradePrices StopEntryTarget { get; }
        public bool isActive { get; }
        public void Continue(BidAskData data);
        public void UpdateExits(ExitPrices exitPrices);
        public void Exit(long date, double exitPrice);
    }

    public abstract class TradeStateGenerator : TradeGeneratorInterface
    {
        private Guid _id { get; set; }
        private Action<Guid, Trade> _onExit { get; }
        private Action<Guid, DatedResult> _onContinue { get; }
        public TradePrices StopEntryTarget { get; private set; }
        public TradeCompiler TradeBuilder { get; private set; }
        public bool isActive { get; private set; }

        protected TradeStateGenerator(int marketIndex, TradePrices tradeInit, Action<Guid,Trade> onExit, Action<Guid,DatedResult> onContinue) {
            TradeBuilder = new TradeCompiler(marketIndex);
            _id = Guid.NewGuid();
            _onExit = onExit;
            _onContinue = onContinue;
            isActive = true;
            StopEntryTarget = tradeInit;
        }

        public void Continue(BidAskData data) {
            CheckStopsAndTargets(data);
            if(isActive)
                _onContinue?.Invoke(_id,TradeBuilder.Status);
        }

        public void UpdateExits(ExitPrices exitPrices) {
            StopEntryTarget = new TradePrices(exitPrices, StopEntryTarget.EntryPrice);
        }

        public void Exit(long date, double exitPrice) {
            AddTradeBuilderStats(date, exitPrice, exitPrice);
            _onExit?.Invoke(_id,TradeBuilder.CompileTrade());
            isActive = false;
        }
        
        protected void AddTradeBuilderStats(long date, double data, double low) {
            this.TradeBuilder.AddResult(date, CalculateReturn(data), CalculateReturn(low));
        }

        protected abstract void CheckStopsAndTargets(BidAskData data);

        protected abstract double CalculateReturn(double data);
    }

    public class LongTradeGenerator : TradeStateGenerator
    {
        public LongTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) : base(marketIndex, tradeInit, onExit, onContinue) {
        }

        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Close.Ticks, data.Close.Bid, data.Low.Bid);
        }

        private bool CheckTargets(BidAskData data) {
            if (data.High.Bid > StopEntryTarget.TargetPrice) {
                if (data.Open.Bid > StopEntryTarget.TargetPrice)
                    Exit(data.Open.Ticks, data.Open.Bid);
                else Exit(data.High.Ticks, StopEntryTarget.TargetPrice);
                return true;
            }

            return false;
        }

        private bool CheckStops(BidAskData data) {
            if (data.Low.Bid < StopEntryTarget.StopPrice) {
                if (data.Open.Bid < StopEntryTarget.StopPrice)
                    Exit(data.Open.Ticks, data.Open.Bid);
                else Exit(data.Low.Ticks, StopEntryTarget.StopPrice);
                return true;
            }

            return false;
        }

        protected override double CalculateReturn(double data) {
            return (data / this.StopEntryTarget.EntryPrice) - 1;
        }
    }

    public class ShortTradeGenerator : TradeStateGenerator
    {
        public ShortTradeGenerator(int marketIndex, TradePrices tradeInit, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) : base(marketIndex, tradeInit, onExit, onContinue) {
        }

        protected override void CheckStopsAndTargets(BidAskData data) {
            if (!CheckStops(data) && !CheckTargets(data))
                AddTradeBuilderStats(data.Close.Ticks, data.Close.Ask, data.High.Ask);
        }

        private bool CheckStops(BidAskData data) {
            if (data.High.Ask > StopEntryTarget.StopPrice) {
                if (data.Open.Ask > StopEntryTarget.StopPrice)
                    Exit(data.Open.Ticks, data.Open.Ask);
                else Exit(data.High.Ticks, StopEntryTarget.StopPrice);
                return true;
            }

            return false;
        }

        private bool CheckTargets(BidAskData data) {
            if (data.Low.Ask < StopEntryTarget.TargetPrice) {
                if (data.Open.Ask < StopEntryTarget.TargetPrice)
                    Exit(data.Open.Ticks, data.Open.Ask);
                else Exit(data.Low.Ticks, StopEntryTarget.TargetPrice);
                return true;
            }

            return false;
        }

        protected override double CalculateReturn(double data) {
            return 1- ( data / this.StopEntryTarget.EntryPrice) ;
        }
    }


}

