using DataStructures;
using System;
using System.Linq;

namespace RuleSets.Entry
{
    public class FiftyFiftyEntry : RuleBase
    {
        public FiftyFiftyEntry()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public override void CalculateBackSeries( BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];
            Random rand = new Random();
            for (int i = 0; i < data.Count; i++)
            {
                /*if (rand.NextDouble() > 0.5) */
                Satisfied[i] = true;
            }

        }
    }
}
