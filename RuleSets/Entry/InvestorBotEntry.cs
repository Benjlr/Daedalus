using DataStructures;
using DataStructures.PriceAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class InvestorBotEntry : RuleBase
    {
        public InvestorBotEntry()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }

        private double atrnum = 3;
        private int gapSize = 8;
        private int startLook = 8;
        private int endLook = 5;
        private double spread = 2;

        public override void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var twentyEMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var tenSMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var sixEMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);
            var atr = AverageTrueRange.Calculate(data);
            var nrwRs = NRWRBars.Calculate(data);

            for (int i = 20; i < data.Count; i++)
            {
                if ((Math.Abs(data[i].Close - twentyEMA[i]) < atrnum * atr[i]
                     || Math.Abs(data[i].Close - tenSMA[i]) < atrnum * atr[i]))
                {
                    for (int j = i - startLook; j < i - endLook; j++)
                    {
                        if (nrwRs[j] > gapSize &&
                            data[i].Close < sixEMA[i] &&
                            rawData[i].Open_Ask - rawData[i].Open_Bid <= spread)
                        {
                            Satisfied[i] = true;
                        }
                    }
                }
            }
        }
    }
}
