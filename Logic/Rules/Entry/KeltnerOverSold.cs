using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class KeltnerOverSold : RuleBase
    {
        public KeltnerOverSold()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];

            var ema20 = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var atr = AverageTrueRange.Calculate(data);

            var lower = new List<double>();
            var upper = new List<double>();

            for (int i = 0; i < data.Count; i++)
            {
                lower.Add(ema20[i] - (2.25 * atr[i]));
                upper.Add(ema20[i] + (2.25 * atr[i]));
            }


            for (int i = 10; i < data.Count; i++)
            {
                if (data[i].Close > ema20[i])
                {
                    for (int j = i; j >= i - 10; j--)
                    {
                        if (data[j].Low < ema20[j])
                        {
                            for (int k = j; k >= j-10; k--)
                            {
                                if(data[k].High > upper[k]) Satisfied[i] = true;
                            }
                          
                        }
                    }

                }
            }

        }
    }
}
