using DataStructures;
using System;
using System.Collections.Generic;
using Logic;


namespace Thought
{
    //Add inheristed class that notifies when trade added in order to execute any strategy or portfolio adjustments
    public class StrategyExecuter
    {
        private MarketSide _side { get; }
        private List<Trade> _trades { get; }
        private List<TradeGeneratorInterface> _generators { get; }
        private bool _canEnterWhenExposed { get; }
        private Func<ExitPrices> _exitPrices { get; }

        public StrategyExecuter(MarketSide side, bool entersAllEntries, Func<ExitPrices> myExits) {
            _side = side;
            _canEnterWhenExposed = entersAllEntries;
            _exitPrices = myExits;
            _trades = new List<Trade>();
            _generators = new List<TradeGeneratorInterface>();
        }

        public List<Trade> Execute(UniverseObject uObject) {
            iterateTime(uObject.Strategy, uObject.MarketData.RawData);
            return _trades;
        }

        private void iterateTime(Strategy strat, BidAskData[] prices) {
            for (int i = 1; i < prices.Length; i++) {
                destroyInactiveTraders();
                progressTrades(prices[i]);
                lookForEntry(strat.Entries[i], prices, i);
                LookForExits(strat.Exits[i], prices[i]);
            }
        }

        private void LookForExits(bool exit, BidAskData prices) {
            if (exit)
                exitActions(prices);
        }

        private void lookForEntry(bool entries, BidAskData[] prices, int i) {
            if (entries)
                entryActions(prices, i);
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

        private void entryActions(BidAskData[] prices, int i) {
            if (_generators.Count > 0 && !_canEnterWhenExposed)
                return;
            else
                _generators.Add(buildGenerator(
                    new TradePrices(_exitPrices.Invoke(), getEntry(prices[i])), i));
        }

        private void exitActions(BidAskData prices) {
            foreach (var gen in _generators)
                switch (_side) {
                    case MarketSide.Bull: gen.Exit(prices.Open.Ticks, prices.Open.Bid);
                        break;
                    case MarketSide.Bear: gen.Exit(prices.Open.Ticks, prices.Open.Ask);
                        break;
                }
        }

        private double getEntry(BidAskData prices) {
            return _side switch
            {
                MarketSide.Bull => prices.Open.Bid,
                MarketSide.Bear => prices.Open.Ask,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return _side switch
            {
                MarketSide.Bear => new ShortTradeGenerator(index, tradeInit, addTrade),
                MarketSide.Bull => new LongTradeGenerator(index, tradeInit, addTrade),
                _ => null,
            };
        }

        private void addTrade(Trade trade) {
            _trades.Add(trade);
        }

    }
}
