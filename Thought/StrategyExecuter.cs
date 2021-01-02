using System;
using DataStructures;
using Logic;
using System.Collections.Generic;


namespace Thought
{
    public abstract class StrategyExecuter
    {
        private Action<Guid, Trade> _tradeGenerated { get; set; }
        private Action<Guid, DatedResult> _cyclePassed { get; set; }
        protected List<TradeGeneratorInterface> _generators { get; set; }
        protected BidAskData[] _prices { get; set; }
        protected Strategiser _strat { get; set; }
        protected bool _canEnterWhenExposed { get; }
        protected int _counter { get; set; } 

        protected StrategyExecuter(bool enterWhenEntered, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) {
            _canEnterWhenExposed = enterWhenEntered;
            _tradeGenerated = onExit;
            _cyclePassed = onContinue;
        }

        public void ExecuteAll() {
            for (_counter = 1; _counter < _prices.Length; _counter++)
                IterationActions();
        }
        public bool ExecuteStep() {
            if(_counter >= _prices.Length)
                return true;

            IterationActions();
            _counter++;
            return false;
        }

        public void Init(TradingField field) {
            _strat = field.Strategy;
            _prices = field.MarketData.PriceData;
            _generators = new List<TradeGeneratorInterface>();
            _counter = 1;
        }
        
        private void IterationActions() {
            destroyInactiveTraders();
            progressTrades();
            lookForEntry();
            lookForExits();
        }

        private void lookForExits() {
            if (_strat.IsExit(_prices[_counter], _counter))
                exitActions();
        }

        private void lookForEntry() {
            if (_strat.IsEntry(_prices[_counter], _counter))
                entryActions();
        }

        private void progressTrades() {
            _generators.ForEach(x => {
                x.Continue(_prices[_counter]);
                x.UpdateExits(_strat.Stops.NewExit(x.TradeBuilder.Status, x.StopEntryTarget.CurrentExits, _prices, _counter));
            });
        }

        private void destroyInactiveTraders() {
            for (int j = _generators.Count - 1; j >= 0; j--) {
                if (!_generators[j].isActive)
                    _generators.RemoveAt(j);
            }
        }

        private void entryActions() {
            if (_generators.Count > 0 && !_canEnterWhenExposed)
                return;
            else
                _generators.Add(buildGenerator(
                    new TradePrices(_strat.Stops.InitialExit, getEntry()), _counter, _tradeGenerated, _cyclePassed));
        }

        protected abstract void exitActions();
        protected abstract double getEntry();
        protected abstract TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue);

    }

    public class LongStrategyExecuter : StrategyExecuter
    {
        public LongStrategyExecuter( bool enterWhenEntered, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) 
            : base(enterWhenEntered, onExit, onContinue) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices[_counter].Open.Ticks, _prices[_counter].Open.Bid);
        }

        protected override double getEntry() {
            return _prices[_counter].Open.Ask;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid,Trade> onExit, Action<Guid, DatedResult> onContinue) {
            return new LongTradeGenerator(index, tradeInit, onExit, onContinue);
        }
    }

    public class ShortStrategyExecuter : StrategyExecuter
    {
        public ShortStrategyExecuter( bool enterWhenEntered, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue)
            : base( enterWhenEntered, onExit, onContinue) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices[_counter].Open.Ticks, _prices[_counter].Open.Ask);
        }

        protected override double getEntry() {
                return _prices[_counter].Open.Bid;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) {
            return new ShortTradeGenerator(index, tradeInit, onExit, onContinue);
        }
    }


}
