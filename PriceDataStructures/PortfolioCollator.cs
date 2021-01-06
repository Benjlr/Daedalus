using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures.StatsTools;

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
            //if (CurrentExposure.Count > 10)
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
        //        IMHO the amount to bet is the smaller of two numbers:

        //          1) The amount you are prepared to lose.
        //          2) Your expected trade's profit, divided by the variance of your trade's profits, 
        //             times your account size, divided by 2 (or 4 if you are more conservative).
        //          
        //Eg if for every $1 I put at risk, my expected pay off is $0.05 and the standard 
        //          deviation of payoffs is $1 and my account is $100,000, then I should be betting 1/2 * 0.05 / 1^2 * 100,000 = $2500 per trade(or 2.5%).
        //          If your account gets larger or smaller, the amount you bet will also change proportionally.
        //          Eg if my account builds to $200,000 while still maintaining the same edge, I will be betting $5000 per trade (still 2.5%).
        //          For those interested in the maths, it is a specific implementation of the Kelly Optimal f.
        //          This is usually a good equation for optimal trade sizing.If you don't believe me, try it in Excel. It works better
        //          when trade returns are normally distributed (they aren't but it is still a decent approximation, 
        //even when skewed or fat tails). The problem most people fall into is overestimating their future edge.
        private void investCash(Guid id) {
            var totalAmnt = getCurrentPortfolioValue();
            //new TradeStatistics(this.Results.SelectMany(x => x.Trades.Select(v => v.FinalResult)).ToList()).AverageExpectancy > 0
            var returnVal = CurrentExposure.Values.Sum(c => c.Exposure.Return);

            //if (returnVal > 0)
                _invested.Add(id, _maxRisk * Cash);
            //else 
            //    _invested.Add(id,0 );

            //if (totalAmnt <= 0)
            //    _invested.Add(id,0.0075 * Cash);
            //else if(totalAmnt / 2.0 > _maxRisk)
            //    _invested.Add(id, _maxRisk * Cash);
            //else 
            //    _invested.Add(id,0.02 * Cash);

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