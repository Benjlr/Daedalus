using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class PriceContractionFromLow : RuleBase
    {
        public PriceContractionFromLow()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
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

                if (percentage < 0.05)
                {
                    if (nrwRs[i] > 20) Satisfied[i] = true;
                }
            }
        }
    }
}
