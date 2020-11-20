

namespace DataStructures
{
    public struct TradeState
    {
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }
    }

    public class TradeStateGenerator
    {
        private TradeTimeLineBuilder _tradeBuilder { get; set; }
        private MarketSide _isLong { get; set; }
        public TradeState CurrentState { get; private set; }
        
        private TradeStateGenerator(MarketSide longShort, int marketIndex) {
            _isLong = longShort;
            _tradeBuilder = new TradeTimeLineBuilder();
            _tradeBuilder.Init(marketIndex);

        }

        public static TradeStateGenerator Invest( ExitPrices stopTarget, MarketSide longShort, int marketIndex, double entryPrice, double closePrice) {
            TradeStateGenerator generator = new TradeStateGenerator(longShort, marketIndex);
            generator.CurrentState = new TradeState() {
                EntryPrice = entryPrice,
                StopPrice = entryPrice * stopTarget.StopPercentage,
                TargetPrice = entryPrice * stopTarget.TargetPercentage,
                Return = generator.CalculateReturn(closePrice, entryPrice)
            };
            generator._tradeBuilder.AddResult(generator.CurrentState.Return);
            return generator;
        }

        public void Continue(double closeData) {
            CurrentState = new TradeState() {
                EntryPrice = CurrentState.EntryPrice,
                StopPrice = CurrentState.StopPrice,
                TargetPrice = CurrentState.TargetPrice,
                Return = CalculateReturn(closeData, CurrentState.EntryPrice)
            };
            _tradeBuilder.AddResult(CurrentState.Return);
        }

        public void ContinueUpdateExits(double closeData, ExitPrices exitPrices) {
            CurrentState = new TradeState()
            {
                EntryPrice = CurrentState.EntryPrice,
                StopPrice = CurrentState.EntryPrice * exitPrices.StopPercentage,
                TargetPrice = CurrentState.EntryPrice * exitPrices.TargetPercentage,
                Return = CalculateReturn(closeData, CurrentState.EntryPrice)
            };
            _tradeBuilder.AddResult(CurrentState.Return);
        }

        public Trade Exit(double exitPrice) {
            _tradeBuilder.AddResult(CalculateReturn(exitPrice, CurrentState.EntryPrice));
            return _tradeBuilder.CompileTrade();
        }

        private double CalculateReturn(double current, double entry) {
            if (_isLong.Equals(MarketSide.Bull)) return (current - entry) / entry;
            else return -(current - entry) / entry;

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

        public static ExitPrices NoTarget(double stop) {
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
