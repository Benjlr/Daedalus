using LinqStatistics;
using System;
using System.Diagnostics;
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

            int length = data.Length-500;

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(200, data.Length - length);

                FinalResultLong[i] = new double[length];
                FinalResultShort[i] = new double[length];

                double capitalLong = initCapital;
                double capitalShort = initCapital;

                for (int j = start; j < start + length; j++)
                {
                    FinalResultLong[i][j-start] = capitalLong;
                    FinalResultShort[i][j - start] = capitalShort;


                    if (myStrat.Entries[j] && j+1 < length)
                    {
                        var x = j + 1;
                        

                        double entryPriceBull = data[x].Open_Ask;
                        double entryPriceBear = data[x].Open_Bid;

                        var startCapLong = capitalLong;
                        var startCapShort = capitalShort;

                        FinalResultLong[i][x - start] = startCapLong;
                        FinalResultShort[i][x - start] = startCapShort;

                        x++;
                        
                        while (x-start < length && !myStrat.Exits[x-1 ])
                        {
                            startCapLong = capitalLong+ (data[x].Open_Bid - entryPriceBull) * dollarsPerPoint;
                            startCapShort = capitalShort + (entryPriceBear - data[x].Open_Ask) * dollarsPerPoint;


                            FinalResultLong[i][x - start] = startCapLong;
                            FinalResultShort[i][x - start] = startCapShort;
                            

                            x++;

                            //if ((data[x].Open_Bid - entryPriceBull)  > 10) break;
                            //else if ((data[x].Open_Bid - entryPriceBull)  < -10) break;
                        }
                        if (x-start >= length ) break;

                        capitalLong = capitalLong + (data[x].Open_Bid - entryPriceBull) * dollarsPerPoint; ;
                        capitalShort = capitalShort + (entryPriceBear - data[x].Open_Ask) * dollarsPerPoint; ;
                        FinalResultLong[i][x - start] = capitalLong;
                        FinalResultShort[i][x - start] = capitalShort;
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
                LowerBound[i] = array.Last();
                Average[i] = array.Average();
                Debug.WriteLine("Range Test: " + i.ToString());
            }
        }

        private double _initialStop = 70;
        private double _subsqStop = 10;

        public void RunLongStops(
            MarketData[] data, 
            Strategy myStrat, 
            double initCapital, 
            double dollarsPerPoint)
        {
            int length = data.Length - 300;
            //int length = 3000;

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(200, data.Length - length);

                FinalResultLong[i] = new double[length];
                FinalResultShort[i] = new double[length];

                double capitalLong = initCapital;
                double capitalShort = initCapital;

                for (int j = start; j < start + length; j++)
                {
                    FinalResultLong[i][j - start] = capitalLong;
                    FinalResultShort[i][j - start] = capitalShort;
                    
                    if (myStrat.Entries[j] && j + 1 < length)
                    {
                        var x = j + 1;
                        
                        double entryPriceBull = data[x].Open_Ask;
                        var stopLong = data[x].Low_Bid - _initialStop;

                        var startCapLong = capitalLong;
                        FinalResultLong[i][x - start] = startCapLong;
                        
                        x++;

                        var longX = x;

                        while (longX - start < length)
                        {
                            if (data[longX].Low_Bid < stopLong)
                            {
                                if (data[longX].Open_Bid < stopLong) stopLong = data[longX].Open_Bid;
                                break;
                            }
                            
                            //if (data[longX].Low_Bid - stopLong > _initialStop) stopLong = data[longX].Low_Bid - _initialStop;
                            if (myStrat.Exits[longX]) stopLong = data[longX].Low_Bid - _subsqStop;

                            startCapLong = capitalLong + (stopLong - entryPriceBull) * dollarsPerPoint;
                            FinalResultLong[i][longX - start] = startCapLong;

                            longX++;
                        }
                        if (longX - start >= FinalResultLong[i].Length - 1) break;

                        capitalLong += (stopLong - entryPriceBull) * dollarsPerPoint; ;
                        FinalResultLong[i][longX - start] = capitalLong;
                        j = longX;

                        //double entryPriceBear = data[x].Open_Bid;
                        //var stopShort = data[x].highbis + stopAmount;
                        //var startCapShort = capitalShort;
                        //FinalResultShort[i][x - start] = startCapShort;
                        //var shortX = x;

                        //while (shortX - start < length)
                        //{
                        //    if (data[shortX].High_Ask > stopShort)
                        //    {
                        //        if (data[shortX].Low_Ask > stopShort) stopShort = data[shortX].Open_Ask;
                        //        break;
                        //    }

                        //    if (stopShort - data[shortX].Open_Ask > stopAmount) stopShort = data[shortX].Open_Ask + stopAmount;

                        //    startCapShort = capitalShort + (stopShort - entryPriceBear) * dollarsPerPoint;
                        //    FinalResultShort[i][shortX - start] = startCapShort;

                        //    shortX++;
                        //}

                        //capitalShort += (entryPriceBear - stopShort) * dollarsPerPoint; ;
                        //FinalResultShort[i][shortX - start] = capitalShort;
                        //j = shortX;

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
                LowerBound[i] = array.Last();
                Average[i] = array.Average();
                Debug.WriteLine("Range Test: " + i.ToString());
            }


        }
    }
}
