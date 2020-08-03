using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
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
            int lookback = 60;

            for (int i = lookback; i < data.Count; i++)
            {
                var max = data.GetRange(i-lookback,lookback).Max(x => x.High);
                var low = data.GetRange(i-lookback,lookback).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);

                if (percentage > 0.95)
                {
                    if (nrwRs[i] < -10) Satisfied[i] = true;
                }
            }

            //private double stopLows = 9;
            //private double atrnum = 50;
            //private int gapSize = 8;
            //private double spread = 5;
            //private int startLook = 9;
            //private int endLook = 4;


            //    if ((Math.Abs(tl[i].Close - twenny[i]) < atrnum * atr[i]
            //         || Math.Abs(tl[i].Close - tenny[i]) < atrnum * atr[i])
            //        && tl[i].Close > twohunny[i])
            //    {
            //        for (int j = i - startLook; j < i - endLook; j++)
            //        {
            //            if (nrwr[j] > gapSize && tl[j].High > tl[i].Close && tl[i].Close < six[i] && tl[i].OpenAsk - tl[i].OpenBid <= spread)
            //            {

            //                direction = Positioning.Long;
            //                size = ((int)Math.Floor(currCapital * 0.07 / stop));

            //                return;
            //            }
            //        }
            //    }

            //    if ((Math.Abs(tl[i].Close - twenny[i]) < atrnum * atr[i]
            //         || Math.Abs(tl[i].Close - tenny[i]) < atrnum * atr[i])
            //        && tl[i].Close < twohunny[i])
            //    {
            //        for (int j = i - startLook; j < i - endLook; j++)
            //        {
            //            if (nrwr[j] > gapSize && tl[j].Low < tl[i].Close && tl[i].Close > six[i] && tl[i].OpenAsk - tl[i].OpenBid <= spread)
            //            {
            //                direction = Positioning.Short;
            //                size = ((int)Math.Floor(currCapital * 0.07 / stop));

            //                return;
            //            }
            //        }
            //    }




        }
    }
}
