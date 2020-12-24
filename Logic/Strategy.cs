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
        public ExitInterface Stops { get; }

        private StaticStrategy(bool[] entries, bool[] exits, ExitInterface stops) {
            Entries = entries;
            Exits = exits;
            Stops = stops;
        }


        public bool IsEntry(BidAskData data, int i) {
            return Entries[i - 1];
        }

        public bool IsExit(BidAskData data, int i) {
            return Exits[i - 1];
        }

        public Strategiser Slice(int startIndex, int endIndex) {
            return new StaticStrategy(ListTools.GetNewArrayByIndex(Entries, startIndex, endIndex),
                ListTools.GetNewArrayByIndex(Exits, startIndex, endIndex), this.Stops);
        }

        public class StrategyBuilder
        {
            private List<IRuleSet> _entryRules { get; set; }
            private List<IRuleSet> _exitRules { get; set; }

            public StaticStrategy CreateStrategy(IRuleSet[] myRules, Market myMarket, ExitInterface stops) {
                foreach (var t in myRules)
                    t.CalculateBackSeries(myMarket.PriceData);
                InitRules(myRules);
                return Iterate(myMarket, stops);
            }

            private void InitRules(IRuleSet[] myRules) {
                _entryRules = myRules.Where(x => x.Order.Equals(ActionPoint.Entry)).ToList();
                _exitRules = myRules.Where(x => x.Order.Equals(ActionPoint.Exit)).ToList();
            }

            private StaticStrategy Iterate(Market myMarket, ExitInterface stops) {
                var _entries = new bool[myMarket.PriceData.Length];
                var _exits = new bool[myMarket.PriceData.Length];
                for (int i = 0; i < myMarket.PriceData.Length; i++) {
                    if (_entryRules.Any(x => x.Satisfied[i])) _entries[i] = true;
                    if (_exitRules.Any(x => x.Satisfied[i])) _exits[i] = true;
                }

                return new StaticStrategy(_entries, _exits, stops);
            }
        }
    }

    public interface Strategiser
    {
        public ExitInterface Stops { get; }
        public bool IsEntry(BidAskData data, int i);
        public bool IsExit(BidAskData data, int i);
        public Strategiser Slice(int startIndex, int endIndex);
    }


    public class DynamicStrategy : Strategiser
    {
        public ExitPrices AdjustStopTarget(TradePrices initial, DatedResult current) {
            throw new System.NotImplementedException();
        }

        public ExitInterface Stops { get; }

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
