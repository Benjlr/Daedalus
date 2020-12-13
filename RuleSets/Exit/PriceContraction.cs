using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class PriceContraction : RuleBase
    {
        public PriceContraction()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }


        public override void CalculateBackSeries(List<BidAskData> data, BidAskData[] rawData)
        {
            var atrs = AverageTrueRange.Calculate(data, 3);
            var ten = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 10);
            var twety = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var fissy = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            Satisfied = new bool[data.Count];
            var nrwRs = NRWRBars.Calculate(data);
            int lookback = 40;

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i - lookback, lookback).Max(x => x.High.Mid);
                var low = data.GetRange(i - lookback, lookback).Min(x => x.Low.Mid);
                var cuur = data[i].Close.Mid;

                var percentage = (cuur - low) / (max - low);

                //if (percentage > 0.9)
                {
                    //if (nrwRs[i] < -8) Satisfied[i] = true;
                    if (ten[i] - twety[i] > 2.2 * atrs[i]) Satisfied[i] = true;
                    //if (data[i-1].Low < fissy[i] && data[i].Low < data[i - 1].Low) Satisfied[i] = true;
                }
            }

        }
    }
}
