using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public interface ITradeCollator
    {
        Dictionary<Guid, MarketExposure> CurrentExposure { get; }
        List<MarketResults> Results { get; set; }
        bool CanEnter(string id);
        void AddTrade(Trade trade, Guid id, string marketName);
        void AddExposureItem(MarketExposure result, Guid id);
        void RemoveExposureItem( Guid id);
    }

    public class SimpleCollator : ITradeCollator
    {
        private bool _increaseExposure { get; set; }
        public SimpleCollator(bool increasesExposure) {
            _increaseExposure = increasesExposure;
            Results = new List<MarketResults>();
            CurrentExposure = new Dictionary<Guid, MarketExposure>();
        }

        public Dictionary<Guid, MarketExposure> CurrentExposure { get; }

        public List<MarketResults> Results { get; set; }

        public bool CanEnter(string id) {
            if (!_increaseExposure && CurrentExposure.Values.Any(z=>z.MarketName.Equals(id)))
                return false;
            return true;
        }

        public void AddTrade(Trade trade, Guid id, string marketName) {
            if (!Results.Any(x => x.MarketName.Equals(marketName)))
                Results.Add(new MarketResults(marketName));
            Results.First(x => x.MarketName.Equals(marketName)).Trades.Add(trade);
            if (CurrentExposure.ContainsKey(id))
                CurrentExposure.Remove(id);
        }

        public virtual void AddExposureItem(MarketExposure result, Guid id) {
            if (CurrentExposure.ContainsKey(id))
                CurrentExposure[id] = result;
            else
                CurrentExposure.Add(id, result);
        }
        public void RemoveExposureItem(Guid id) {
            CurrentExposure.Remove(id);
        }

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

    public readonly struct MarketExposure
    {
        public string MarketName { get; }
        public DatedResult Exposure { get; }
        public MarketExposure(string name, DatedResult exposure) {
            MarketName = name;
            Exposure = exposure;
        }

    }
}
