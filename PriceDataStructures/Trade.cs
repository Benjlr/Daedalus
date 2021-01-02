using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public readonly struct Trade
    {
        public DatedResult[] ResultTimeline { get; }
        public double FinalResult { get; }
        public double FinalDrawdown { get; }
        public int MarketStart { get; }
        public int MarketEnd { get; }
        public bool Win { get; }
        
        public Trade(DatedResult[] results, int startIndex) {
            ResultTimeline = results;
            MarketStart = startIndex;
            MarketEnd = results.Length + startIndex-1;
            FinalResult = results[^1].Return;
            FinalDrawdown = results.Min(x=>x.Drawdown);
            Win = results[^1].Return > 0;
        }

        public int Duration => MarketEnd - MarketStart+1;
        public double[] ResultArray => ResultTimeline.Select(x => x.Return).ToArray();
    }
    
    public class TradeCompiler
    {
        private int _index { get;  }
        private List<DatedResult> ResultTimeline { get;  }
        private double _currentDrawdown { get; set; }
        public DatedResult Status { get; private set; }
        public int Count { get; private set; }

        public TradeCompiler(int start) {
            ResultTimeline = new List<DatedResult>();
            _index = start;
        }

        public void AddResult(long date, double result, double drawdown) {
            Status = new DatedResult(date, result, CheckDrawdown(drawdown));
            Count++;
            ResultTimeline.Add(Status);
        }

        private double CheckDrawdown(double drawdown) {
            if (drawdown < _currentDrawdown)
                _currentDrawdown = drawdown;
            return _currentDrawdown;
        }

        public Trade CompileTrade() {
            var trade= new Trade(ResultTimeline.ToArray(), _index);
            Callback?.Invoke(trade);
            return trade;
        }

        public static Action<Trade> Callback;
    }

    public readonly struct TradePrices
    {
        public double EntryPrice { get; }
        public double StopPrice { get; }
        public double TargetPrice { get; }
        public ExitPrices CurrentExits { get; }

        public TradePrices(ExitPrices exits, double entryPrice) {
            EntryPrice = entryPrice;
            StopPrice = entryPrice * exits.StopPercentage;
            TargetPrice = entryPrice * exits.TargetPercentage;
            CurrentExits = exits;
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