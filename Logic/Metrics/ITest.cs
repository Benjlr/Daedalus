namespace Logic.Metrics
{
    public interface ITest
    {
        double AverageGainLong { get; }
        double AverageGainShort { get; }
        double AverageLossLong { get; }
        double AverageLossShort { get; }
        double WinPercentageLong { get; }
        double WinPercentageShort { get; }
        double ExpectancyLong { get; }
        double ExpectancyShort { get; }

        void Run(MarketData[] data, bool[] entries);
    }
}
