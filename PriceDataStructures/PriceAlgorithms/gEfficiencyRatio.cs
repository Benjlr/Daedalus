using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class gEfficiencyRatio
    {
        public static List<double> Calculate(List<SessionData> input, int lb = 20, int smoothing = 20) {
            var unsmoothedGer = new List<double>();

            for (var i = 0; i < lb + 1; i++) unsmoothedGer.Add(0);
            for (var i = lb + 1; i < input.Count; i++) {
                var tempRange = input.GetRange(i - lb + 1, lb);
                var range = tempRange.Max(x => x.High) - tempRange.Min(x => x.Low);
                unsmoothedGer.Add(range > 0 ? (tempRange.Last().Close - tempRange.Min(x => x.Low)) / range : 0);
            }

            return MovingAverage.SimpleMovingAverage(unsmoothedGer, smoothing > 0 ? smoothing : 1);
        }
    }
}
