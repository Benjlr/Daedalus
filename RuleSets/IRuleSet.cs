using DataStructures;

namespace RuleSets
{
    public interface IRuleSet
    {
        MarketSide Dir { get; }
        ActionPoint Order { get; }
        bool[] Satisfied { get; }
        void CalculateBackSeries(BidAskData[] rawData);
    }
}
