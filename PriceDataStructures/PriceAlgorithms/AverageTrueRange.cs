using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class AverageTrueRange
    {
        public static List<double> Calculate(List<BidAskData> input, int period = 20) {
            var atr = new List<double>();
            atr.Add(input.First().High.Mid - input.First().Low.Mid);

            for (var i = 1; i < input.Count; i++) {
                var trueRangeVals = new List<double>
                {
                    input[i].High.Mid - input[i].Low.Mid, 
                    Math.Abs(input[i].High.Mid - input[i - 1].Close.Mid), 
                    Math.Abs(input[i].Low.Mid - input[i - 1].Close.Mid)
                };
                atr.Add(((atr.Last() * (period - 1.0)) + trueRangeVals.Max()) / period);
            }

            return atr;
        }

        public static double Calculate(BidAskData input, double lastClose, double lastATR, int period = 20) {
            if (lastATR == 0 && lastClose == 0)
                return input.High.Mid - input.Low.Mid;

            var trueRangeVals = new double[3]
            {
                input.High.Mid - input.Low.Mid,
                Math.Abs(input.High.Mid - lastClose),
                Math.Abs(input.Low.Mid - lastClose)
            };
            return ((lastATR * (period - 1.0)) + trueRangeVals.Max()) / period;
        }

        public static List<double> CalculateATRPC(List<BidAskData> input, int atrLB = 2, int ATRPCLB = 55) {
            var atr = Calculate(input, atrLB);
            var atrPC = new List<double>();

            for (int i = 0; i < atr.Count; i++) {
                if (i >= ATRPCLB-1 ) {
                    var lastTwenty = atr.GetRange(i- ATRPCLB+1,ATRPCLB).ToList();

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
