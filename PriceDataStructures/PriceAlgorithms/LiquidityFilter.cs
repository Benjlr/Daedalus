using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class LiquidityFilter
    {
        private const int _minimumVolume = 150000;
        private const double _minimumPrice = 2;
        private static double _minimumTurnOver => _minimumPrice * _minimumVolume;

        public static bool IsLiquid(List<double> closes, List<double> volumes) {
            var vals = new List<double>();
            for (int i = 0; i < closes.Count; i++)
                vals.Add(closes[i] * volumes[i]);
            
            var avgTurnover = MovingAverage.SimpleMovingAverage(vals, 30);
            var avgVolume = MovingAverage.SimpleMovingAverage(volumes, 30);
            avgTurnover = avgTurnover.Skip(avgTurnover.Count - 30).ToList();
            avgVolume = avgVolume.Skip(avgVolume.Count - 30).ToList();
            var boolOne = avgTurnover.Count(c => c < _minimumTurnOver) < 10;
            var boolTwo = avgVolume.Count(x => x < _minimumVolume) < 10;
            return boolOne  && boolTwo;
        }
    }
}
