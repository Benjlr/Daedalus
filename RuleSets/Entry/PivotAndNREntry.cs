using DataStructures;
using DataStructures.PriceAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using Action = DataStructures.Action;

namespace RuleSets.Entry
{
    public class PivotAndNREntry : RuleBase
    {
        public PivotAndNREntry()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }

        public override void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var pivots = Pivots.Calculate(data);
            var hourly = SessionCollate.CollateToHourly(data);
            var nrwrsHourly = NRWRBars.Calculate(hourly);


            for (int i = 2; i < data.Count; i++)
            {
                var pivs = pivots.GetRange(0, i).Where(x => x.HighPivot > 0 && x.LowPivot > 0).ToList();
                if (pivs.Count < 3) continue;
                var closestPiv = pivots[pivs.Count - 1];
                var seondclosestPiv = pivots[pivs.Count - 2];
                var furthestPiv = pivots[pivs.Count - 3];

                if (closestPiv.HighPivot == 0) continue;
                if (seondclosestPiv.LowPivot == 0) continue;
                if (furthestPiv.HighPivot == 0) continue;

                var ClosestPivCost = data[closestPiv.Index].High;
                var seondclosestPivCXot = data[seondclosestPiv.Index].Low;
                var furthestpivcost = data[furthestPiv.Index].High;

                if (ClosestPivCost > furthestpivcost &&
                    ClosestPivCost > seondclosestPivCXot)
                {

                    var dist = Math.Abs(ClosestPivCost - seondclosestPivCXot);

                    if (data[i].High > ClosestPivCost - 0.8 * dist)
                    {
                        Satisfied[i] = true;
                        var currenthrlyIndex = hourly.IndexOf(hourly.First(x => x.CloseDate.Hour == data[i].OpenDate.AddHours(-1).Hour));
                        if (nrwrsHourly[currenthrlyIndex] < -7) Satisfied[i] = true;
                    }



                }


            }

        }
    }
}
