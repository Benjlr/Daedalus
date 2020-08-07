using System;
using System.Linq;

namespace Logic.Metrics.CoreTests
{
    public class RangeTest
    {
        private int _ranges { get; }
        private int _length { get; }
        private Random _rand { get; }

        public double[][] FinalResultLong { get; private set; }
        public double[][] FinalResultShort { get; private set; }


        public RangeTest(int rangesToTest)
        {
            _ranges = rangesToTest;
            FinalResultLong = new double[_ranges][];
            FinalResultShort = new double[_ranges][];
            
            _rand = new Random();
        }

        public void Run(MarketData[] data, Strategy myStrat, double initCapital, double dollarsPerPoint)
        {

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(200, data.Length - 1500);
                var end = _rand.Next(1400, 1500);

                FinalResultLong[i] = new double[end];
                FinalResultShort[i] = new double[end];

                double capitalLong = initCapital;
                double capitalShort = initCapital;

                for (int j = start; j < start + end; j++)
                {
                    FinalResultLong[i][j-start] = capitalLong;
                    FinalResultShort[i][j - start] = capitalShort;


                    if (myStrat.Entries[j])
                    {
                        var x = j + 1;
                        double entryPriceBull = data[x].Open_Ask;
                        double entryPriceBear = data[x].Open_Bid;
                        FinalResultLong[i][x-start] = capitalLong;
                        FinalResultShort[i][x - start] = capitalShort;

                        x++;

                        while (x < data.Length && !myStrat.Exits[x ])
                        {
                            capitalLong += (data[x].Open_Bid - entryPriceBull) * dollarsPerPoint;
                            capitalShort += (entryPriceBear - data[x].Open_Ask) * dollarsPerPoint;
                            FinalResultLong[i][x - start] = capitalLong;
                            FinalResultShort[i][x - start] = capitalShort;
                            x++;
                        }
                        if (x >= data.Length - 1) break;

                        j = x;
                    }
                }
            }
        }
    }
}
