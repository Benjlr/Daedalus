using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class KeltnerOverBought : RuleBase
    {
        public KeltnerOverBought()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }
        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];

            var ema20 = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var atr = AverageTrueRange.Calculate(data);

            var lower = new List<double>();
            var upper = new List<double>();

            for (int i = 0; i < data.Count; i++)
            {
                lower.Add(ema20[i] - (2.25 * atr[i]));
                upper.Add(ema20[i] + (2.25 * atr[i]));
            }


            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Close.Mid > upper[i]) Satisfied[i] = true;
            }

        }
    }
}
