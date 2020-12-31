using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Linq;

namespace RuleSets.Entry
{
    public class PivotExit : RuleBase
    {
        public PivotExit()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }

        public override void CalculateBackSeries(BidAskData[] rawData) {
            Satisfied = new bool[rawData.Length];
            var pivots = Pivots.Calculate(rawData);
            var hourly = SessionCollate.CollateToHourly(rawData.ToList());
            var nrwrsHourly = NRWRBars.Calculate(hourly);


            for (int i = 2; i < rawData.Length; i++)
            {
                var lastHighPiv = -1;

                for (int k = i - 1; k > 0; k--)
                {
                    if (pivots[k].HighPivot > 0)
                    {
                        lastHighPiv = k;
                        break;
                    }
                }

                var lastHighPivCost = rawData[lastHighPiv].High.Mid;

                if (rawData[i].High.Mid > lastHighPivCost)
                {
                    //var currenthrlyIndex = hourly.IndexOf(hourly.First(x => x.Close.Time.Hour == data[i].Open.Time.AddHours(-1).Hour));
                    //if (nrwrsHourly[currenthrlyIndex] < -6)
                    Satisfied[i] = true;
                }
            }

        }
    }
}
