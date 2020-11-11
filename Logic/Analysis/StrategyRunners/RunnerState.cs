namespace Logic.Analysis.StrategyRunners
{
    public class RunnerState
    {
        public StrategyState Portfolio { get; set; }
        public StrategyState Market { get; set; }

        public RunnerState()
        {
            Portfolio = new StrategyState();
            Market = new StrategyState();
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
