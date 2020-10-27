using Logic.Utils.Calculations;
using PriceSeriesCore.FinancialSeries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class ATRContraction : RuleBase
    {
        public ATRContraction()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public void Calc(List<Session> data, MarketData[] rawData)
        {
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data, 20);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fissy = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];
            var coun = 0;

            for (int i = 50; i < data.Count; i++)
            {
                var sixtoTen = Math.Abs(SixMA[i] - tenMA[i]);

                if (sixtoTen < atr[i] * 0.5
                )
                {
                    coun++;
                    if (coun > 7 && twentyMa[i] > fissy[i] && atrPC[i - 1] == 0.0 && atrPC[i] != 0.0) Satisfied[i] = true;
                }
                else coun = 0;
            }

            //var atrPC = AverageTrueRange.CalculateATRPC(data);

            //for (int i = 50; i < data.Count; i++)
            //{
            //    if (atrPC[i - 1] == 0.0 && atrPC[i] != 0.0 && twentyMa[i] > fissy[i] ) Satisfied[i] = true;
            //}
        }



        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {

            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data,20);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fissy = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];
            var coun = 0;

            for (int i = 0; i < data.Count; i++)
            {

                //if ( atrPC[i] == 0.0) Satisfied[i] = true;

                //var xxx = ListTools.GetPositionRange(ListTools.GetNewList(data, i - 25, i), data[i].Close);
                var sixtoTen = Math.Abs(SixMA[i] - tenMA[i]);

                if (sixtoTen < atr[i] * 0.5
                )
                {
                    coun++;
                    if (coun > 9 && twentyMa[i] > fissy[i] && atrPC[i - 1] == 0.0 && atrPC[i] == 0.0)
                    {
                        //var newIndex = i + Satisfied.Length / 2;
                        //if (newIndex > Satisfied.Length) newIndex -= Satisfied.Length;
                        //Satisfied[newIndex] = true;
                        Satisfied[i] = true;
                    }
                }
                else coun = 0;
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

            var myLineCloseness = MovingAverage.GetRMSE(new List<List<double>>() { tenMA, SixMA, twentyMa });

            Satisfied = new bool[data.Count];

            for (int i = 0; i < data.Count; i++)
            {

                var lines = myLineCloseness.Skip(Math.Max(0, i - 8)).Take(9).ToList();

                if (atrPC[i] == 0.0 && lines.All(x => x < 0.1))
                {
                    var m = (tenMA[i] - tenMA[i - 6]) / 6;

                    /*   if(Math.Abs(m) <2)*/
                    Satisfied[i] = true;
                }
            }



        }
    }
}