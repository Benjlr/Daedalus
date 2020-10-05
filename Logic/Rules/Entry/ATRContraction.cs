using System;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Logic.Utils.Calculations;

namespace Logic.Rules.Entry
{
    public class ATRContraction : RuleBase
    {
        public ATRContraction()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fissy = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];
            var coun = 0;

            for (int i = 50; i < data.Count; i++)
            {
                var sixtoTen = Math.Abs(SixMA[i] - tenMA[i]);
                var tentoTwenny = Math.Abs(twentyMa[i] - tenMA[i]);
                var max = data.GetRange(i - 11, 12).Max(x => x.High);
                var low = data.GetRange(i - 11, 12).Min(x => x.Low);
                var cuur = data[i].Close;

                var percentage = (cuur - low) / (max - low);


                if ( tentoTwenny < atr[i] * 0.5 
                    && sixtoTen < atr[i] * 0.5
                    )
                {
                    coun++;

                    if(coun > 8 && twentyMa[i] > fissy[i] && percentage > 0.8 && atrPC[i] < 0.1) Satisfied[i] = true;
                }
                else
                {
                    coun = 0;
                }
            }
            
        }
    }

    public class ATRExpansion : RuleBase
    {
        public ATRExpansion()
        {
            Dir = Thesis.Bull;
            Order = Pos.Exit;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];
            var coun = 0;

            for (int i = 0; i < data.Count; i++)
            {
                var sixToClose = ( data[i].Close- tenMA[i]);
                //var tentoTwenny = Math.Abs(twentyMa[i] - tenMA[i]);

                if (atrPC[i] > 0.66 && sixToClose > atr[i] * 1 )
                {
                    Satisfied[i] = true;
                }
            }

        }
    }
}
