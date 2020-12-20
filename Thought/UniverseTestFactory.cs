using DataStructures;
using Logic.Metrics;
using System.Threading.Tasks;

namespace Thought
{
    public class UniverseTestFactory
    {
        public UniverseTest[] RunTests(Universe myUniverse, TestOption option) {
            return IterateTests(myUniverse, option);
        }

        private UniverseTest[] IterateTests(Universe myUniverse, TestOption options) {
            var results = new UniverseTest[myUniverse.Elements.Count];
            Parallel.For(
                0, myUniverse.Elements.Count, i => {
                results[i] = new UniverseTest(myUniverse.Elements[i], options);
            });
            return results;
        }
    }

    public readonly struct UniverseTest
    {
        public string Name { get; }
        public ITest[] LongTests { get; }
        public ITest[] ShortTests { get; }

        public UniverseTest(TradingField market, TestOption option) {
            Name = market.MarketData.Id;
            LongTests = option.Run(market.Strategy, market.MarketData, MarketSide.Bull);
            ShortTests = option.Run(market.Strategy, market.MarketData, MarketSide.Bear);
        }
    }
}
