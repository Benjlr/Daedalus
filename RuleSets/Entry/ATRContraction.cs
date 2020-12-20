using DataStructures;
using DataStructures.PriceAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class ATRContraction : RuleBase
    {
        public ATRContraction()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public  void aasass(List<BidAskData> data, BidAskData[] rawData)
        {
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data, 20);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var fissy = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

            Satisfied = new bool[data.Count];
            var coun = 0;

            for (int i = 50; i < data.Count; i++)
            {
                var sixtoTen = Math.Abs(SixMA[i] - tenMA[i]);

                if (sixtoTen < atr[i] * 0.5) {
                    coun++;
                    if (coun > 7 && twentyMa[i] > fissy[i] && atrPC[i - 1] == 0.0 && atrPC[i] != 0.0 && rawData[i].Open.Ask - rawData[i].Open.Bid <= 3) Satisfied[i] = true;
                }
                else coun = 0;
            }

            //var atrPC = AverageTrueRange.CalculateATRPC(data);

            //for (int i = 50; i < data.Count; i++)
            //{
            //    if (atrPC[i - 1] == 0.0 && atrPC[i] != 0.0 && twentyMa[i] > fissy[i] ) Satisfied[i] = true;
            //}
        }
        public double GetPositionInRange(List<double> myInput, double value) {
            var Min = myInput.Min();
            var Max = myInput.Max();
            return (value - Min) / (Max - Min);
        }




        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();

            var atrPC = AverageTrueRange.CalculateATRPC(data);
            Satisfied = new bool[data.Count];
            var volavg = rawData.Select(x => x.Volume).ToList();
            for (int i = 30; i < data.Count; i++) {
                var myVOl = GetPositionInRange(volavg.GetRange(i - 20, 20), volavg[i]);
                if (atrPC[i] == 0.0 && myVOl < 0.2  ) 
                    Satisfied[i] = true; }

        }

    }

    public class ATRContractionLong : RuleBase
    {
        public ATRContractionLong() {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public double GetPositionInRange(List<double> myInput, double value) {
            var Min = myInput.Min();
            var Max = myInput.Max();
            return (value - Min) / (Max - Min);
        }
        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            Satisfied = new bool[data.Count];
            //var sma = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 200);
            var volavg = rawData.Select(x => x.Volume).ToList();
            for (int i = 30; i < data.Count; i++) {
                var myVOl = GetPositionInRange(volavg.GetRange(i - 20, 20), volavg[i]);
                if (/*sma[i - 1] < data[i - 1].Close &&*/ atrPC[i] == 0.0 && myVOl < 0.1 && rawData[i].Open.Ask - rawData[i].Open.Bid <= 4)
                    Satisfied[i] = true;
            }
        }
    }
    public class ATRContractionShort : RuleBase
    {
        public ATRContractionShort() {
            Dir = MarketSide.Bear;
            Order = ActionPoint.Entry;
        }

        public double GetPositionInRange(List<double> myInput, double value) {
            var Min = myInput.Min();
            var Max = myInput.Max();
            return (value - Min) / (Max - Min);
        }
        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var sma = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            Satisfied = new bool[data.Count];
            var volavg = rawData.Select(x => x.Volume).ToList();
            for (int i = 201; i < data.Count; i++) {
                var myVOl = GetPositionInRange(volavg.GetRange(i - 20, 20), volavg[i]);
                if (sma[i-1] > data[i-1].Close.Mid && atrPC[i] == 0.0 && myVOl < 0.1 && rawData[i].Open.Ask - rawData[i].Open.Bid <= 4)
                    Satisfied[i] = true;
            }
        }
    }

    public class ATRExpansion : RuleBase
    {
        public ATRExpansion()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }


        public override void CalculateBackSeries(BidAskData[] rawData)
        {

            var data = rawData.ToList();

            var atrPC = AverageTrueRange.CalculateATRPC(data);
            var atr = AverageTrueRange.Calculate(data);
            var twentyMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var tenMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 10);
            var SixMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

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