using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class FiftyFiftyExit : RuleBase
    {
        public FiftyFiftyExit()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];

            Random rand = new Random();
            for (int i = 0; i < data.Count; i++)
            {
                if (rand.NextDouble() > 0.5) Satisfied[i] = true;
            }

        }
    }
}
