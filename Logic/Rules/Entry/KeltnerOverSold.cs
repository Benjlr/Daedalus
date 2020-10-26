using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;
using Logic.Utils.Calculations;

namespace Logic.Rules.Entry
{
    public class KeltnerOverSold : RuleBase
    {
        public KeltnerOverSold()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public override void CalculateBackSeries(List<Session> myData, MarketData[] rawData)
        {
            Satisfied = new bool[myData.Count];
            var daily = SessionCollate.CollateToHourly(myData);


            var ema20 = MovingAverage.ExponentialMovingAverage(daily.Select(x => x.Close).ToList(), 20);
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
                if (daily[i].Low < lower[i])
                {

                    Satisfied[i] = true;
                    break;

                }
            }

        }
    }
}
