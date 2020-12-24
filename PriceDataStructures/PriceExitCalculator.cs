namespace DataStructures
{
    public abstract class PriceExitCalculator : ExitInterface
    {
        protected ExitPrices _currentExit { get; set; }
        public ExitPrices InitialExit { get; protected set; }
        public abstract ExitPrices NewExit(Trade trade, BidAskData[] prices, int index);


    }

    public class TrailingStopPercentage : PriceExitCalculator
    {
        private double _trailingPercentage { get; } 
        public TrailingStopPercentage(ExitPrices initialExits, double trailingPercent) {
            InitialExit = initialExits;
            _currentExit = initialExits;
            _trailingPercentage = trailingPercent;
        }
        public override ExitPrices NewExit(Trade trade, BidAskData[] prices, int index) {
            if((trade.FinalResult+1) - _currentExit.StopPercentage > _trailingPercentage)
                _currentExit = new ExitPrices((trade.FinalResult + 1) - _trailingPercentage, _currentExit.TargetPercentage); 
            return _currentExit;
        }
    }

    public class StaticStopTarget : PriceExitCalculator
    {

        public StaticStopTarget(ExitPrices initialExits) {
            InitialExit = initialExits;
            _currentExit = initialExits;
        }
        public override ExitPrices NewExit(Trade trade, BidAskData[] prices, int index) {
            _currentExit = InitialExit;
            return _currentExit;
        }
    }

    public interface ExitInterface
    {
        ExitPrices InitialExit { get;  }
        ExitPrices NewExit(Trade trade, BidAskData[] prices, int index);
    }
}