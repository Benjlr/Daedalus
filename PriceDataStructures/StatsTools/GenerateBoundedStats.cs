using System.Collections.Generic;
using System.Linq;
using LinqStatistics;

namespace DataStructures.StatsTools
{
    public class GenerateBoundedStats
    {
        public static List<BoundedStat> Generate(List<List<double>> tests) {
            var retVal = new List<BoundedStat>();
            for (int i = 0; i < tests[0].Count; i++) 
                retVal.Add(new BoundedStat(tests.Select(x => x[i] ).ToList(),0.65));

            return retVal;
        }
    }

    public class BoundedStat
    {
        public double Maximum { get; set; }
        public double Upper { get; set; }
        public double Median { get; set; }
        public double Average { get; set; }
        public double Lower { get; set; }
        public double Minimum { get; set; }

        public BoundedStat(List<double> input, double breadth) {
            if (IsValidList(ref input)) {
                GenerateMaxMin(input);
                AverageAndMedian(input);
                GenerateBounds(input, breadth);
            }
        }

        private bool IsValidList(ref List<double> input) {
            input = input.OrderBy(x => x).ToList();
            return input.Count > 0;
        }

        private void GenerateMaxMin(List<double> input) {
            Maximum = input.Last();
            Minimum = input.First();
        }

        private void AverageAndMedian(List<double> input) {
            Median = input.Median();
            Average = input.Average();
        }

        private void GenerateBounds(List<double> input, double breadth) {
            int lowerIndex = (int)(input.Count * ((1 - breadth)/2.0));
            int upperIndex = (int)(input.Count * (breadth + ((1 - breadth) / 2.0)));

            Lower = input[lowerIndex];
            Upper = input[upperIndex];
        }
    }
}
