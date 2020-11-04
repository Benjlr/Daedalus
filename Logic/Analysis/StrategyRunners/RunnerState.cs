using Logic.Metrics.EntryTests.TestsDrillDown;
using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Analysis.StrategyRunners
{
    public class RunnerState
    {
        public StrategyState Portfolio { get; set; }
        public StrategyState Market { get; set; }
    }

    public class StrategyState
    {
        public TradeState Actionition { get; set; }
        public DrillDownStats Stats { get; set; }
        public double Return { get; set; }
    }

    public class TradeState
    {
        public bool Invested { get; set; }
        public double EntryPrice { get; set; }
    }
}
