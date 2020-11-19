using System.Collections.Generic;
using System.Linq;

namespace DataStructures.PriceAlgorithms
{
    public class LiquidityFilter
    {
        private const int _minimumVolume = 150000;
        private const double _minimumPrice = 5;
        private static double _minimumTurnOver => _minimumPrice * _minimumVolume;


        public static bool FilterForLiquidity(List<SessionData> stockSessions) {
            var avgTurnover = MovingAverage.SimpleMovingAverage(stockSessions.Select(c => c.Close * c.Volume).ToList(), 30).Last();
            var avgVolume = MovingAverage.SimpleMovingAverage(stockSessions.Select(c => c.Volume).ToList(), 30).Last();
            return avgTurnover > _minimumTurnOver && avgVolume > _minimumVolume;
        }
    }
}
