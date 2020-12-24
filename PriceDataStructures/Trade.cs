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
            FinalResult = results.Last().Return;
            FinalDrawdown = results.Min(x=>x.Drawdown);
            Win = results.Last().Return > 0;
        }

        public int Duration => MarketEnd - MarketStart+1;
        public double[] ResultArray => ResultTimeline.Select(x => x.Return).ToArray();
    }
    
    public readonly struct TradeCompiler
    {
        private int _index { get;  }
        public List<DatedResult> ResultTimeline { get;  }

        public TradeCompiler(int start) {
            ResultTimeline = new List<DatedResult>();
            _index = start;
        }

        public void AddResult(long date, double result, double drawdown) {
            ResultTimeline.Add(new DatedResult(date, result, drawdown < 0 ? drawdown : 0));
        }

        public Trade CompileTrade(bool callback = false) {
            var trade= new Trade(ResultTimeline.ToArray(), _index);
            if(callback)
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

        public TradePrices(ExitPrices exits, double entryPrice) {
            EntryPrice = entryPrice;
            StopPrice = entryPrice * exits.StopPercentage;
            TargetPrice = entryPrice * exits.TargetPercentage;
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