using System;
using System.Collections.Generic;
using PriceSeriesCore.FinancialSeries;

namespace Logic.Metrics.EntryTests
{
    public class FixedStopTargetExitTest : TestBase
    {
        //Fixed Stop and Target Exit
        private bool _usePercentage { get; }
        public double TargetDistance { get; }
        public double StopDistance { get; }

        public FixedStopTargetExitTest(double target_distance, double stop_distance, bool use_percentage)
        {
            _usePercentage = use_percentage;
            TargetDistance = target_distance;
            StopDistance = stop_distance;
        }

        public override void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null)
        {
            FBEResults = new double[data.Length];

            for (int i = 0; i < entries.Length - 1; i++)
            {
                if (entries[i])
                {
                    int x = i + 1;


                    double entryPriceBull = data[x].Open_Ask;
                    double entryPriceBear = data[x].Open_Bid;

                    double stopBull, stopBear, targetBull, targetBear;

                    if (_usePercentage)
                    {
                        stopBull = entryPriceBull * (1 - StopDistance);
                        stopBear = entryPriceBear * (StopDistance + 1);
                        targetBear = entryPriceBear * (TargetDistance - 1);
                        targetBull = entryPriceBull * (1 + TargetDistance);
                    }
                    else
                    {
                        stopBear = entryPriceBear + StopDistance;
                        stopBull = entryPriceBull - StopDistance;
                        targetBear = entryPriceBear - TargetDistance;
                        targetBull = entryPriceBull + TargetDistance;
                    }

                    bool stillLong = true, stillShort = true;

                    while (x < data.Length && (stillLong || stillShort))
                    {
                        if (stillLong)
                        {
                            if (data[x].Open_Bid <= stopBull) FBEResults[i] = data[x].Open_Bid - entryPriceBull;
                            else if (data[x].Low_Bid <= stopBull) FBEResults[i] = stopBull - entryPriceBull;
                            else if (data[x].Open_Bid >= targetBull) FBEResults[i] = data[x].Open_Bid - entryPriceBull;
                            else if (data[x].High_Bid >= targetBull) FBEResults[i] = targetBull - entryPriceBull;

                            if (  Math.Abs(FBEResults[i]) > 0)
                            {
                                //longDuration[i] = x - i;
                                stillLong = false;
                            }
                        }

                        x++;
                    }
                }
            }
            
        }

    }
}
