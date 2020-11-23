using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    public readonly struct Trade
    {
        public double [] Results { get; }
        public double Result { get; }
        public double Drawdown { get; }
        public int MarketStart { get; }
        public int MarketEnd { get; }
        public bool Win { get; }


        public Trade(double[] results, int startIndex) {
            Results = results;
            MarketStart = startIndex;
            MarketEnd = results.Length + startIndex-1;
            Result = results.Last();
            Drawdown = results.Any(x => x < 0) ? results.Where(x => x < 0).Min() : 0;
            Win = results.Last() > 0;
        }

        public int Duration => MarketEnd - MarketStart+1;
    }



    public class TradeTimeLineBuilder{
        private Stack<double> _results { get; set; }
        private int _index { get; set; }

        public void Init(int start) {
            _results = new Stack<double>();
            _index = start;
        }

        public void AddResult(double result) {
            _results.Push(result);
        }

        public Trade CompileTrade() {
            return new Trade(_results.Reverse().ToArray(), _index);
        }
    }
}