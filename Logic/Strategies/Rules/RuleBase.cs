using System.Collections.Generic;
using Logic.Rules;
using PriceSeriesCore.FinancialSeries;

namespace Logic.Strategies.Rules
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
