using DataStructures;
using Logic;
using System.Collections.Generic;


namespace Thought
{
    public abstract class StrategyExecuter
    {
        protected List<Trade> _trades { get; set; }
        protected BidAskData[] _prices { get; set; }
        protected List<TradeGeneratorInterface> _generators { get; set; }
        protected Strategiser _strat { get; set; }
        protected bool _canEnterWhenExposed { get; }
        protected int _counter { get; set; } 

        protected StrategyExecuter(bool enterWhenEntered) {
            _canEnterWhenExposed = enterWhenEntered;
        }

        public List<Trade> ExecuteAll() {
            for (_counter = 1; _counter < _prices.Length; _counter++)
                IterationActions();
            return _trades;
        }
        public List<Trade> ExecuteStep() {
            if(_counter >= _prices.Length)
                return _trades;

            IterationActions();
            _counter++;
            return new List<Trade>();
        }

        public void Init(TradingField field) {
            _strat = field.Strategy;
            _prices = field.MarketData.PriceData;
            _trades = new List<Trade>();
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
                x.UpdateExits(_strat.Stops.NewExit(x.TradeBuilder.ResultTimeline[^1], x.StopEntryTarget.CurrentExits, _prices, _counter));
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
                    new TradePrices(_strat.Stops.InitialExit, getEntry()), _counter));
        }

        protected abstract void exitActions();
        protected abstract double getEntry();
        protected abstract TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index);

        protected void addTrade(Trade trade) {
            _trades.Add(trade);
        }

    }

    public class LongStrategyExecuter : StrategyExecuter
    {
        public LongStrategyExecuter( bool enterWhenEntered) 
            : base( enterWhenEntered) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices[_counter].Open.Ticks, _prices[_counter].Open.Bid);
        }

        protected override double getEntry() {
            return _prices[_counter].Open.Ask;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return new LongTradeGenerator(index, tradeInit, addTrade);
        }
    }

    public class ShortStrategyExecuter : StrategyExecuter
    {
        public ShortStrategyExecuter( bool enterWhenEntered)
            : base( enterWhenEntered) {
        }

        protected override void exitActions() {
            foreach (var gen in _generators)
                gen.Exit(_prices[_counter].Open.Ticks, _prices[_counter].Open.Ask);
        }

        protected override double getEntry() {
                return _prices[_counter].Open.Bid;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return new ShortTradeGenerator(index, tradeInit, addTrade);
        }
    }


}
