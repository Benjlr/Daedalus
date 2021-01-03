using System;
using DataStructures;
using Logic;
using System.Collections.Generic;


namespace Thought
{
    public abstract class StrategyExecuter
    {
        private Func<string, bool> _portfolioAllowsEntries { get; set; }
        private Action<Guid, Trade> _tradeGenerated { get; set; }
        private Action<Guid, DatedResult> _cyclePassed { get; set; }
        protected List<TradeGeneratorInterface> _generators { get; set; }
        protected Market _prices { get; set; }
        protected Strategiser _strat { get; set; }
        protected int _counter { get; set; } 


        protected StrategyExecuter(Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue, Func<string, bool> portfolioCheck ) {
            _tradeGenerated = onExit;
            _cyclePassed = onContinue;
            _portfolioAllowsEntries = portfolioCheck;
        }

        public void ExecuteAll() {
            for (_counter = 1; _counter < _prices.PriceData.Length; _counter++)
                IterationActions();
        }
        public bool ExecuteStep() {
            if(_counter >= _prices.PriceData.Length)
                return true;

            IterationActions();
            _counter++;
            return false;
        }

        public void Init(TradingField field) {
            _strat = field.Strategy;
            _prices = field.MarketData;
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
            if (_strat.IsExit(_prices.PriceData[_counter], _counter) ||
                _counter == _prices.PriceData.Length -1)
                exitActions();
            
        }

        private void lookForEntry() {
            if (_strat.IsEntry(_prices.PriceData[_counter], _counter) && (_portfolioAllowsEntries?.Invoke(_prices.Id) ?? true))
                entryActions();
        }

        private void progressTrades() {
            _generators.ForEach(x => {
                x.Continue(_prices.PriceData[_counter]);
                x.UpdateExits(_strat.Stops.NewExit(x.TradeBuilder.Status, x.StopEntryTarget.CurrentExits, _prices.PriceData, _counter, x.TradeBuilder.Count));
            });
        }

        private void destroyInactiveTraders() {
            for (int j = _generators.Count - 1; j >= 0; j--) {
                if (!_generators[j].isActive)
                    _generators.RemoveAt(j);
            }
        }

        private void entryActions() {
            _generators.Add(buildGenerator(
                    new TradePrices(_strat.Stops.InitialExit, getEntry()), _counter, _tradeGenerated, _cyclePassed));
        }

        protected abstract void exitActions();
        protected abstract double getEntry();
        protected abstract TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue);

    }

    public class LongStrategyExecuter : StrategyExecuter
    {
        public LongStrategyExecuter(Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue, Func<string, bool> portfolioCheck) 
            : base(onExit, onContinue, portfolioCheck) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices.PriceData[_counter].Open.Ticks, _prices.PriceData[_counter].Open.Bid);
        }

        protected override double getEntry() {
            return _prices.PriceData[_counter].Open.Ask;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid,Trade> onExit, Action<Guid, DatedResult> onContinue) {
            return new LongTradeGenerator(index, tradeInit, onExit, onContinue);
        }
    }

    public class ShortStrategyExecuter : StrategyExecuter
    {
        public ShortStrategyExecuter(  Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue, Func<string, bool> portfolioCheck)
            : base(  onExit, onContinue, portfolioCheck) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices.PriceData[_counter].Open.Ticks, _prices.PriceData[_counter].Open.Ask);
        }

        protected override double getEntry() {
                return _prices.PriceData[_counter].Open.Bid;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index, Action<Guid, Trade> onExit, Action<Guid, DatedResult> onContinue) {
            return new ShortTradeGenerator(index, tradeInit, onExit, onContinue);
        }
    }


}
