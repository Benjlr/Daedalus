using System;
using System.Collections.Generic;
using System.Text;
using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;

namespace Logic.Analysis.StrategyRunners
{
    public class RunnerState
    {
        public StrategyState Portfolio { get; set; }
        public StrategyState Market { get; set; }
    }

    public class StrategyState
    {
        public TradeState InvestedState { get; set; }
        public DrillDownStats Stats { get; set; }
        public List<double> Returns { get; set; }
    }

    public class TradeState
    {
        public bool Invested { get; set; } = false;
        public double EntryPrice { get; set; }
        public double StopPrice { get; set; }
        public double TargetPrice { get; set; }
        public double Return { get; set; }

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
