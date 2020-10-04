using System;
using System.Collections.Generic;
using System.Linq;
using PriceSeries.FinancialSeries;

namespace Logic.Utils.Calculations
{
    public class AverageTrueRange
    {
        public static List<double> Calculate(List<Session> input, int period = 20)
        {
            var atr = new List<double>();

            var trueRangeVals = new List<double> { input.First().High - input.First().Low, Math.Abs(input.First().High - input.First().Close), Math.Abs(input.First().Low - input.First().Close) };
            atr.Add(trueRangeVals.Max());

            for (var i = 1; i < input.Count; i++)
            {
                trueRangeVals = new List<double> { input[i].High - input[i].Low, Math.Abs(input[i].High - input[i - 1].Close), Math.Abs(input[i].Low - input[i - 1].Close) };
                var tr = trueRangeVals.Max();
                atr.Add((atr.Last() * (period - 1.00) + tr) / period);
            }

            return atr;
        }

        public static List<double> CalculateATRPC(List<Session> input)
        {
            var atr = new List<double>();
            var atrPC = new List<double>();

            double period = 5;


            var trueRangeVals = new List<double> { input.First().High - input.First().Low, Math.Abs(input.First().High - input.First().Close), Math.Abs(input.First().Low - input.First().Close) };
            atr.Add(trueRangeVals.Max());

            for (var i = 1; i < input.Count; i++)
            {
                trueRangeVals = new List<double> { input[i].High - input[i].Low, Math.Abs(input[i].High - input[i - 1].Close), Math.Abs(input[i].Low - input[i - 1].Close) };
                var tr = trueRangeVals.Max();
                atr.Add((atr.Last() * (period - 1.00) + tr) / period);
            }


            for (int i = 0; i < atr.Count; i++)
            {
                if (i > 20)
                {
                    var lastTwenty = atr.Skip(Math.Max(0, i - 29)).Take(30).ToList();

                    var last = lastTwenty.Last();
                    var Min = lastTwenty.Min();
                    var Max = lastTwenty.Max();

                    var curr = (last - Min) / (Max - Min);
                    if (curr > 0.85) curr = 1;
                    else if (curr > 0.66) curr = 0.66;
                    else if (curr > 0.33) curr = 0.33;
                    else if (curr > 0.1) curr = 0.1;
                    else curr = 0.0;
                    atrPC.Add(curr);
                }
                else atrPC.Add(0);
            }

            return atrPC;
        }
    }
}
