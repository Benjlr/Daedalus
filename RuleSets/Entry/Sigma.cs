using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class Sigma : RuleBase
    {
        public Sigma()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var fiftyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            var sigma = SigmaSpike.Calculate(data.Select(x => x.Close.Mid).ToList());

            int lookback = 300;



            Satisfied = new bool[data.Count];

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High.Mid);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low.Mid);
                var cuur = data[i].Close.Mid;

                var percentage = (cuur - low) / (max - low);

                if (percentage > 0.6 && sigma.Skip(i - 5).Any(x => x > 10)) Satisfied[i] = true;
            }

        }
    }
}
