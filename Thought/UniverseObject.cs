using DataStructures;
using Logic;
using RuleSets;

namespace Thought
{
    public readonly struct UniverseObject
    {
        public string Name { get; }
        public Market MarketData { get; }
        public Strategy Strategy { get; }

        public UniverseObject(string name, Market market, IRuleSet[] rules) {
            Name = name;
            MarketData = market;
            Strategy = Strategy.StrategyBuilder.CreateStrategy(rules, market);
        }
    }
}
