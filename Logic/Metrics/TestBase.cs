using DataStructures;
using DataStructures.StatsTools;
using System.Collections.Generic;

namespace Logic.Metrics
{
    public abstract class TestBase : ITest
    {
        public List<Trade> Trades { get; protected set; }
        public ExtendedStats Stats { get; protected set; }
        protected List<double> _currentTrade { get; set; }
        protected int _endIndex { get; set; }

        public void Run(BidAskData[] data, bool[] entries, List<SessionData> myInputs = null) {
            initLists(data.Length);
            IterateEntries(data, entries);
            GenerateStats();
        }

        protected void initLists(int length) {
            Trades=new List<Trade>();
        }

        protected void IterateEntries(BidAskData[] data, bool[] entries) {
            for (int i = 1; i < entries.Length; i++) 
                if (entries[i - 1])
                    PerformEntryActions(data, i);
        }

        protected void PerformEntryActions(BidAskData[] data, int i) {
            _currentTrade = new List<double>();
            SetResult(data, i);
            Trades.Add(new Trade(_currentTrade.ToArray(), i));
        }

        protected abstract void SetResult(BidAskData[] data, int i);

        private void GenerateStats() {
            Stats = new ExtendedStats(Trades);
        }
    }
}


