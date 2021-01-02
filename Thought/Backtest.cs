using System;
using DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace Thought
{

    public class BackTestItem
    {
        private ITradeCollator _collator { get; set; }
        public string _name { get; }
        private int _counter { get; set; }
        public long[] Times { get; set; }
        public StrategyExecuter Executor { get; set; }
        public bool Finished { get; set; }

        public long LastTick() {
            if (_counter < Times.Length)
                return Times[_counter];
            return long.MaxValue;
        }

        public BackTestItem(TradingField field, MarketSide dir, ITradeCollator collator, bool increaseExposure) {
            Times = field.MarketData.PriceData.Select(x => x.Close.Ticks).ToArray();
            _name = field.MarketData.Id;
            _counter = 0;
            Finished = false;
            _collator = collator;
            GenerateExecutor(dir, increaseExposure);
            Executor.Init(field);
        }

        public void ExecuteStep() {
            Finished = Executor.ExecuteStep();
            _counter++;
        }

        private void GenerateExecutor(MarketSide dir, bool increaseExposure) {
            if (dir.Equals(MarketSide.Bull)) Executor = new LongStrategyExecuter(increaseExposure, AddTrade, AddResult);
            else Executor = new ShortStrategyExecuter(increaseExposure, AddTrade, AddResult);
        }

        private void AddTrade(Guid id, Trade trade) {
            _collator.AddTrade(trade,id, _name);
        }

        private void AddResult(Guid id, DatedResult result) {
            _collator.AddExposureItem(result,id, _name);
        }

    }

    public class Backtest
    {
        private List<BackTestItem> _results { get; set; }
        private Universe _markets { get; }
        protected long _earliestDate { get; set; }
        public List<Trade> Results { get; set; }


        public Backtest(Universe markets, MarketSide dir, ITradeCollator collator, bool increaseExposure) {
            _markets = markets;
            InitLists();
            GenerateStrategies(markets, dir, collator, increaseExposure);
        }
        
        public void RunBackTestByDates() {
            GetEarliestDate();
            while (_results.Any(x=>!x.Finished) && _earliestDate != long.MaxValue)
                IterateThroughMarkets();
        }

        private void GetEarliestDate() {
            foreach (var element in _markets.Elements)
                if (element.MarketData.PriceData[0].Open.Ticks < _earliestDate)
                    _earliestDate = element.MarketData.PriceData[0].Close.Ticks;
        }

        private void InitLists() {
            _results = new List<BackTestItem>();
            Results = new List<Trade>();
            _earliestDate = long.MaxValue;
        }

        private void GenerateStrategies(Universe markets, MarketSide dir, ITradeCollator collator, bool increaseExposure) {
            foreach (var market in markets.Elements) 
                _results.Add(new BackTestItem(market, dir, collator, increaseExposure));
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
                item.ExecuteStep();

            if (item.LastTick() < newlaggardDate)
                newlaggardDate = item.LastTick();
            return newlaggardDate;
        }
    }

    public class LinearBacktest
    {
        private StrategyExecuter _executor { get; set; }
        public Universe Markets { get; }

        public LinearBacktest(Universe markets, StrategyExecuter exec) {
            Markets = markets;
            _executor = exec;
        }

        public void RunBackTest() {
            foreach (var element in Markets.Elements) {
                _executor.Init(element);
                _executor.ExecuteAll();
            }
        }
    }
}
