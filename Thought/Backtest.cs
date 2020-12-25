using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Thought
{
    public class Backtest
    {
        public Universe Markets { get; }

        public Backtest(Universe markets) {
            Markets = markets;
        }

        public virtual List<Trade> RunBackTest(StrategyExecuter exec) {
            var results = new List<Trade>();
            foreach (var element in Markets.Elements) {
                exec.Init(element);
                results.AddRange(exec.ExecuteAll());
            }

            return results;
        }

        public virtual List<Trade> RunBackTestByDates(List<StrategyExecuter> exec){
            results = new List<Trade>();
            counters = new List<int>();
            execs = exec;
            laggardDate = new DateTime(3000,01,01).Ticks;
            for(int i = 0; i < Markets.Elements.Count ; i++){
                if(Markets.Elements[i].MarketData.PriceData[0].Open.Ticks < laggardDate)
                    laggardDate =Markets.Elements[i].MarketData.PriceData[0].Open.Ticks;
                execs[^1].Init(Markets.Elements[i]);
                counters.Add(0);
            }
            IterateThroughMarkets();
            return results;
        }

        List<Trade> results {get;set;}
        List<StrategyExecuter> execs {get;set;}
        List<int> counters {get;set;}
        protected long laggardDate {get;set;}
        long newlaggardDate {get;set;}
        protected virtual void IterateThroughMarkets(){
            newlaggardDate = new DateTime(3000,01,01).Ticks;
            for(int i = 0; i < Markets.Elements.Count ; i++){
                if(counters[i] == Markets.Elements[i].MarketData.PriceData.Length) 
                    continue;
                if(Markets.Elements[i].MarketData.PriceData[counters[i]].Open.Ticks < laggardDate){
                    results.AddRange(execs[i].ExecuteStep());
                    counters[i]++;
                    if(Markets.Elements[i].MarketData.PriceData[counters[i]].Open.Ticks < newlaggardDate)
                        newlaggardDate =Markets.Elements[i].MarketData.PriceData[counters[i]].Open.Ticks;
                }
            }
            laggardDate = newlaggardDate;

        }

        private List<Trade> _orderedTrades;
        private List<DatedResult> _categorisedResults;
        private long _totalSpan;

        public List<DatedResult> ParseResults(TimeSpan time, List<Trade> results) {
            Initialise(results);
            for (long i = 0; i < _totalSpan; i += time.Ticks)
                SumAndAdd(ParseTrades(time, i), i + time.Ticks);
            return _categorisedResults;
        }

        private void Initialise(List<Trade> results) {
            _orderedTrades = results.OrderBy(x => x.ResultTimeline.Last().Date).ToList();
            _totalSpan = (_orderedTrades.Last().ResultTimeline.Last().Date - _orderedTrades.First().ResultTimeline.First().Date);
            _categorisedResults = new List<DatedResult>();
        }

        protected List<DatedResult> ParseTrades(TimeSpan time, long i) {
            var resultsTw = new List<DatedResult>();
            foreach (var t in _orderedTrades) {
                if (CheckRelevant(time, t, i))
                    continue;
                AddRelevantResult(time, t, i, resultsTw);
            }
            return resultsTw;
        }

        protected void SumAndAdd(List<DatedResult> resultsTw, long date) {
            var avgDD = GetDrawdown(resultsTw);
            var sum = resultsTw.Sum(x => x.Return);
            _categorisedResults.Add(new DatedResult(date, sum, avgDD));
        }

        protected double GetDrawdown(List<DatedResult> resultsTw) {
            if (resultsTw.Any(x => x.Drawdown < 0))
                return resultsTw.Where(x => x.Drawdown < 0).Average(x => x.Drawdown);
            else
                return 0;
        }

        protected void AddRelevantResult(TimeSpan time, Trade t, long i, List<DatedResult> resultsTw) {
            var relevantResult = t.ResultTimeline.LastOrDefault(x => x.Date < i + time.Ticks);
            if (relevantResult.Date > 0)
                resultsTw.Add(relevantResult);
        }

        protected bool CheckRelevant(TimeSpan time, Trade t, long i) {
            if (t.ResultTimeline.Last().Date < i)
                return true;
            if (t.ResultTimeline.First().Date > i + time.Ticks)
                return true;
            return false;
        }
    }

}
