using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public readonly struct Trade
    {
        public DatedResult[] Results { get; }
        public double FinalResult { get; }
        public double FinalDrawdown { get; }
        public int MarketStart { get; }
        public int MarketEnd { get; }
        public bool Win { get; }


        public Trade(DatedResult[] results, int startIndex) {
            Results = results;
            MarketStart = startIndex;
            MarketEnd = results.Length + startIndex-1;
            FinalResult = results.Last().Return;
            FinalDrawdown = results.Min(x=>x.Drawdown);
            Win = results.Last().Return > 0;
        }

        public int Duration => MarketEnd - MarketStart+1;
        public double[] ResultArray => Results.Select(x => x.Return).ToArray();
    }
    
    public class ArrayBuilder{
        private List<DatedResult> _results { get; set; }
        private int _index { get; set; }
        public static Action<Trade> Callback;


        public void Init(int start) {
            _results = new List<DatedResult>();
            _index = start;
        }

        public void AddResult(long date, double result, double drawdown) {
            _results.Add(new DatedResult(date, result, drawdown < 0 ? drawdown : 0));
        }

        public Trade CompileTrade() {
            var trade= new Trade(_results.ToArray(), _index);
            Callback?.Invoke(trade);
            return trade;
        }
    }

    public readonly struct DatedResult
    {
        public long Date { get; }
        public double Return { get; }
        public double Drawdown { get; }

        public DatedResult(long date, double result, double drawdown) {
            Date = date;
            Return = result;
            Drawdown = drawdown;
        }
    }
}