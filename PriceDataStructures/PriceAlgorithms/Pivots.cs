using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public struct PivotStruct
    {
        public int HighPivot { get; set; }
        public int LowPivot { get; set; }
        public int Index { get; set; }
        
        public PivotStruct(int highPiv,int lowPiv, int ind) {
            Index = ind;
            HighPivot = highPiv;
            LowPivot = lowPiv;
        }
    }

    public class Pivots
    {

        public static List<PivotStruct> Calculate(List<SessionData> tlist) {
            
            var calcedPivs = new List<PivotStruct>();
            int order = 1;
            for (var i = 1; i < tlist.Count - 1; i++) {
                var higher = false;
                var lower = false;

                if (tlist[i].High >= tlist[i - 1].High && tlist[i].High >= tlist[i + 1].High) higher = true;
                if (tlist[i].Low <= tlist[i - 1].Low && tlist[i].Low <= tlist[i + 1].Low) lower = true;
                
                if (higher && lower) calcedPivs.Add( new PivotStruct(1,1,i));
                else if (higher) calcedPivs.Add(new PivotStruct(1, 0, i));
                else if (lower) calcedPivs.Add(new PivotStruct(0, 1, i));
            }



            while (order < 10) {
                var highPivs = calcedPivs.Where(x => x.HighPivot == order).ToList();
                var lowPivs = calcedPivs.Where(x => x.LowPivot == order).ToList();

                for (int i = 1; i < highPivs.Count-1; i++) 
                    if (tlist[highPivs[i].Index].High >= tlist[highPivs[i - 1].Index].High
                        && tlist[highPivs[i].Index].High >= tlist[highPivs[i + 1].Index].High) {
                        var preExistingPiv = calcedPivs[calcedPivs.IndexOf(calcedPivs.First(c=>c.Index == highPivs[i].Index))];
                        calcedPivs[calcedPivs.IndexOf(calcedPivs.First(c => c.Index == highPivs[i].Index))] =  new PivotStruct(preExistingPiv.HighPivot+1, preExistingPiv.LowPivot, preExistingPiv.Index);
                    }

                for (int i = 1; i < lowPivs.Count - 1; i++)
                    if (tlist[lowPivs[i].Index].Low <= tlist[lowPivs[i - 1].Index].Low
                        && tlist[lowPivs[i].Index].Low <= tlist[lowPivs[i + 1].Index].Low) {
                        var preExistingPiv = calcedPivs[calcedPivs.IndexOf(calcedPivs.First(c => c.Index == lowPivs[i].Index))];
                        calcedPivs[calcedPivs.IndexOf(calcedPivs.First(c => c.Index == lowPivs[i].Index))] = new PivotStruct(preExistingPiv.HighPivot, preExistingPiv.LowPivot+1, preExistingPiv.Index);
                    }

                order++;
            }


            return calcedPivs;
        }
    }
}