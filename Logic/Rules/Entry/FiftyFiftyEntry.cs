using PriceSeries.FinancialSeries;
using System;
using System.Collections.Generic;

namespace Logic.Rules.Entry
{
    public class FiftyFiftyEntry : RuleBase
    {
        public FiftyFiftyEntry()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];
            Random rand = new Random();
            for (int i = 0; i < data.Count; i++)
            {
                /*if (rand.NextDouble() > 0.5) */Satisfied[i] = true;
            }

        }
    }
}
