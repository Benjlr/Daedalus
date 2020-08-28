using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Rules.Exit
{
    public class ThreeHigherHighs : RuleBase
    {
        public ThreeHigherHighs()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied =new bool[data.Count];

            for (int i = 2; i < data.Count; i++)
            {
                if (data[i].High > data[i - 1].High && data[i - 1].High > data[i - 2].High) Satisfied[i] = true;
            }
        }
    }
}
