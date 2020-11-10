using System;
using System.Collections.Generic;
using System.Text;
using Logic.Utils;

namespace Logic.Analysis.StrategyRunners
{
    public class RunnerState
    {
        public StrategyState Portfolio { get; set; }
        public StrategyState Market { get; set; }
    }

    public class StrategyState
    {
        public TradeState InvestedState { get; private set; }
        public DrillDownStats Stats { get; private set; }
        public List<double> Returns { get; private set; }

        public StrategyState BuildNextState(MarketData data, StrategyOptions options, bool isEntry)
        {
            if (InvestedState.Invested)
                return LastStateWasInvested(data);
            else
                return LastStateWasNotInvested(data,isEntry);
        }

        private StrategyState LastStateWasInvested( MarketData data)
        {
            return new StrategyState();
            //if (prevState)
        }
        private StrategyState LastStateWasNotInvested( MarketData data, bool isEntry)
        {
            return new StrategyState();

            //if (prevState)
        }
    }


    public class TradeState
    {
        public bool Invested { get; set; } = false;
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }

        public static TradeState InvestLong(MarketData data)
        {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = data.Open_Ask,
                StopPrice = data.Open_Ask * (1 - 0.005),
                TargetPrice = data.Open_Ask * (1 + 0.005),
                Return = (data.Open_Bid - data.Open_Ask) / data.Open_Ask,
            };
        }
        public static TradeState ContinueLong(MarketData data, TradeState Entry)
        {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = Entry.EntryPrice,
                StopPrice = Entry.StopPrice,
                TargetPrice = Entry.TargetPrice,
                Return = (data.Open_Bid - Entry.EntryPrice) / Entry.EntryPrice,
            };
        }

        public static TradeState InvestShort(MarketData data)
        {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = data.Open_Bid,
                StopPrice = data.Open_Bid * (1 + 0.005),
                TargetPrice = data.Open_Bid * (1 - 0.005),
                Return = (data.Open_Ask - data.Open_Bid) / data.Open_Bid,
            };
        }

        public static TradeState ContinueShort(MarketData data, TradeState Entry)
        {
            return new TradeState()
            {
                Invested = true,
                EntryPrice = Entry.EntryPrice,
                StopPrice = Entry.StopPrice,
                TargetPrice = Entry.TargetPrice,
                Return = (data.Open_Ask - Entry.EntryPrice) / Entry.EntryPrice,
            };
        }

        public static TradeState DoNothing()
        {
            return new TradeState();
        }

    }

    public interface StopGenerator
    {
        public double GenerateStop();
    }
    public interface TargetGenerator
    {
        public double GenerateTarget();
    }
}
