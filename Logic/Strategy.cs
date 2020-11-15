using System.Linq;
using Logic.Utils;
using RuleSets;

namespace Logic
{
    public readonly struct Strategy
    {
        public bool[] Entries { get; }
        public bool[] Exits { get; }

        public IRuleSet[] Rules { get; }

        private Strategy(IRuleSet[] rules, bool[] entries, bool[] exits)
        {
            Entries = entries;
            Exits = exits;
            Rules = rules;
        }

        public Strategy Slice(int startIndex, int endIndex) {
            return new Strategy(Rules, ListTools.GetNewArrayByIndex(Entries, startIndex, endIndex),
                ListTools.GetNewArrayByIndex(Exits, startIndex, endIndex));
        }

        public class StrategyBuilder
        {
            public static Strategy CreateStrategy(IRuleSet[] myRules, Market myMarket) {
                var dt = myMarket.CostanzaData.ToList();
                foreach (var t in myRules) t.CalculateBackSeries(dt, myMarket.RawData);
                //RulesContext.InitBroaderMarketContext(myMarket.CostanzaData.ToList());

                var entryRules = myRules.Where(x => x.Order.Equals(Action.Entry));
                var exitRules = myRules.Where(x => x.Order.Equals(Action.Exit));

                bool[] entries = new bool[myMarket.RawData.Length];
                bool[] exits = new bool[myMarket.RawData.Length];


                for (int i = 0; i < myMarket.RawData.Length - 2; i++) {
                    if (entryRules.Any(x => x.Satisfied[i])) entries[i] = true;
                    //if (exitRules.Any(x => x.Satisfied[i]) || RulesContext.CloseActionitions(myMarket.RawData[i])) exits[i] = true;
                    //if (entryRules.Any(x => x.Satisfied[i])) exits[i+10] = true;
                }


                return new Strategy(myRules, entries, exits);
            }

        }
    }
}
