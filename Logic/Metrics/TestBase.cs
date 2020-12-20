using DataStructures;
using DataStructures.StatsTools;
using System;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        public List<Trade> Trades { get; protected set; }
        public ExtendedStats Stats { get; protected set; }
        public void Run(BidAskData[] data, Func<BidAskData, int, bool> entries, List<BidAskData> myInputs = null) {
            initLists(data.Length);
            IterateEntries(data, entries);
            GenerateStats();
        }

        protected TradeGeneratorInterface _currentTrade { get; set; }

  

        protected void initLists(int length) {
            Trades=new List<Trade>();
        }

        protected void IterateEntries(BidAskData[] data, Func<BidAskData, int, bool> entries) {
            for (int i = 1; i < data.Length; i++) 
                if (entries(data[i],i))
                    PerformEntryActions(data, i);
        }

        protected void PerformEntryActions(BidAskData[] data, int i) {
            SetResult(data, i);
        }

        protected void AddTrade(Trade myTrade) {
            Trades.Add(myTrade);
        }
        
        protected abstract void SetResult(BidAskData[] data, int i);

        private void GenerateStats() {
            Stats = new ExtendedStats(Trades);
        }
    }
}


