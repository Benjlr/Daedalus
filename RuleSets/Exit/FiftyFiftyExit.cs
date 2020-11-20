using DataStructures;
using System;
using System.Collections.Generic;
using Action = DataStructures.Action;

namespace RuleSets.Exit
{
    public class FiftyFiftyExit : RuleBase
    {
        public FiftyFiftyExit()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData)
        {
            Satisfied = new bool[data.Count];

            Random rand = new Random();
            for (int i = 0; i < data.Count; i++)
            {
                if (rand.NextDouble() > 0.5) Satisfied[i] = true;
            }

        }
    }
}
