using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using LinqStatistics;
using Logic.Calculations;
using PriceSeries.FinancialSeries;

namespace Logic.Metrics.CoreTests
{
    public class RangeTest
    {
        private int _ranges { get; }
        private int _length { get; }
        private Random _rand { get; }

        public double[][] FinalResultLong { get; private set; }
        public double[][] FinalResultShort { get; private set; }
        
        public double[] UpperBound { get; private set; }
        public double[] UpperQuartile { get; private set; }
        public double[] LowerQuartile { get; private set; }
        public double[] LowerBound { get; private set; }
        public double[] Average { get; private set; }
        public double[] Median { get; private set; }

        public RangeTest(int rangesToTest)
        {
            _ranges = rangesToTest;
            FinalResultLong = new double[_ranges][];
            FinalResultShort = new double[_ranges][];
            
            _rand = new Random();
        }

        public void Run(MarketData[] data, Strategy myStrat, double initCapital, double dollarsPerPoint)
        {
            int length = 6000;

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(200, data.Length - length);

                FinalResultLong[i] = new double[length];
                FinalResultShort[i] = new double[length];

                double capitalLong = initCapital;
                double capitalShort = initCapital;

                for (int j = start; j < start + length-1; j++)
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

                        while (x-start < length && !myStrat.Exits[x ])
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

            UpperBound = new double[length];
            LowerBound = new double[length];
            UpperQuartile = new double[length];
            LowerQuartile = new double[length];
            Average = new double[length];
            Median = new double[length];
            for (int i = 0; i < length; ++i)
            {
                double[] numArray = new double[_ranges];
                for (int x = 0; x < _ranges; ++x) numArray[x] = FinalResultLong[x][i];

                var array = numArray.OrderByDescending(x => x).ToArray();
                UpperBound[i] = array[0];
                UpperQuartile[i] = array[(int)(_ranges / 4.0)];
                Median[i] = array.Median();
                LowerQuartile[i] = array[(int)(3.0 * _ranges / 4.0)];
                LowerBound[i] = array[_ranges - 1];
                Average[i] = array.Average();
                Debug.WriteLine("Range Test: " + i.ToString());
            }
        }

        private double _initialStop = 1;
        private double _subsqStop = 0.5;

        public void RunLongStops(MarketData[] data, List<Session> sessions, Strategy myStrat, double initCapital, double dollarsPerPoint)
        {
            var atr = AverageTrueRange.Calculate(sessions);

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(200, data.Length - 1500);
                var end = _rand.Next(1400, 1500);

                FinalResultLong[i] = new double[end];
                double capitalLong = initCapital;

                for (int j = start; j < start + end; j++)
                {
                    FinalResultLong[i][j - start] = capitalLong;


                    if (myStrat.Entries[j])
                    {
                        var x = j + 1;
                        double entryPriceBull = data[x].Open_Ask;
                        double stop = entryPriceBull - (_initialStop * atr[x]);
                        FinalResultLong[i][x - start] = capitalLong;

                        for (int k = x; k < start+end; k++)
                        {
                            //if()
                        }

                        while (x < data.Length && !myStrat.Exits[x])
                        {
                            capitalLong += (data[x].Open_Bid - entryPriceBull) * dollarsPerPoint;
                            FinalResultLong[i][x - start] = capitalLong;
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
