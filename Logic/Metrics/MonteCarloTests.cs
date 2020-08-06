using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LinqStatistics;


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

                    while (x < market.RawData.Length-1 && !strat.Exits[x-1]) x++;
                    
                    if (x >= market.RawData.Length - 1) break;
                    
                    returnsLong[x] = market.RawData[x].Open_Bid - entryPriceBull;
                    returnsShort[x] = entryPriceBear - market.RawData[x].Open_Ask;

                    

                    j = x;
                }
            }


            LongIterations = new double[iterations][];
            ShortIterations = new double[iterations][];

            _rand = new Random();

            for (int i = 0; i < iterations; i++)
            {

                _rand.Shuffle(returnsShort);
                _rand.Shuffle(returnsLong);

                var myCapitalLong = initCapital;
                var myCapitalShort = initCapital;

                LongIterations[i] = new double[returnsLong.Length];
                ShortIterations[i] = new double[returnsShort.Length];

                for (int j = 0; j < returnsLong.Length; j++)
                {
                    if (myCapitalLong > 0) myCapitalLong += (returnsLong[j] * dollarsPerPoint);
                    if (myCapitalLong < 0) myCapitalLong = 0;

                    LongIterations[i][j] = myCapitalLong;
                }

                for (int j = 0; j < returnsShort.Length; j++)
                {
                    if (myCapitalShort > 0) myCapitalShort += (returnsShort[j] * dollarsPerPoint);
                    if (myCapitalShort < 0) myCapitalShort = 0;

                    ShortIterations[i][j] = myCapitalShort;
                }
            }

            UpperBound = new double[returnsLong.Length];
            LowerBound = new double[returnsLong.Length];
            UpperQuartile = new double[returnsLong.Length];
            LowerQuartile= new double[returnsLong.Length];
            Average= new double[returnsLong.Length];
            Median= new double[returnsLong.Length];

            for (int i = 0; i < returnsLong.Length; i++)
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

                Console.WriteLine(i);

            }


            //var parallelExec = Parallel.For(0, iterations, (x) =>
            //{
            //    double[] cloneShort = (double[])returnsShort.Clone();
            //    double[] cloneLong = (double[])returnsLong.Clone();

            //    _rand.Shuffle(cloneShort);
            //    _rand.Shuffle(cloneLong);

            //    var myCapitalLong = initCapital;
            //    var myCapitalShort = initCapital;

            //    LongIterations[x] = new double[cloneLong.Length];
            //    ShortIterations[x] = new double[cloneShort.Length];

            //    for (int j = 0; j < cloneLong.Length; j++)
            //    {
            //        if (myCapitalLong > 0) myCapitalLong += (cloneLong[j] * dollarsPerPoint);
            //        if (myCapitalLong < 0) myCapitalLong = 0;

            //        LongIterations[x][j] = myCapitalLong;
            //    }

            //    for (int j = 0; j < cloneShort.Length; j++)
            //    {
            //        if (myCapitalShort > 0) myCapitalShort += (cloneShort[j] * dollarsPerPoint);
            //        if (myCapitalShort < 0) myCapitalShort = 0;

            //        ShortIterations[x][j] = myCapitalShort;
            //    }

            //});

            //while (!parallelExec.IsCompleted)
            //{
            //    Thread.Sleep(5);
            //}

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
