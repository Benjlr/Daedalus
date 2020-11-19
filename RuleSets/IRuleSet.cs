using DataStructures;
using System.Collections.Generic;

namespace RuleSets
{
    public interface IRuleSet
    {
        MarketSide Dir { get; }
        Action Order { get; }
        bool[] Satisfied { get; }
        void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData);
    }
}
