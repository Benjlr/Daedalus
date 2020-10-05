using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqStatistics;
using Logic.Utils;


namespace Logic.Metrics
{
    public class MonteCarloTest
    {
        public double[][] LongIterations { get; private set; }
        public double[][] ShortIterations { get; private set; }

        public double[] UpperBound { get; private set; }
        public double[] UpperQuartile { get; private set; }
        public double[] LowerQuartile { get; private set; }
        public double[] LowerBound { get; private set; }
        public double[] Average { get; private set; }
        public double[] Median { get; private set; }

        private static Random _rand;

        public void Run(Strategy strat, Market market, double initCapital, double dollarsPerPoint, int iterations)
        {
            List<double> returnsLong = new List<double>();
            List<double> ddura = new List<double>();
            List<double> returnsShort = new List<double>();



            for (int j = 0; j < market.RawData.Length; j++)
            {
                if (strat.Entries[j])
                {
                    var x = j + 1;
                    if (x >= market.RawData.Length) continue;

                    double entryPriceBull = market.RawData[x].Open_Ask;
                    double entryPriceBear = market.RawData[x].Open_Bid;
                    var counts = 0;
                    x++;

                    while (x < market.RawData.Length && !strat.Exits[x])
                    {
                        //if (x - j + 1 > 350) break;


                        //if ((market.RawData[x].Open_Bid - entryPriceBull)  > 10) break;
                        //if ((market.RawData[x].Open_Bid - entryPriceBull) < -10) break;
                        counts++;
                        x++;
                    }
                    x++;

                    if (x >= market.RawData.Length) continue;
                    
                    returnsLong.Add( market.RawData[x].Open_Bid - entryPriceBull);
                    returnsShort.Add(entryPriceBear - market.RawData[x].Open_Ask);
                    ddura.Add(counts);
                    counts = 0;
                    j = x;
                }
            }
            
            LongIterations = new double[iterations][];
            ShortIterations = new double[iterations][];
            
            File.WriteAllLines(@"C:\Temp\Rets.csv", returnsLong.Select(x=>x.ToString()).ToList());
            File.WriteAllLines(@"C:\Temp\surs.csv", ddura.Select(x=>x.ToString()).ToList());

            var shortAvg = returnsShort.Average();
            var longAvg = returnsLong.Average();
            var stDevLong = returnsLong.StandardDeviation();
            var stDevShort = returnsShort.StandardDeviation();

            _rand = new Random();

            var span = market.RawData.Last().Time.Ticks - market.RawData[0].Time.Ticks;
            var pcntgeOfYeaar = new TimeSpan(span).TotalDays / 365.0;
            int count = (int)Math.Round(returnsLong.Count / pcntgeOfYeaar,MidpointRounding.AwayFromZero) ;

            for (int i = 0; i < iterations; i++)
            {
                var myCapitalLong = initCapital;
                var myCapitalShort = initCapital;

                LongIterations[i] = new double[count];
                ShortIterations[i] = new double[count];

                for (int j = 0; j < count; j++)
                {
                    //if (myCapitalLong > 0) myCapitalLong += (BoxMullerDistribution.Generate(longAvg,stDevLong) * dollarsPerPoint);
                    if (myCapitalLong > 0) myCapitalLong += returnsLong[_rand.Next(returnsLong.Count)]*dollarsPerPoint;
                    if (myCapitalLong < 0) myCapitalLong = 0;

                    LongIterations[i][j] = myCapitalLong;
                }

                for (int j = 0; j < count; j++)
                {
                    if (myCapitalShort > 0) myCapitalShort += (BoxMullerDistribution.Generate(shortAvg,stDevShort) * dollarsPerPoint);
                    if (myCapitalShort < 0) myCapitalShort = 0;

                    ShortIterations[i][j] = myCapitalShort;
                }
            }

            UpperBound = new double[count];
            LowerBound = new double[count];
            UpperQuartile = new double[count];
            LowerQuartile= new double[count];
            Average= new double[count];
            Median= new double[count];

            for (int i = 0; i < count; i++)
            {
                var arraySlice = new double[iterations];

                for (int j = 0; j < iterations; j++) arraySlice[j] = LongIterations[j][i];

                arraySlice = arraySlice.OrderByDescending(x=>x).ToArray();
                UpperBound[i] = arraySlice[0];
                UpperQuartile[i] = arraySlice[(int) (iterations / 4.0)];
                Median[i] = arraySlice.Median();
                LowerQuartile[i] = arraySlice[(int) (3.0*iterations / 4.0)];
                LowerBound [i] = arraySlice[iterations-1];
                Average[i] = arraySlice.Average();
                Debug.WriteLine("Monte Carlo Test: " + i);

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
        public static void Shuffle<T>(this Random rng, List<T> array)
        {
            int n = array.Count;
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
