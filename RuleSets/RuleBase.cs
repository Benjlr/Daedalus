using DataStructures;

namespace RuleSets
{
    public abstract class RuleBase : IRuleSet
    {
        public MarketSide Dir { get; protected set; }
        public ActionPoint Order { get; protected set; }
        public bool[] Satisfied { get; protected set; }

        public virtual void CalculateBackSeries(BidAskData[] rawData)
        {
        }
    }
}
