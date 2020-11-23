using System.Collections.Generic;
using System.Linq;
using LinqStatistics;

namespace DataStructures.PriceAlgorithms
{
    public class SigmaSpike
    {
        public static List<double> Calculate(List<double> input, int period = 20) {
            var retval = new List<double>();
            var returnSeries = new List<double> (){0};
            for (int i = 1; i < input.Count; i++)
                returnSeries.Add((input[i] / input[i-1])-1);
            
            for (var i = 0; i < period-1  && i < input.Count; i++) retval.Add(0);
            for (var i = period-1 ; i < input.Count; i++) {
                var range = returnSeries.GetRange(i - (period-1 ), period-1).ToList();
                retval.Add(returnSeries[i] / range.StandardDeviation());
            }

            return retval;
        }
    }
}
