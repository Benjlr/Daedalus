using System.Collections.Generic;
using System.Linq;
using Logic.Utils.Calculations;
using PriceSeriesCore.FinancialSeries;
using PriceSeriesCore.Indicators.Derived;
using AverageTrueRange = Logic.Utils.Calculations.AverageTrueRange;

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
            var atrs = AverageTrueRange.Calculate(data, 3);
            var ten = SimpleMovingAverage.Calculate(data.Select(x=>x.Close).ToList(), 10);
            var twety = ExponentialMovingAverage.Calculate(data.Select(x=>x.Close).ToList(), 20);
            var fissy = ExponentialMovingAverage.Calculate(data.Select(x=>x.Close).ToList(), 50);
            Satisfied = new bool[data.Count];
            var nrwRs = NRWRBars.Calculate(data);
            int lookback = 40;

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);

                //if (percentage > 0.9)
                {
                    //if (nrwRs[i] < -8) Satisfied[i] = true;
                    if (ten[i] - twety[i] > 2.2*atrs[i]) Satisfied[i] = true;
                    //if (data[i-1].Low < fissy[i] && data[i].Low < data[i - 1].Low) Satisfied[i] = true;
                }
            }

        }
    }
}
