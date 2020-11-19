namespace DataStructures
{
    public readonly struct Trade
    {
        public double [] Results { get;  }
        public uint StartIndex { get;  }

        public Trade(double[] results, uint startIndex) {
            Results = results;
            StartIndex = startIndex;
        }
    }

    public class TradeTimeLineBuilder{
    }
}