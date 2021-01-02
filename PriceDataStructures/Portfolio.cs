using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public interface ITradeCollator
    {
        Dictionary<Guid, DatedResult> CurrentExposure { get; }
        List<MarketResults> Results { get; set; }
        void AddTrade(Trade trade, Guid id, string marketName);
        void AddExposureItem(DatedResult result, Guid id, string marketName);
        void RemoveExposureItem( Guid id);
    }

    public class SimpleCollator : ITradeCollator
    {
        public SimpleCollator() {
            CurrentExposure = new Dictionary<Guid, DatedResult>();
            Results = new List<MarketResults> {new MarketResults("")};
        }

        public Dictionary<Guid, DatedResult> CurrentExposure { get; }

        public List<MarketResults> Results { get; set; }


        public void AddTrade(Trade trade, Guid id, string marketName) {
           Results[0].Trades.Add(trade);
        }

        public void AddExposureItem(DatedResult result, Guid id, string marketName) {
        }

        public void RemoveExposureItem(Guid id) {

        }

    }

    public class Portfolio : ITradeCollator
    {
        public Portfolio() {
            Results = new List<MarketResults>();
            CurrentExposure = new Dictionary<Guid, DatedResult>();
        }
        public int Cash { get; set; }
        public int InvestedCount { get; set; }
        public double InvestedCapital { get; set; }
        public double RiskAmount { get; set; }
        public Dictionary<Guid, DatedResult> CurrentExposure { get; }

        public List<MarketResults> Results { get; set; }

        public void AddTrade(Trade trade, Guid id, string marketName) {
            if(!Results.Any(x=>x.MarketName.Equals(marketName)))
                Results.Add(new MarketResults(marketName) );
            Results.First(x=>x.MarketName.Equals(marketName)).Trades.Add(trade);
        }

        public virtual void AddExposureItem(DatedResult result, Guid id, string marketName) {
            CurrentExposure.Add(id,result);
        }
        public void RemoveExposureItem(Guid id) {
            CurrentExposure.Remove(id);
;        }
    }

    public class MarketResults
    {
        public MarketResults(string name) {
            MarketName = name;
            Trades = new List<Trade>();
        }
        public string MarketName { get; set; }
        public List<Trade> Trades { get; set; }
    }
}
