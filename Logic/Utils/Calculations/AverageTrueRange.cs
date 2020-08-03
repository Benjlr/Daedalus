using PriceSeries.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Calculations
{
    public class AverageTrueRange
    {
        public static List<double> Calculate(List<Session> input, int period = 20)
        {
            var atr = new List<double>();


            if (input.Count - 1 < period)
            {
                input.ForEach(x => atr.Add(x.Close));
                return atr;
            }

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
    }
}
