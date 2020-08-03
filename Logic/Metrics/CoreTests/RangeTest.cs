using System;
using System.Linq;

namespace Logic.Metrics.CoreTests
{
    public class RangeTest
    {
        private int _ranges { get; }
        private int _length { get; }
        private Random _rand { get; }

        public double[] FinalResultLong { get; private set; }
        public double[] WinRatioLong { get; private set; }
        public double[] FinalResultShort { get; private set; }
        public double[] WinRatioShort { get; private set; }


        public RangeTest(int rangesToTest)
        {
            _ranges = rangesToTest;
            FinalResultLong = new double[_ranges];
            FinalResultShort = new double[_ranges];
            WinRatioLong = new double[_ranges];
            WinRatioShort = new double[_ranges];
            _rand = new Random();
        }

        public void Run(MarketData[] data, Strategy myStrat)
        {

            for (int i = 0; i < _ranges; i++)
            {
                var start = _rand.Next(0, data.Length - 310);
                var end = _rand.Next(260, 310);

                double capitalLong = 0;
                double capitalShort = 0;

                int totalTrades = 0;
                int totalWinsLong = 0;
                int totalWinsShort = 0;

                for (int j = start; j < start + end; j++)
                {
                    if (myStrat.Entries[j])
                    {
                        var x = j + 1;
                        double entryPriceBull = data[x].Open_Ask;
                        double entryPriceBear = data[x].Open_Bid;
                        x++;

                        while (x < data.Length && !myStrat.Exits[x]) x++;
                        if (x >= data.Length - 1) break;

                        totalTrades++;
                        if (data[x].Open_Bid > entryPriceBull) totalWinsLong++;
                        if (entryPriceBear > data[x].Open_Ask) totalWinsShort++;
                        capitalLong += data[x].Open_Bid - entryPriceBull;
                        capitalShort += entryPriceBear - data[x].Open_Ask;

                    }
                }

                FinalResultLong[i] = capitalLong;
                FinalResultShort[i] = capitalShort;
                WinRatioLong[i] = (double)totalWinsLong / totalTrades;
                WinRatioShort[i] = (double)totalWinsShort / totalTrades;
            }

            FinalResultLong = FinalResultLong.OrderBy(x => x).ToArray();
            FinalResultShort = FinalResultShort.OrderBy(x => x).ToArray();
            WinRatioLong = WinRatioLong.OrderBy(x => x).ToArray();
            WinRatioShort = WinRatioShort.OrderBy(x => x).ToArray();


        }
    }
}
