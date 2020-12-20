using DataStructures;
using System;
using System.Collections.Generic;
using Logic;


namespace Thought
{
    //Add inheristed class that notifies when trade added in order to execute any strategy or portfolio adjustments
    public abstract class StrategyExecuter
    {
        private List<Trade> _trades { get; set; }
        protected List<TradeGeneratorInterface> _generators { get; set; }
        private Strategiser _strat { get; set; }
        private bool _canEnterWhenExposed { get; }

        protected StrategyExecuter(bool enterWhenEntered) {
            _canEnterWhenExposed = enterWhenEntered;

        }

        public List<Trade> Execute(TradingField greens) {
            Init(greens.Strategy);
            iterateTime(greens.MarketData.PriceData);
            return _trades;
        }

        private void Init(Strategiser strat) {
            _strat = strat;
            _trades = new List<Trade>();
            _generators = new List<TradeGeneratorInterface>();
        }

        private void iterateTime(BidAskData[] prices) {
            for (int i = 1; i < prices.Length; i++) {
                destroyInactiveTraders();
                progressTrades(prices[i]);
                lookForEntry(prices, i);
                LookForExits(prices, i);
            }
        }

        private void LookForExits(BidAskData[] prices, int i) {
            if (_strat.IsExit(prices[i],i))
                exitActions(prices[i]);
        }

        private void lookForEntry(BidAskData[] prices, int i) {
            if (_strat.IsEntry(prices[i],i))
                entryActions(prices[i], i);
        }

        private void progressTrades(BidAskData prices) {
            _generators.ForEach(x => x.Continue(prices));
        }

        private void destroyInactiveTraders() {
            for (int j = _generators.Count - 1; j >= 0; j--) {
                if (!_generators[j].isActive)
                    _generators.RemoveAt(j);
            }
        }

        private void entryActions(BidAskData prices, int i) {
            if (_generators.Count > 0 && !_canEnterWhenExposed)
                return;
            else
                _generators.Add(buildGenerator(
                    new TradePrices(_strat.AdjustPrices(prices,i,0), getEntry(prices)), i));
        }

        protected abstract void exitActions(BidAskData prices);
        protected abstract double getEntry(BidAskData prices);
        protected abstract TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index);

        protected void addTrade(Trade trade) {
            _trades.Add(trade);
        }

    }

    public class LongStrategyExecuter : StrategyExecuter
    {
        public LongStrategyExecuter(bool enterWhenEntered) 
            : base(enterWhenEntered) {
        }

        protected override void exitActions(BidAskData prices) {
            foreach (var gen in _generators)
                gen.Exit(prices.Open.Ticks, prices.Open.Bid);
        }

        protected override double getEntry(BidAskData prices) {
            return prices.Open.Ask;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return new LongTradeGenerator(index, tradeInit, addTrade);
        }
    }

    public class ShortStrategyExecuter : StrategyExecuter
    {
        public ShortStrategyExecuter(bool enterWhenEntered)
            : base(enterWhenEntered) {
        }

        protected override void exitActions(BidAskData prices) {
            foreach (var gen in _generators)
                gen.Exit(prices.Open.Ticks, prices.Open.Ask);
        }

        protected override double getEntry(BidAskData prices) {
                return prices.Open.Bid;
        }

        protected override TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return new ShortTradeGenerator(index, tradeInit, addTrade);
        }
    }
}
