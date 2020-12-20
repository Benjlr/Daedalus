using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using Logic;

namespace Thought
{
    public class Backtest
    {
        public Universe Markets { get; }

        public Backtest(Universe markets) {
            Markets = markets;

        }

        public List<Trade> RunBackTest() {
            
            var results = new List<Trade>();
            foreach (var element in Markets.Elements) {
                var exec = new LongStrategyExecuter(true);
                var trades = exec.Execute(element);
                foreach (var trade in trades)
                    results.Add(trade);
            }

            return results;
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
            _orderedTrades = results.OrderBy(x => x.Results.Last().Date).ToList();
            _totalSpan = (_orderedTrades.Last().Results.Last().Date - _orderedTrades.First().Results.First().Date);
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
            var relevantResult = t.Results.LastOrDefault(x => x.Date < i + time.Ticks);
            if (relevantResult.Date > 0)
                resultsTw.Add(relevantResult);
        }

        protected bool CheckRelevant(TimeSpan time, Trade t, long i) {
            if (t.Results.Last().Date < i)
                return true;
            if (t.Results.First().Date > i + time.Ticks)
                return true;
            return false;
        }
    }


}
