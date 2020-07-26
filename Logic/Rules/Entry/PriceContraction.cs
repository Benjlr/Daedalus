using PriceSeries.FinancialSeries;
using PriceSeries.Indicators.Derived;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class PriceContraction : RuleBase
    {
        public PriceContraction()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var nrwRs = new List<int>();
            data.ForEach(x => nrwRs.Add(0));

            for (var i = data.Count - 1; i >= 1; i--)
            {
                var wideRange = 0;
                var narrowRange = 0;
                var myWRCounter = i - 1;
                var myNRCounter = i - 1;

                var todaysRange = data[i].High - data[i].Low;
                var yesterdaysRange = data[i - 1].High - data[i - 1].Low;


                while (todaysRange > yesterdaysRange)
                {
                    wideRange++;
                    myWRCounter--;
                    if (myWRCounter < 1) break;
                    yesterdaysRange = data[myWRCounter].High - data[myWRCounter].Low;
                }

                yesterdaysRange = data[i - 1].High - data[i - 1].Low;

                while (todaysRange < yesterdaysRange)
                {
                    narrowRange--;
                    myNRCounter--;
                    if (myNRCounter < 1) break;
                    yesterdaysRange = data[myNRCounter].High - data[myNRCounter].Low;
                }

                if (narrowRange <= -5) nrwRs[i] = narrowRange;
                else if (wideRange >= 5) nrwRs[i] = wideRange;
            }

            Satisfied = new bool[data.Count];

            for (int i = 53; i < nrwRs.Count; i++)
            {
                var max = data.GetRange(i-52,52).Max(x => x.High);
                var low = data.GetRange(i-52,52).Min(x => x.Low);
                var cuur = data[i].Close;

                var twohundredMA = ExponentialMovingAverage.Calculate(data.Select(x => x.Close).ToList(), 50);


                if ((cuur - low) / (max - low) > 0.8 && cuur > twohundredMA[i])
                {
                    if (nrwRs[i] < -5) Satisfied[i] = true;
                }
            }






        }
    }
}
