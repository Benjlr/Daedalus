using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using Logic.Utils;
using Logic.Utils.Calculations;
using PriceSeries.FinancialSeries;

namespace Logic.Metrics.EntryTests
{
    public class FixedBarExitTest : TestBase
    {
        // Fixed Bar Exit
        private int Fixed_Bar_exit { get; }

        public FixedBarExitTest(int bars_to_wait)
        {
            Fixed_Bar_exit = bars_to_wait;
        }


        public override void Run(MarketData[] data, bool[] entries, List<Session> myInputs)
        {
            FBELong = new double[data.Length];
            FBEShort = new double[data.Length];
            FBEDrawdown = new double[data.Length];
            AtrsUp = new double[data.Length];
            AtrsDown = new double[data.Length];
            var myAtrs = AverageTrueRange.Calculate(myInputs,20);

            for (int i = 80; i < entries.Length - 1; i++)
            {
                if (entries[i])
                {
                    int x = i + 1;
                    if (x + Fixed_Bar_exit >= data.Length) break;

                    double entryPriceBull = data[x].Open_Ask;
                    double entryPriceBear = data[x].Open_Bid;

                    FBELong[i] = data[x + Fixed_Bar_exit].Open_Bid*100 / entryPriceBull-100;
                    //FBELong[i] = data[x + Fixed_Bar_exit].Open_Bid - entryPriceBull;
                    FBEShort[i] = entryPriceBear - data[x + Fixed_Bar_exit].Open_Ask;


                    for (int j = x; j < x + Fixed_Bar_exit; j++)
                    {
                        //if (data[j].Low_Ask - entryPriceBull < FBEDrawdown[i]) FBEDrawdown[i] = data[j].Low_Ask - entryPriceBull;
                        if ((data[j].Low_Ask*100 / entryPriceBull)  -100 < FBEDrawdown[i]) FBEDrawdown[i] = data[j].Low_Ask *100/ entryPriceBull-100;

                        //if (listo[j].High_Ask - entryPriceBull > 5)
                        //{
                        //    FBELong[i] = 5;
                        //    break;
                        //}
                        // if (FBEDrawdown[i] < -20)
                        //{
                        //    FBELong[i] = -20;

                        //    break;

                        //}
                    }

                    AtrsDown[i] = FBEDrawdown[i] / myAtrs[i];
                    AtrsUp[i] = FBELong[i] / myAtrs[i];





                }
            }

            WinPercentageLong = (double) FBELong.Count(x => x > 0) / (double) FBELong.Count(x => Math.Abs(x) > 0);
            WinPercentageShort = (double) FBEShort.Count(x => x > 0) / (double) FBEShort.Count(x => Math.Abs(x) > 0);
            if (double.IsNaN(WinPercentageLong))
            {
                WinPercentageLong = 0;
            }


            MedianGainLong = FBELong.Any(x => x > 0) ?  FBELong.Where(x => x > 0).Median():0;
            MedianLossLong = FBELong.Any(x=>x< 0) ?  FBELong.Where(x => x < 0).Median() : 0;

            AverageGainLong = FBELong.Any(x => x > 0) ? FBELong.Where(x => x > 0).Average(): 0;
            AverageLossLong = FBELong.Any(x => x < 0) ? FBELong.Where(x => x < 0).Average() : 0;

        }
    }
}
