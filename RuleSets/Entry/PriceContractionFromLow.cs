using PriceSeriesCore.FinancialSeries;
using RuleSets.Calculations;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class PriceContractionFromLow : RuleBase
    {
        public PriceContractionFromLow()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {

            Satisfied = new bool[data.Count];
            var nrwRs = NRWRBars.Calculate(data);
            int lookback = 20;

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);

                if (percentage > 0.5)
                {
                    if (nrwRs[i] > 20) Satisfied[i] = true;
                }
            }
        }
    }
}
