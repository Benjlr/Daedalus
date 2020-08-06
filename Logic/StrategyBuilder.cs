using System.Diagnostics.Tracing;
using Logic.Rules;
using System.Linq;

namespace Logic
{
    public class StrategyBuilder
    {
        public static Strategy CreateStrategy(IRuleSet[] myRules, Market myMarket)
        {
            var dt = myMarket.CostanzaData.ToList();
            foreach (var t in myRules) t.CalculateBackSeries(dt, myMarket.RawData);

            var entryRules = myRules.Where(x => x.Order.Equals(Pos.Entry));
            var exitRules = myRules.Where(x => x.Order.Equals(Pos.Exit));

            bool[] entries = new bool[myMarket.RawData.Length];
            bool[] exits = new bool[myMarket.RawData.Length];

            int[] durations = new int[myMarket.RawData.Length];


            for (int i = 0; i < myMarket.RawData.Length; i++)
            {
                if (entryRules.Any(x => x.Satisfied[i]) && RulesContext.IsValid(myMarket.RawData[i])) entries[i] = true;
                if (exitRules.Any(x => x.Satisfied[i]) || RulesContext.ClosePositions(myMarket.RawData[i])) exits[i] = true;
            }

            for (int i = 0; i < entries.Length; i++)
            {
                if (entries[i])
                {
                    int counter = 1;
                    for (int x = i+1; x < exits.Length; x++)
                    {
                        if (exits[x])
                        {
                            durations[i] = counter;
                            break;
                        }
                        counter++;
                    }
                }
            }

            return new Strategy(myRules, entries, exits, durations);
        }

    }
}
