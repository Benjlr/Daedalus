using System.Collections.Generic;
using PriceSeriesCore;

namespace RuleSets
{
    public interface IRuleSet
    {
        MarketSide Dir { get; }
        Action Order { get; }
        bool[] Satisfied { get; }
        void CalculateBackSeries(List<Session> data, MarketData[] rawData);
    }
}
