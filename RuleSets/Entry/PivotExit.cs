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

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];
            var pivots = Pivots.Calculate(data);
            var hourly = SessionCollate.CollateToHourly(data);
            var nrwrsHourly = NRWRBars.Calculate(hourly);


            for (int i = 2; i < data.Count; i++)
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

                var lastHighPivCost = data[lastHighPiv].High.Mid;

                if (data[i].High.Mid > lastHighPivCost)
                {
                    //var currenthrlyIndex = hourly.IndexOf(hourly.First(x => x.Close.Time.Hour == data[i].Open.Time.AddHours(-1).Hour));
                    //if (nrwrsHourly[currenthrlyIndex] < -6)
                    Satisfied[i] = true;
                }
            }

        }
    }
}
