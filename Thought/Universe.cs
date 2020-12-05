using DataStructures;
using RuleSets;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Thought
{
    public class Universe
    {
        public List<UniverseObject> Elements { get; }
        private IRuleSet[] Ruleset { get; }

        public Universe(IRuleSet[] rules) {
            Ruleset = rules;
            Elements = new List<UniverseObject>();
        }

        public UniverseObject GetObject(string name) {
            return Elements.FirstOrDefault(x => x.Name == name);
        }

        public void AddMarket(string market) {
            if (!Elements.Any(x => x.Name.Equals(market)))
                Elements.Add(new UniverseObject(Path.GetFileNameWithoutExtension(market), OpenMarket(market), Ruleset));
        }

        public void AddMarket(BidAskData[] market, string name) {
            if (!Elements.Any(x => x.Name.Equals(market)))
                Elements.Add(new UniverseObject(name, Market.MarketBuilder.CreateMarket(market), Ruleset));
        }

        public void AddMarket(SessionData[] market, string name) {
            if (!Elements.Any(x => x.Name.Equals(market)))
                Elements.Add(new UniverseObject(name, Market.MarketBuilder.CreateMarket(market), Ruleset));
        }

        public void AddMarket(List<string> markets) {
            for (int i = 0; i < markets.Count; i++)
                AddMarket(markets[i]);
        }

        private Market OpenMarket(string market) {
            if (DataLoader.CheckDataType(market) == typeof(SessionData))
                return Market.MarketBuilder.CreateMarket(DataLoader.LoadConsolidatedData(market));
            return Market.MarketBuilder.CreateMarket(DataLoader.LoadBidAskData(market));
        }
        public double[] GetArrayForReturns(string myMarket) {
            var market = this.GetObject(myMarket).MarketData;
            return new double[market.RawData.Length];
        }
    }
}