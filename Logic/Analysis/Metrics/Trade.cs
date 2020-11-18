namespace Logic.Analysis.Metrics
{
    public readonly struct Trade
    {
        public double [] Results { get;  }
        public int StartIndex { get;  }

        public Trade(double[] results, int startIndex) {
            Results = results;
            StartIndex = startIndex;
        }
    }

    public class TradeTimeLineBuilder{
        public 
    }
}