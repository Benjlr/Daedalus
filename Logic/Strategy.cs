using DataStructures;
using DataStructures.StatsTools;
using RuleSets;
using System.Collections.Generic;
using System.Linq;

namespace Logic
{
    public class StaticStrategy : Strategiser
    {
        private bool[] Entries { get; }
        private bool[] Exits { get; }
        private ExitPrices ExitPrices { get; set; }

        private StaticStrategy(bool[] entries, bool[] exits) {
            Entries = entries;
            Exits = exits;
            ExitPrices = ExitPrices.StopOnly(0.98);
        }

        public bool IsEntry(BidAskData data, int i) {
            return Entries[i - 1];
        }

        public bool IsExit(BidAskData data, int i) {
            return Exits[i - 1];
        }

        public ExitPrices AdjustStopTarget(TradePrices initial, DatedResult current) {
            if (current.Return+1 - (initial.StopPrice / initial.EntryPrice) > 0.02)
                ExitPrices = new ExitPrices(current.Return +0.98 ,initial.TargetPrice);
            return ExitPrices;
        }

        public Strategiser Slice(int startIndex, int endIndex) {
            return new StaticStrategy(ListTools.GetNewArrayByIndex(Entries, startIndex, endIndex),
                ListTools.GetNewArrayByIndex(Exits, startIndex, endIndex));
        }

        public class StrategyBuilder
        {
            private List<IRuleSet> _entryRules { get; set; }
            private List<IRuleSet> _exitRules { get; set; }

            public StaticStrategy CreateStrategy(IRuleSet[] myRules, Market myMarket) {
                foreach (var t in myRules)
                    t.CalculateBackSeries(myMarket.PriceData);
                InitRules(myRules);
                return Iterate(myMarket);
            }

            private void InitRules(IRuleSet[] myRules) {
                _entryRules = myRules.Where(x => x.Order.Equals(ActionPoint.Entry)).ToList();
                _exitRules = myRules.Where(x => x.Order.Equals(ActionPoint.Exit)).ToList();
            }

            private StaticStrategy Iterate(Market myMarket) {
                var _entries = new bool[myMarket.PriceData.Length];
                var _exits = new bool[myMarket.PriceData.Length];
                for (int i = 0; i < myMarket.PriceData.Length; i++) {
                    if (_entryRules.Any(x => x.Satisfied[i])) _entries[i] = true;
                    if (_exitRules.Any(x => x.Satisfied[i])) _exits[i] = true;
                }

                return new StaticStrategy(_entries, _exits);
            }
        }
    }

    public interface Strategiser
    {
        public bool IsEntry(BidAskData data, int i);
        public bool IsExit(BidAskData data, int i);
        public ExitPrices AdjustStopTarget(TradePrices initial, DatedResult current);
        public Strategiser Slice(int startIndex, int endIndex);
    }


    public class DynamicStrategy : Strategiser
    {
        public ExitPrices AdjustStopTarget(TradePrices initial, DatedResult current) {
            throw new System.NotImplementedException();
        }

        public bool IsEntry(BidAskData data, int i) {
            throw new System.NotImplementedException();
        }

        public bool IsExit(BidAskData data, int i) {
            throw new System.NotImplementedException();
        }

        public Strategiser Slice(int startIndex, int endIndex) {
            throw new System.NotImplementedException();
        }
    }
}
