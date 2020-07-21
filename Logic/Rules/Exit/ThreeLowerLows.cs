using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Rules.Exit
{
    public class ThreeLowerLows : RuleBase
    {
        public ThreeLowerLows()
        {
            Dir = Thesis.Bull;
            Order = Pos.Exit;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied =new bool[data.Count];

            for (int i = 2; i < data.Count; i++)
            {
                if (data[i].Low < data[i - 1].Low && data[i - 1].Low < data[i - 2].Low) Satisfied[i] = true;
            }
        }
    }
}
