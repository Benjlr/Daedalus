using RuleSets;
using System.Linq;
using Logic.Markets;

namespace Logic.Strategies
{
    public class StrategyBuilder
    {
        public static Strategy CreateStrategy(IRuleSet[] myRules, Market myMarket)
        {
            var dt = myMarket.CostanzaData.ToList();
            foreach (var t in myRules) t.CalculateBackSeries(dt, myMarket.RawData);
            //RulesContext.InitBroaderMarketContext(myMarket.CostanzaData.ToList());

            var entryRules = myRules.Where(x => x.Order.Equals(Action.Entry));
            var exitRules = myRules.Where(x => x.Order.Equals(Action.Exit));

            bool[] entries = new bool[myMarket.RawData.Length];
            bool[] exits = new bool[myMarket.RawData.Length];


            for (int i = 0; i < myMarket.RawData.Length-2; i++)
            {
                if (entryRules.Any(x => x.Satisfied[i])) entries[i] = true;
                //if (exitRules.Any(x => x.Satisfied[i]) || RulesContext.CloseActionitions(myMarket.RawData[i])) exits[i] = true;
                //if (entryRules.Any(x => x.Satisfied[i])) exits[i+10] = true;
            }


            return new Strategy(myRules, entries, exits);
        }

    }
}
