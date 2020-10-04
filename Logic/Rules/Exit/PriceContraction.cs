using System.Collections.Generic;
using System.Linq;
using Logic.Utils.Calculations;
using PriceSeries.FinancialSeries;

namespace Logic.Rules.Exit
{
    public class PriceContraction : RuleBase
    {
        public PriceContraction()
        {
            Dir = Thesis.Bull;
            Order = Pos.Exit;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            
            Satisfied = new bool[data.Count];
            var nrwRs = NRWRBars.Calculate(data);
            int lookback = 40;

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);

                if (percentage > 0.9)
                {
                    if (nrwRs[i] < -3) Satisfied[i] = true;
                }
            }

        }
    }
}
