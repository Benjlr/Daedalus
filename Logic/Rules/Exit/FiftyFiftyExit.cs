using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;

namespace Logic.Rules.Exit
{
    public class FiftyFiftyExit : RuleBase
    {
        public FiftyFiftyExit()
        {
            Dir = Thesis.Bull;
            Order = Pos.Exit;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
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
