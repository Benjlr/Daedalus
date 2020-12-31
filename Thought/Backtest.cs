using System;
using DataStructures;
using System.Collections.Generic;
using System.Linq;
using DataStructures.StatsTools;

namespace Thought
{

    public class BackTestItem
    {
        public BidAskData[] Prices { get; set; }
        public StrategyExecuter Executor { get; set; }
        public int Counter { get; set; }
        public bool Finished { get; set; }

        public long LastTick() {
            if(Counter < Prices.Length)
                return Prices[Counter].Close.Ticks;
            return long.MaxValue;
        } 

        public BackTestItem(TradingField field, MarketSide dir, bool increaseExposure) {
            Prices = field.MarketData.PriceData;
            Counter = 0;
            Finished = false;
            GenerateExecutor(dir, increaseExposure);
            Executor.Init(field);
        }

        public List<Trade> ExecuteStep() {
            var trades= Executor.ExecuteStep();
            Counter++;
            Finished = trades.Count > 0;
            return trades;
        }

        private void GenerateExecutor(MarketSide dir, bool increaseExposure) {
            if (dir.Equals(MarketSide.Bull)) Executor = new LongStrategyExecuter(increaseExposure);
            else Executor = new ShortStrategyExecuter(increaseExposure);
        }
    }

    public class Backtest
    {
        List<BackTestItem> _results { get; set; }
        protected long _earliestDate { get; set; }
        Universe Markets { get; }
        public List<Trade> Results { get; set; }


        public Backtest(Universe markets, MarketSide dir, bool increaseExposure) {
            Markets = markets;
            InitLists();
            GenerateStrategies(markets, dir, increaseExposure);
        }
        
        public List<Trade> RunBackTestByDates() {
            for (int i = 0; i < Markets.Elements.Count; i++)
                if (Markets.Elements[i].MarketData.PriceData[0].Open.Ticks < _earliestDate)
                    _earliestDate = Markets.Elements[i].MarketData.PriceData[0].Close.Ticks;

            while (_results.Any(x=>!x.Finished) && _earliestDate != long.MaxValue)
                IterateThroughMarkets();
            return Results;
        }

        private void InitLists() {
            _results = new List<BackTestItem>();
            Results = new List<Trade>();
            _earliestDate = long.MaxValue;
        }

        private void GenerateStrategies(Universe markets, MarketSide dir, bool increaseExposure) {
            foreach (var market in markets.Elements) 
                _results.Add(new BackTestItem(market, dir, increaseExposure));
        }
        
        protected virtual void IterateThroughMarkets() {
            var newlaggardDate = long.MaxValue;
            foreach (var item in _results)
                newlaggardDate = backtestItemActions(item, newlaggardDate);

            _earliestDate = newlaggardDate;
        }

        private long backtestItemActions(BackTestItem item, long newlaggardDate) {
            if (item.Finished)
                return newlaggardDate;

            if (item.LastTick() <= _earliestDate)
                Results.AddRange(item.ExecuteStep());

            if (item.LastTick() < newlaggardDate)
                newlaggardDate = item.LastTick();
            return newlaggardDate;
        }
    }

    public class LinearBacktest
    {
        protected List<Trade> _results { get; set; }
        private StrategyExecuter _executor { get; set; }
        public Universe Markets { get; }

        public LinearBacktest(Universe markets, StrategyExecuter exec) {
            Markets = markets;
            _results = new List<Trade>();
            _executor = exec;
        }

        public List<Trade> RunBackTest() {
            foreach (var element in Markets.Elements) {
                _executor.Init(element);
                _results.AddRange(_executor.ExecuteAll());
            }

            return _results;
        }
    }
}
