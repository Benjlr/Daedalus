using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public class Portfolio : ITradeCollator
    {
        private bool _increaseExposure { get; }
        public Dictionary<Guid, MarketExposure> CurrentExposure { get; }
        public List<MarketResults> Results { get; set; }
        private Dictionary<Guid, double> _invested { get; }
        private double _maxRisk { get; }
        public double Cash { get; private set; }

        public Portfolio(double startCash, double maxRisk, bool enterSameInstrumentTwice) {
            Cash = startCash;
            _maxRisk = maxRisk;
            Results = new List<MarketResults>();
            CurrentExposure = new Dictionary<Guid, MarketExposure>();
            _invested = new Dictionary<Guid, double>();
            _increaseExposure = enterSameInstrumentTwice;
        }

        public bool CanEnter(string id) {
            if (!_increaseExposure && CurrentExposure.Values.Any(z => z.MarketName.Equals(id)))
                return false;
            //if (getInvestedCapital() > Cash)
            //    return false;
            return true;
        }

        public void AddTrade(Trade trade, Guid id, string marketName) {
            if(!Results.Any(x=>x.MarketName.Equals(marketName)))
                Results.Add(new MarketResults(marketName) );

            Results.First(x=>x.MarketName.Equals(marketName)).Trades.Add(trade);

            if (CurrentExposure.ContainsKey(id))
                CurrentExposure.Remove(id);

            addCash(id, trade.FinalResult);
        }

        public virtual void AddExposureItem(MarketExposure result, Guid id) {
            if (CurrentExposure.ContainsKey(id)) 
                CurrentExposure[id] = result;
            else {
                investCash(id);
                CurrentExposure.Add(id, result);
            }
        }
        public void RemoveExposureItem(Guid id) {
            CurrentExposure.Remove(id);
        }

        private void addCash(Guid id, double finalReturn) {
            if (!_invested.ContainsKey(id))
                investCash(id);

            Cash +=  (_invested[id]*finalReturn);
            _invested.Remove(id);
        }

        private void investCash(Guid id) {
            var totalAmnt = getCurrentPortfolioValue();
            if (totalAmnt <= 0)
                _invested.Add(id,0.0075 * Cash);
            else if(totalAmnt / 2.0 > _maxRisk)
                _invested.Add(id, _maxRisk * Cash);
            else 
                _invested.Add(id,0.02 * Cash);

        }

        private double getCurrentPortfolioValue() {
            double amnt = 0;
            foreach (var t in _invested) 
                amnt += t.Value * (1+CurrentExposure[t.Key].Exposure.Return);
            return amnt /Cash;
        }

        private double getInvestedCapital() {
            double amnt = 0;
            foreach (var t in _invested)
                amnt += t.Value;
            return amnt;
        }
    }
}