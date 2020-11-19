using System.Collections.Generic;

namespace DataStructures.PriceAlgorithms
{
    public class NRWRBars
    {
        public static List<int> Calculate(List<SessionData> input) {
            var retval = new List<int>();
            input.ForEach(x => retval.Add(0));

            for (var i = 1; i < input.Count; i++) {
                var range = 0;
                var counter = i - 1;

                var todaysRange = input[i].High - input[i].Low;
                var yesterdaysRange = input[counter].High - input[counter].Low;


                if (todaysRange > yesterdaysRange) {
                    while (todaysRange > yesterdaysRange) {
                        range++;
                        counter--;
                        if (counter < 1) break;
                        yesterdaysRange = input[counter].High - input[counter].Low;
                        retval[i] = range;
                    }

                }
                else {
                    while (todaysRange < yesterdaysRange) {
                        range--;
                        counter--;
                        if (counter < 1) break;
                        yesterdaysRange = input[counter].High - input[counter].Low;
                        retval[i] = range;
                    }
                }

            }

            return retval;

        }
    }
}
