using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class AverageTrueRange
    {
        public static List<double> Calculate(List<SessionData> input, int period = 20) {
            var atr = new List<double>();
            atr.Add(input.First().High - input.First().Low);

            for (var i = 1; i < input.Count; i++) {
                var trueRangeVals = new List<double>
                {
                    input[i].High - input[i].Low, 
                    Math.Abs(input[i].High - input[i - 1].Close), 
                    Math.Abs(input[i].Low - input[i - 1].Close)
                };
                atr.Add(((atr.Last() * (period - 1.0)) + trueRangeVals.Max()) / period);
            }

            return atr;
        }

        public static List<double> CalculateATRPC(List<SessionData> input, int atrLB = 2, int ATRPCLB = 55) {
            var atr = Calculate(input, atrLB);
            var atrPC = new List<double>();

            for (int i = 0; i < atr.Count; i++) {
                if (i > ATRPCLB ) {
                    var lastTwenty = atr.Skip(Math.Max(0, i - ATRPCLB-1)).Take(ATRPCLB+1).ToList();

                    var last = lastTwenty.Last();
                    var Min = lastTwenty.Min();
                    var Max = lastTwenty.Max();

                    var curr = (last - Min) / (Max - Min);
                    if (curr < 0.1) curr = 0;
                    atrPC.Add(curr);
                }
                else atrPC.Add(1);
            }

            return atrPC;
        }
    }
}
