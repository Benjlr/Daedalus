namespace Logic.Analysis.StrategyRunners
{
    public class RunnerState
    {
        public StrategyState Portfolio { get; set; }
        public StrategyState Market { get; set; }
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
