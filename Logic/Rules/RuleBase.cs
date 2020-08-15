using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Rules
{
    public abstract class RuleBase : IRuleSet
    {
        public Thesis Dir { get; protected set; }
        public Pos Order { get; protected set; }
        public bool[] Satisfied { get; protected set; }

        public virtual void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {            
        }
    }

}
