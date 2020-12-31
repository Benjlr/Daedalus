using DataStructures;
using DataStructures.PriceAlgorithms;
using DataStructures.StatsTools;
using System;
using System.Linq;

namespace RuleSets.Entry
{
    public class PivotAndNREntry : RuleBase
    {
        public PivotAndNREntry()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public override void CalculateBackSeries(BidAskData[] rawData) {
            Satisfied = new bool[rawData.Length];
            var pivots = Pivots.Calculate(rawData).Where(x=>x.HighPivot > 1 || x.LowPivot >1).ToList();

            var endPive = pivots[^1].HighPivot > 1 ? 1 : -1;

            for (int i = pivots.Count-2; i >= 0; i--) {
                if (endPive == -1) {
                    if (pivots[i].HighPivot < 2) {
                        pivots.RemoveAt(i);
                    }
                    else endPive = 1;
                }
                else {
                    if (pivots[i].LowPivot < 2) {
                        pivots.RemoveAt(i);
                    }
                    else endPive = -1;
                }
            }


            var startpive = pivots[0].HighPivot > 1 ? 1 : -1;
            for (int i = 1; i < pivots.Count; i++) {
                if (startpive == -1) {
                    if (pivots[i].HighPivot < 2) {
                        String a = "";
                    }
                    else startpive = 1;
                }
                else {
                    if (pivots[i].LowPivot < 2) {
                        String a = "";
                    }
                    else startpive = -1;
                }

            }



            for (int i = 3; i < pivots.Count; i++) {
                var pivs = ListTools.GetNewListByEndIndexAndCount(pivots, i, 3);
                if (pivs.Count < 3) continue;
                var closestPiv = pivots[pivs.Count - 1];
                var seondclosestPiv = pivots[pivs.Count - 2];
                var furthestPiv = pivots[pivs.Count - 3];

                if (closestPiv.HighPivot < 2) continue;
                if (seondclosestPiv.LowPivot < 2) continue;
                if (furthestPiv.HighPivot < 2) continue;

                var ClosestPivCost = rawData[closestPiv.Index].High.Mid;
                var seondclosestPivCXot = rawData[seondclosestPiv.Index].Low.Mid;
                var furthestpivcost = rawData[furthestPiv.Index].High.Mid;

                if (ClosestPivCost > furthestpivcost &&
                    ClosestPivCost > seondclosestPivCXot)
                {

                    var dist = Math.Abs(ClosestPivCost - seondclosestPivCXot);

                    if (rawData[i].High.Mid > ClosestPivCost - 0.5 * dist) {
                        Satisfied[i] = true;
                    }



                }


            }

        }
    }
}
