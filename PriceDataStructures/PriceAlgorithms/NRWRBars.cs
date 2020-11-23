using System.Collections.Generic;

namespace DataStructures.PriceAlgorithms
{
    public class NRWRBars
    {
        public static List<int> Calculate(List<SessionData> input) {
            var retval = new List<int>();
            var ranges =new List<double>();
            input.ForEach(x => ranges.Add(x.High - x.Low));
            input.ForEach(x => retval.Add(0));

            for (var i = 1; i < input.Count; i++) {
                if (ranges[i] < ranges[i - 1]) 
                    for (int j = i-1; j >= 0; j--) {
                        retval[i]--;
                        if (ranges[j] <= ranges[i]) break;
                    }
                else if (ranges[i] > ranges[i - 1]) 
                    for (int j = i-1; j >= 0; j--) {
                        retval[i]++;
                        if (ranges[j] >= ranges[i]) break;
                    }
            }

            return retval;

        }
    }
}
