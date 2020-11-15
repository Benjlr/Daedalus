using PriceSeriesCore;

namespace Logic.Analysis.StrategyRunners
{

    public class TradeState
    {
        public bool Invested { get; set; } 
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }
        private bool _isLong { get; set; }

        public TradeState Invest(MarketData data, ExitPrices stopTarget, bool isLong) {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = isLong ? data.Open_Ask : data.Open_Bid,
                StopPrice = isLong ? data.Open_Ask * stopTarget.StopPercentage : data.Open_Bid * stopTarget.StopPercentage,
                TargetPrice = isLong ? data.Open_Ask * stopTarget.TargetPercentage : data.Open_Bid * stopTarget.TargetPercentage,
                Return = isLong ? CalculateReturnLong(data.Close_Bid, data.Open_Ask) : CalculateReturnShort(data.Close_Ask, data.Open_Bid),
                _isLong = isLong
            };
        }
        public TradeState Continue(MarketData data) {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = this.EntryPrice,
                StopPrice = this.StopPrice,
                TargetPrice = this.TargetPrice,
                Return = _isLong ? CalculateReturnLong(data.Close_Bid, this.EntryPrice) : 
                    CalculateReturnShort(data.Close_Ask, this.EntryPrice),

            };
        }
        public TradeState ContinueUpdateExits(MarketData data, ExitPrices exitPrices) {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = this.EntryPrice,
                StopPrice = this.EntryPrice * exitPrices.StopPercentage,
                TargetPrice = this.EntryPrice * exitPrices.TargetPercentage,
                Return = _isLong ? CalculateReturnLong(data.Close_Bid, this.EntryPrice) : 
                    CalculateReturnShort(data.Close_Ask, this.EntryPrice),

            };
        }
        private double CalculateReturnLong(double current, double entry) {
            return (current - entry) / entry;
        }

        private double CalculateReturnShort(double current, double entry) {
            return (entry - current) / entry;
        }

        public TradeState DoNothing() {
            return new TradeState();
        }

    }

    public struct ExitPrices
    {
        public double StopPercentage { get; set; }
        public double TargetPercentage { get; set; }

        public ExitPrices(double stop, double target) {
            StopPercentage = stop;
            TargetPercentage = target;
        }

        public static ExitPrices NoTarget(double stop)
        {
            return new ExitPrices(stop, double.NaN);
        }

        public static ExitPrices NoStop(double target) {
            return new ExitPrices(double.NaN, target);
        }
        public static ExitPrices NoStopTarget() {
            return new ExitPrices(double.NaN, double.NaN);
        }
    }
}
