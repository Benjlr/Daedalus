using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class InvestorBotEntry : RuleBase
    {
        public InvestorBotEntry()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        private double atrnum = 2;
        private int gapSize = 8;
        private int startLook = 8;
        private int endLook = 5;
        private double spread = 2;

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var twentyEMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var tenSMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var sixEMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);
            var twoHundredSMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 200);
            var atr = AverageTrueRange.Calculate(data);
            var nrwRs = NRWRBars.Calculate(data);

            for (int i = 201; i < data.Count; i++)
            {
                if ((Math.Abs(data[i].Close - twentyEMA[i]) < atrnum * atr[i]
                     || Math.Abs(data[i].Close - tenSMA[i]) < atrnum * atr[i]))
                    //&& data[i].Close > twoHundredSMA[i])

                    for (int j = i - startLook; j < i - endLook; j++)
                    {
                        if (nrwRs[j] > gapSize && 
                            data[j].High > data[i].Close && 
                            data[i].Close < sixEMA[i] && 
                            rawData[i].Open_Ask- rawData[i].Open_Bid <= spread)
                        {
                            Satisfied[i] = true;
                        }
                    }
            }
        }
    }
}
