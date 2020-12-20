using DataStructures;
using Logic;
using System.Collections.Generic;
using System.Linq;

namespace Thought
{
    public class Universe
    {
        public List<TradingField> Elements { get; }

        public Universe() {
            Elements = new List<TradingField>();
        }

        public TradingField GetObject(string name) {
            return Elements.FirstOrDefault(x => x.MarketData.Id == name);
        }

        public void AddMarket(string market, Strategiser strat) {
            if (!Elements.Any(x => x.MarketData.Id.Equals(market)))
                Elements.Add(new TradingField(OpenMarket(market), strat));
        }

        public void AddMarket(Market market, Strategiser strat) {
            if (!Elements.Any(x => x.MarketData.Id.Equals(market.Id)))
                Elements.Add(new TradingField( market, strat));
        }

        public void AddMarket(List<string> markets, Strategiser strat) {
            foreach (var market in markets)
                AddMarket(market, strat);
        }
        
        private Market OpenMarket(string market) {
            return new Market(DataLoader.LoadData(market), market);
        }
        public double[] GetArrayForReturns(string myMarket) {
            var market = this.GetObject(myMarket).MarketData;
            return new double[market.PriceData.Length];
        }
    }
}