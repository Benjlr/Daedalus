using PriceSeriesCore.FinancialSeries;
using RuleSets.Calculations;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class Sigma : RuleBase
    {
        public Sigma()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fiftyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var sigma = SigmaSpike.Calculate(data);

            int lookback = 300;



            Satisfied = new bool[data.Count];

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);

                if (percentage > 0.6 && sigma.Skip(i - 5).Any(x => x > 10)) Satisfied[i] = true;
            }

        }
    }
}
