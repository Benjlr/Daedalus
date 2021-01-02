using DataStructures;
using Logic;

namespace Thought
{
    public readonly struct TradingField
    {
        public Market MarketData { get; }
        public Strategiser Strategy { get; }


        public TradingField(Market market, Strategiser strat) {
            MarketData = market;
            Strategy = strat;
        }
    }
}
