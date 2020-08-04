using System;
using System.Collections.Generic;
using System.Text;


namespace Logic.Metrics
{ 
    public class MonteCarloTests
    {
        public void Run(Strategy strat, Market market, double initCapital, double dollarsPerPoint)
        {
            double[] returnsLong = new double[market.RawData.Length];
            double[] returnsShort = new double[market.RawData.Length];

            for (int j = 0; j < market.RawData.Length; j++)
            {
                if (strat.Entries[j])
                {
                    var x = j + 1;
                    double entryPriceBull = market.RawData[x].Open_Ask;
                    double entryPriceBear = market.RawData[x].Open_Bid;
                    x++;

                    while (x < market.RawData.Length && !strat.Exits[x]) x++;
                    if (x >= market.RawData.Length - 1) break;

                    returnsLong[x] = market.RawData[x].Open_Bid - entryPriceBull;
                    returnsShort[x] = entryPriceBear - market.RawData[x].Open_Ask;

                }
            }


            var my_iterations_long = new double[500][];
            var my_iterations_short = new double[500][];

            Random t = new Random();

            for (int i = 0; i < 500; i++)
            {

                var myCapitalLong = initCapital;
                var myCapitalShort = initCapital;

                for (int j = 0; j < returnsLong.Length; j++)
                {
                    myCapitalLong += (returnsLong[j] * dollarsPerPoint);
                    my_iterations_long[i][j] = myCapitalLong;
                }

                for (int j = 0; j < returnsShort.Length; j++)
                {
                    myCapitalShort += (returnsShort[j] * dollarsPerPoint);
                    my_iterations_short[i][j] = myCapitalShort;
                }

                t.Shuffle(returnsShort);
                t.Shuffle(returnsLong);

            }

        }

    }
    static class RandomExtensions
    {
        public static void Shuffle<T>(this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                int k = rng.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}
