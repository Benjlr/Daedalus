using System;

namespace DataStructures
{
    public abstract class PriceExitCalculator : ExitInterface
    {
        protected ExitPrices _currentExit { get; set; }
        public ExitPrices InitialExit { get; protected set; }
        public abstract ExitPrices NewExit(DatedResult trade, BidAskData[] prices, int index);
    }

    public class TrailingStopPercentage : PriceExitCalculator
    {
        private MarketSide _side { get; set; }
        private double _trailingPercentage { get; } 
        public TrailingStopPercentage(ExitPrices initialExits, double trailingPercent) {
            GetDir(initialExits);
            InitialExit = initialExits;
            _currentExit = initialExits;
            _trailingPercentage = trailingPercent;
        }

        private void GetDir(ExitPrices initialExits) {
            if (initialExits.StopPercentage > 1 )
                _side = MarketSide.Bear;
            else
                _side = MarketSide.Bull;
        }

        public override ExitPrices NewExit(DatedResult trade, BidAskData[] prices, int index) {
            switch (_side) {
                case MarketSide.Bull:
                    if ((trade.Return + 1) - _currentExit.StopPercentage > _trailingPercentage)
                        _currentExit = new ExitPrices((trade.Return + 1) - _trailingPercentage, _currentExit.TargetPercentage);
                    return _currentExit;
                case MarketSide.Bear:
                    if (1-_currentExit.StopPercentage - (trade.Return)   < -_trailingPercentage)
                        _currentExit = new ExitPrices((1-trade.Return) + _trailingPercentage, _currentExit.TargetPercentage);
                    return _currentExit;
                default: throw new Exception("What dir idiot?");
            }
        }
    }

    public class StaticStopTarget : PriceExitCalculator
    {

        public StaticStopTarget(ExitPrices initialExits) {
            InitialExit = initialExits;
            _currentExit = initialExits;
        }
        public override ExitPrices NewExit(DatedResult trade, BidAskData[] prices, int index) {
            _currentExit = InitialExit;
            return _currentExit;
        }
    }

    public interface ExitInterface
    {
        ExitPrices InitialExit { get;  }
        ExitPrices NewExit(DatedResult trade, BidAskData[] prices, int index);
    }
}