using LinqStatistics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Calculations
{
    public class MovingAverage
    {
        public static List<double> ExponentialMovingAverage(List<double> input, int period)
        {
            var retval = new List<double>();
            var multiplier = 2.0 / (period + 1);

            retval.Add(input[0]);
            for (var i = 1; i < period; i++)
            {
                retval.Add((input[i] - input.GetRange(0, i).ToList().Average()) * multiplier + input.GetRange(0, i).ToList().Average());
            }

            retval.Add((input[period] - input.GetRange(0, period).ToList().Average()) * multiplier + input.GetRange(0, period).ToList().Average());
            for (var i = period + 1; i < input.Count; i++) retval.Add((input[i] - retval.Last()) * multiplier + retval.Last());
            return retval;

        }

        public static List<double> SimpleMovingAverage(List<double> input, int period)
        {
            var retval = new List<double>();

            if (input.Count - 1 < period)
            {
                retval.Add(input[0]);
                for (var i = 1; i < input.Count; i++) retval.Add(input.GetRange(0, i).Average());
                return retval;
            }

            retval.Add(input[0]);
            for (var i = 1; i < period; i++) retval.Add(input.GetRange(0, i).Average());
            for (var i = period; i < input.Count; i++) retval.Add(input.GetRange(i - (period - 1), period).Average());
            return retval;
        }

        public static List<double> GetRMSE(List<List<double>> lines)
        {
            var retval = new List<double>();

            for (int i = 0; i < lines[0].Count; i++)
            {
                List<double> divisors = new List<double>();

                lines.ForEach(x => divisors.Add(x[i]));
                var rms = divisors.RootMeanSquare();
                var error = 0.0;
                divisors.ForEach(x => error += Math.Abs(x - rms));
                error = error / divisors.Count;

                retval.Add(error);
            }

            return retval;
        }
    }
}
