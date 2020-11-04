using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Logic.Utils;
using Logic.Utils.Calculations;
using PriceSeriesCore;
using PriceSeriesCore.FinancialSeries;

namespace Logic.Rules.Entry
{
    public class PivotAndNREntry :RuleBase
    {
        public PivotAndNREntry()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var pivots = Pivots.Calculate(data, 1);
            var hourly = SessionCollate.CollateToHourly(data);
            var nrwrsHourly = NRWRBars.Calculate(hourly);


            for (int i = 2; i < data.Count; i++)
            {
                var pivs = pivots.GetRange(0, i).Where(x=>x.Pivo!= Pivot.None).ToList();
               if(pivs .Count < 3) continue;
               var closestPiv = pivots[pivs.Count - 1];
               var seondclosestPiv = pivots[pivs.Count - 2];
               var furthestPiv = pivots[pivs.Count - 3];

               if(closestPiv.Pivo != Pivot.High) continue;
               if(seondclosestPiv.Pivo != Pivot.Low) continue;
               if(furthestPiv.Pivo != Pivot.High) continue;

               var ClosestPivCost = data[closestPiv.index].High;
                var seondclosestPivCXot  = data[seondclosestPiv.index].Low;
                var furthestpivcost = data[furthestPiv.index].High;

                if (ClosestPivCost> furthestpivcost&&
                    ClosestPivCost> seondclosestPivCXot)
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
