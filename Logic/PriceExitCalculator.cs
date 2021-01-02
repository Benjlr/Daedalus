using System;
using DataStructures;

namespace Logic
{
    public abstract class PriceExitCalculator : ExitInterface
    {
        public ExitPrices InitialExit { get; protected set; }
        public abstract ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index);
    }

    public class TrailingStopPercentage : PriceExitCalculator
    {
        private MarketSide _side { get; set; }
        private double _trailingPercentage { get; } 
        public TrailingStopPercentage(ExitPrices initialExits, double trailingPercent) {
            GetDir(initialExits);
            InitialExit = initialExits;
            _trailingPercentage = trailingPercent;
        }

        private void GetDir(ExitPrices initialExits) {
            if (initialExits.StopPercentage > 1 )
                _side = MarketSide.Bear;
            else
                _side = MarketSide.Bull;
        }

        public override ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index) {
            switch (_side) {
                case MarketSide.Bull:
                    if ((trade.Return + 1) - currentExit.StopPercentage > _trailingPercentage && (trade.Return + 1) - _trailingPercentage > currentExit.StopPercentage) 
                        currentExit = new ExitPrices((trade.Return + 1) - _trailingPercentage, currentExit.TargetPercentage);
                    return currentExit;
                case MarketSide.Bear:
                    if (1- currentExit.StopPercentage - (trade.Return)   < -_trailingPercentage)
                        currentExit = new ExitPrices((1-trade.Return) + _trailingPercentage, currentExit.TargetPercentage);
                    return currentExit;
                default: throw new Exception("What dir idiot?");
            }
        }
    }

    public class VariableTrailingStopPercentage : PriceExitCalculator
    {
        private MarketSide _side { get; set; }
        private double _trailingPercentage { get; }
        public VariableTrailingStopPercentage(ExitPrices initialExits, double trailingPercent) {
            GetDir(initialExits);
            InitialExit = initialExits;
            _trailingPercentage = trailingPercent;
        }

        private void GetDir(ExitPrices initialExits) {
            if (initialExits.StopPercentage > 1)
                _side = MarketSide.Bear;
            else
                _side = MarketSide.Bull;
        }

        public override ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index) {
            switch (_side) {
                case MarketSide.Bull:
                    var variableTrailing = (1 - (trade.Return/(currentExit.TargetPercentage - 1))) * _trailingPercentage;
                    if ((trade.Return + 1) - currentExit.StopPercentage > variableTrailing && (trade.Return + 1) - variableTrailing > currentExit.StopPercentage) 
                        currentExit = new ExitPrices((trade.Return + 1) - variableTrailing, currentExit.TargetPercentage);
                    return currentExit;
                case MarketSide.Bear:
                    variableTrailing = (1 + (trade.Return / (currentExit.TargetPercentage - 1))) * _trailingPercentage;
                    if (currentExit.StopPercentage - (1-trade.Return )  > variableTrailing && (1-trade.Return) + variableTrailing < currentExit.StopPercentage)
                        currentExit = new ExitPrices((1-trade.Return ) + variableTrailing, currentExit.TargetPercentage);
                    return currentExit;
                default: throw new Exception("What dir idiot?");
            }
        }
    }

    public class StaticStopTarget : PriceExitCalculator
    {

        public StaticStopTarget(ExitPrices initialExits) {
            InitialExit = initialExits;
        }
        public override ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index) {
            return currentExit;
        }
    }

    public class TimedExit : PriceExitCalculator
    {
        private int _interval { get; set; }
        private int _index { get; set; }
        private MarketSide _side { get; set; }

        public TimedExit(ExitPrices initialExits, MarketSide dir, int interval, int startIndex) {
            InitialExit = initialExits;
            _interval = interval;
            _side = dir;
        }
        public override ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index) {
            if (index >= _interval-1) {
                switch (_side) {
                    case MarketSide.Bull:
                        return new ExitPrices(double.MaxValue,double.MaxValue);
                    case MarketSide.Bear:
                        return new ExitPrices(0, 0);
                }
            }
            return currentExit;
        }
    }


    public interface ExitInterface
    {
        ExitPrices InitialExit { get;  }
        ExitPrices NewExit(DatedResult trade, ExitPrices currentExit, BidAskData[] prices, int index);
    }
}