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
            //var daily = SessionCollate.CollateToHourly(myData);


            var ema20 = MovingAverage.ExponentialMovingAverage(myData.Select(x => x.Close.Mid).ToList(), 20);
            var atr = AverageTrueRange.Calculate(myData);


            var lower = new List<double>();
            var upper = new List<double>();

            for (int i = 0; i < myData.Count; i++)
            {
                lower.Add(ema20[i] - (4 * atr[i]));
                upper.Add(ema20[i] + (4 * atr[i]));
            }

            for (int i = 10; i < myData.Count; i++)
            {
                if (myData[i-1].Low.Mid < lower[i-1] )
                {

                    Satisfied[i] = true;
                    break;

                }
            }

        }
    }
}
