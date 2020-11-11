using PriceSeriesCore;
using System.Collections.Generic;

namespace RuleSets
{
    public abstract class RuleBase : IRuleSet
    {
        public MarketSide Dir { get; protected set; }
        public Action Order { get; protected set; }
        public bool[] Satisfied { get; protected set; }

        public virtual void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
        }
    }
}
