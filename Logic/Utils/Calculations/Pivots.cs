using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PriceSeries;
using PriceSeries.FinancialSeries;

namespace Logic.Utils.Calculations
{
    public struct PivotStruct
    {
        public int index { get; set; }
        public PriceSeries.Pivot Pivo{ get; set; }

        public PivotStruct(PriceSeries.Pivot piv, int ind)
        {
            index = ind;
            Pivo = piv;
        }
    }

    public class Pivots
    {

        public static List<PivotStruct> Calculate(List<Session> tlist, int order)
        {

            var calcedPivs = new List<PivotStruct>();

            for (var i = 0; i < tlist.Count; i++) calcedPivs.Add(new PivotStruct(Pivot.None,i));

            for (var i = 1; i < tlist.Count - 1; i++)
            {
                var higher = false;
                var lower = false;

                if (tlist[i].High >= tlist[i - 1].High && tlist[i].High >= tlist[i + 1].High) higher = true;
                if (tlist[i].Low <= tlist[i - 1].Low && tlist[i].Low <= tlist[i + 1].Low) lower = true;


                if (higher && lower) calcedPivs[i] = new PivotStruct(Pivot.Both,i);
                else if (higher) calcedPivs[i] = new PivotStruct(Pivot.High, i);
                else if (lower) calcedPivs[i] = new PivotStruct(Pivot.Low, i);

                
            }

            for (int i = 1; i <= order; i++)
            {
                List<PivotStruct> tbo = new List<PivotStruct>();
                for (var f = 0; f < tlist.Count; f++) tbo.Add(new PivotStruct(Pivot.None, i));


                for (int j = 1; j < calcedPivs.Count-1; j++)
                {
                    
                    if(calcedPivs[j].Pivo == Pivot.None) continue;
                    
                    var lastLowPiv = -1;
                    var nextLowPiv = -1;
                    var lastHighPiv = -1;
                    var nextHighPiv = -1;

                    for (int k = j-1; k > 0; k--)
                    {
                        if (calcedPivs[k].Pivo == Pivot.Low)
                        {
                            lastLowPiv = k;
                            break;
                        }
                    }
                    for (int k = j - 1; k > 0; k--)
                    {
                        if (calcedPivs[k].Pivo == Pivot.High)
                        {
                            lastHighPiv = k;
                            break;
                        }
                    }
                    for (int k = j+1; k < calcedPivs.Count; k++)
                    {
                        if (calcedPivs[k].Pivo == Pivot.Low)
                        {
                            nextLowPiv = k;
                            break;
                        }
                    }
                    for (int k = j + 1; k < calcedPivs.Count; k++)
                    {
                        if (calcedPivs[k].Pivo == Pivot.High)
                        {
                            nextHighPiv = k;
                            break;
                        }
                    }
                    
                    



                    var thisHigh = tlist[j].High;
                    var thisLow = tlist[j].Low;

                    switch (calcedPivs[j].Pivo)
                    {
                        case Pivot.High:
                            if(lastHighPiv == -1 || nextHighPiv == -1) break;

                            var lastHighPivCost = tlist[lastHighPiv].High;
                            var nextHighPivCost = tlist[nextHighPiv].High;

                            if (thisHigh >= lastHighPivCost && thisHigh >= nextHighPivCost)
                            {
                                tbo[j] = new PivotStruct(Pivot.High, j);
                            }
                            break;
                        case Pivot.Low:
                            if (lastLowPiv == -1 || nextLowPiv == -1) break;
                            var nextLowPivCost = tlist[nextLowPiv].Low;
                            var lasLowPivCost = tlist[lastLowPiv].Low;
                            if (thisLow <= lasLowPivCost && thisLow <= nextLowPivCost)
                            {
                                tbo[j] = new PivotStruct(Pivot.Low,j);
                            }
                            break;
                        case Pivot.Both:
                            if (lastHighPiv != -1 && nextHighPiv != -1)
                            {

                                lastHighPivCost = tlist[lastHighPiv].High;
                                nextHighPivCost = tlist[nextHighPiv].High;

                                if (thisHigh >= lastHighPivCost && thisHigh >= nextHighPivCost)
                                {
                                    tbo[j] = new PivotStruct(Pivot.High, j);
                                }
                            }


                            if (lastLowPiv != -1 && nextLowPiv != -1)
                            {
                                nextLowPivCost = tlist[nextLowPiv].Low;
                                lasLowPivCost = tlist[lastLowPiv].Low;

                                if (thisLow <= lasLowPivCost && thisLow <= nextLowPivCost)
                                {

                                    tbo[j] = new PivotStruct(Pivot.Low, j);
                                }
                            }


                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }



                }

                calcedPivs = tbo;
            }




            return calcedPivs;
        }
    }
}