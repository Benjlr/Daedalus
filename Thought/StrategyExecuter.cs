using DataStructures;
using System;
using System.Collections.Generic;


namespace Thought
{
    public class StrategyExecuter
    {
        private readonly MarketSide _side;
        private readonly List<Trade> _trades;
        private readonly List<TradeGeneratorInterface> _generators;
        private readonly bool _canEnterWhenExposed;

        public StrategyExecuter(MarketSide side, bool entersAllEntries) {
            _side = side;
            _canEnterWhenExposed = entersAllEntries;
            _trades = new List<Trade>();
            _generators = new List<TradeGeneratorInterface>();
        }

        public List<Trade> Execute(UniverseObject uObject) {
            iterateTime(uObject.Strategy.Entries, uObject.MarketData.RawData);
            return _trades;
        }

        private void iterateTime(bool[] entries, BidAskData[] prices) {
            for (int i = 1; i < entries.Length; i++) {
                destroyInactiveTraders();
                progressTrades(prices, i);
                lookForEntry(entries, prices, i);
            }
        }

        private void lookForEntry(bool[] entries, BidAskData[] prices, int i) {
            if (entries[i - 1])
                entryActions(_generators, prices, i);
        }

        private void progressTrades(BidAskData[] prices, int i) {
            _generators.ForEach(x => x.Continue(prices[i]));
        }

        private void destroyInactiveTraders() {
            for (int j = _generators.Count - 1; j >= 0; j--) {
                if (!_generators[j].isActive)
                    _generators.RemoveAt(j);
            }
        }

        private void entryActions(List<TradeGeneratorInterface> generators, BidAskData[] prices, int i) {
            if (generators.Count > 0 && !_canEnterWhenExposed)
                return;
            else
                generators.Add(buildGenerator(
                    new TradePrices(new ExitPrices(0.8, 1.2), getEntry(prices, i)), i));
        }

        private double getEntry(BidAskData[] prices, int i) {
            return _side switch
            {
                MarketSide.Bull => prices[i].Open_Bid,
                _ => prices[i].Open_Ask
            };
        }

        private TradeGeneratorInterface buildGenerator(TradePrices tradeInit, int index) {
            return _side switch
            {
                MarketSide.Bear => new ShortTradeGenerator(index, tradeInit, new Action<Trade>((x) => addTrade(_trades, x))),
                MarketSide.Bull => new LongTradeGenerator(index, tradeInit, new Action<Trade>((x) => addTrade(_trades, x))),
                _ => null,
            };
        }

        private void addTrade(List<Trade> list, Trade trade) {
            list.Add(trade);
        }

    }
}
