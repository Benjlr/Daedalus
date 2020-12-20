using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class KeltnerOverSold : RuleBase
    {
        public KeltnerOverSold()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var myData = rawData.ToList();
            Satisfied = new bool[myData.Count];
            var daily = SessionCollate.CollateToHourly(myData);


            var ema20 = MovingAverage.ExponentialMovingAverage(daily.Select(x => x.Close.Mid).ToList(), 20);
            var atr = AverageTrueRange.Calculate(myData);


            var lower = new List<double>();
            var upper = new List<double>();

            for (int i = 0; i < daily.Count; i++)
            {
                lower.Add(ema20[i] - (3 * atr[i]));
                upper.Add(ema20[i] + (3 * atr[i]));
            }

            for (int i = 10; i < daily.Count; i++)
            {
                if (daily[i].Low.Mid < lower[i])
                {

                    Satisfied[i] = true;
                    break;

                }
            }

        }
    }
}
