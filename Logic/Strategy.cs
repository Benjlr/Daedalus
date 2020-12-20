using System.Collections.Generic;
using DataStructures;
using RuleSets;
using System.Linq;
using DataStructures.StatsTools;

namespace Logic
{
    public class StaticStrategy : Strategiser
    {
        private bool[] Entries { get; }
        private bool[] Exits { get; }

        private IRuleSet[] Rules { get; }

        private StaticStrategy(IRuleSet[] rules, bool[] entries, bool[] exits)
        {
            Entries = entries;
            Exits = exits;
            Rules = rules;
        }

        public bool IsEntry(BidAskData data, int i) {
            return Entries[i-1];
        }

        public bool IsExit(BidAskData data, int i) {
            return Exits[i - 1];
        }

        public ExitPrices AdjustPrices(BidAskData data, int i, double currentReturn) {
            return new ExitPrices(0.9,1.1);
        }
        public Strategiser Slice(int startIndex, int endIndex) {
            return new StaticStrategy(Rules, ListTools.GetNewArrayByIndex(Entries, startIndex, endIndex),
                ListTools.GetNewArrayByIndex(Exits, startIndex, endIndex));
        }

        public class StrategyBuilder
        {
            private bool[] _exits { get; set; }
            private bool[] _entries { get; set; }

            private List<IRuleSet> _entryRules { get; set; }
            private List<IRuleSet> _exitRules { get; set; }

            public StaticStrategy CreateStrategy(IRuleSet[] myRules, Market myMarket) {
                foreach (var t in myRules) 
                    t.CalculateBackSeries(myMarket.PriceData);

                InitRules(myRules);
                Iterate(myMarket);
                return new StaticStrategy(myRules, _entries, _exits);
            }

            private void InitRules(IRuleSet[] myRules) {
                _entryRules = myRules.Where(x => x.Order.Equals(ActionPoint.Entry)).ToList();
                _exitRules = myRules.Where(x => x.Order.Equals(ActionPoint.Exit)).ToList();
            }

            private void Iterate(Market myMarket) {
                _entries = new bool[myMarket.PriceData.Length];
                _exits = new bool[myMarket.PriceData.Length];
                for (int i = 0; i < myMarket.PriceData.Length; i++) {
                    if (_entryRules.Any(x => x.Satisfied[i])) _entries[i] = true;
                    if (_exitRules.Any(x => x.Satisfied[i])) _exits[i] = true;
                }
            }
        }
    }

    public interface Strategiser
    {
        public bool IsEntry(BidAskData data, int i);
        public bool IsExit(BidAskData data, int i);
        public ExitPrices AdjustPrices(BidAskData data, int i, double currentReturn);

        public Strategiser Slice(int startIndex, int endIndex);
    }

}
