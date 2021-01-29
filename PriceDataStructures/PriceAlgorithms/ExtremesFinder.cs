namespace DataStructures.PriceAlgorithms
{
    public class ExtremesFinder
    {
        private bool _mirror { get; }
        private int _consecutiveThreshold { get; }
        private int _neighoursPermitted { get; }


        private int consecutiveCount = 0;
        private int neighbourCount = 0;
        private int neighbourReset = 0;
        private double lastValue = double.MinValue;

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
            consecutiveCount++;
            neighbourReset++;
            if (neighbourReset >= _consecutiveThreshold)
                neighbourCount = 0;
        }

        private void CheckReset(double currentValue) {
            neighbourCount++;
            neighbourReset = 0;
            if (neighbourCount <= _neighoursPermitted)
                return;
            lastValue = currentValue;
            consecutiveCount = 0;
            neighbourCount = 0;
        }
    }
}
