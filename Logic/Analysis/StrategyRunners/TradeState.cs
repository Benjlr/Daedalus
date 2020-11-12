using PriceSeriesCore;

namespace Logic.Analysis.StrategyRunners
{

    public class TradeState
    {
        private const double _dist = 0.005;

        public bool Invested { get; set; } = false;
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }

        public TradeState InvestLong(MarketData data) {
            return new TradeState() {
                Invested = true,
                EntryPrice = data.Open_Ask,
                StopPrice = data.Open_Ask * (1 - _dist),
                TargetPrice = data.Open_Ask * (1 + _dist),
                Return = (data.Close_Bid - data.Open_Ask) / data.Open_Ask,
            };
        }
        public TradeState ContinueLong(MarketData data) {
            return new TradeState() {
                Invested = true,
                EntryPrice = this.EntryPrice,
                StopPrice = this.StopPrice,
                TargetPrice = this.TargetPrice,
                Return = (data.Close_Bid - this.EntryPrice) / this.EntryPrice,
            };
        }

        public TradeState InvestShort(MarketData data) {
            return new TradeState() {
                Invested = true,
                EntryPrice = data.Open_Bid,
                StopPrice = data.Open_Bid * (1 + _dist),
                TargetPrice = data.Open_Bid * (1 - _dist),
                Return = (data.Open_Bid - data.Close_Ask) / data.Open_Bid,
            };
        }

        public TradeState ContinueShort(MarketData data) {
            return new TradeState() {
                Invested = true,
                EntryPrice = this.EntryPrice,
                StopPrice = this.StopPrice,
                TargetPrice = this.TargetPrice,
                Return = (this.EntryPrice - data.Close_Ask) / this.EntryPrice,
            };
        }

        public TradeState DoNothing() {
            return new TradeState();
        }

    }
}
