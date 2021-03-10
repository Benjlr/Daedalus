namespace DataStructures.PriceAlgorithms
{
    public class ExtremesFinder
    {
        private bool _mirror { get; }
        private int _consecutiveThreshold { get; }
        private int _neighoursPermitted { get; }


        private int consecutiveCount { get; set; }
        private int neighbourCount { get; set; }
        private int neighbourReset { get; set; }
        private double lastValue { get; set; } = double.MinValue;

        public ExtremesFinder(bool mirror, int consecutivesThreshold, int neighoursPermitted) {
            _mirror = mirror;
            _consecutiveThreshold = consecutivesThreshold;
            _neighoursPermitted = neighoursPermitted;
        }

        public bool CheckExtreme(double currentValue) {
            if (_mirror)
                currentValue = -currentValue;

            if (currentValue > lastValue) {
                ExtremeActions(currentValue);
                return consecutiveCount >= _consecutiveThreshold;
            }

            CheckReset(currentValue);
            return false;
        }

        private void ExtremeActions(double currentValue) {
            lastValue = currentValue;
            consecutiveCount+=1;
            neighbourReset+=1;
            if (neighbourReset >= _consecutiveThreshold)
                neighbourCount = 0;
        }

        private void CheckReset(double currentValue) {
            neighbourCount+=1;
            neighbourReset = 0;
            if (neighbourCount <= _neighoursPermitted)
                return;
            lastValue = currentValue;
            consecutiveCount = 0;
            neighbourCount = 0;
        }
    }
}
