using System;
using Logic.Utils.Calculations;
using PriceSeriesCore;
using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class PivotExit :RuleBase
    {
        public PivotExit()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var pivots = Pivots.Calculate(data, 2);
            var hourly = SessionCollate.CollateToHourly(data);
            var nrwrsHourly = NRWRBars.Calculate(hourly);


            for (int i = 2; i < data.Count; i++)
            {
                var lastHighPiv = -1;

                for (int k = i - 1; k > 0; k--)
                {
                    if (pivots[k].Pivo == Pivot.High)
                    {
                        lastHighPiv = k;
                        break;
                    }
                }
  
                var lastHighPivCost = data[lastHighPiv].High;

                if (data[i].High > lastHighPivCost )
                {
                    //var currenthrlyIndex = hourly.IndexOf(hourly.First(x => x.CloseDate.Hour == data[i].OpenDate.AddHours(-1).Hour));
                    //if (nrwrsHourly[currenthrlyIndex] < -6)
                        Satisfied[i] = true;
                }
            }

        }
    }
}
