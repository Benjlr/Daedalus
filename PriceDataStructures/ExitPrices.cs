namespace DataStructures
{
    public readonly struct ExitPrices
    {
        public double StopPercentage { get; }
        public double TargetPercentage { get; }

        public ExitPrices(double stop, double target) {
            StopPercentage = stop;
            TargetPercentage = target;
        }

        public static ExitPrices StopOnly(double stop) {
            return new ExitPrices(stop, double.NaN);
        }

        public static ExitPrices TargetOnly(double target) {
            return new ExitPrices(double.NaN, target);
        }
        public static ExitPrices NoStopTarget() {
            return new ExitPrices(double.NaN, double.NaN);
        }
    }
}