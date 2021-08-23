using System;
using System.Diagnostics;
using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Linq;
using DataStructures.StatsTools;

namespace RuleSets.Entry
{
    public class BOWP : RuleBase
    {
        public BOWP()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            var twoHundred = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 200);
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var FiftyMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 221; i < data.Count; i++) {
                var booltwo = twoHundred[i - 21] < twoHundred[i];
                var boolFour = data[i].Close.Mid / data.GetRange(i - 60, 60).ToList().Max(x => x.Close.Mid) > 0.8;

                if (boolFour && booltwo) 
                    Satisfied[i] = true;
            }

        }
    }

    public class PivotPoint : RuleBase
    {
        public PivotPoint() {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            var volAvg = MovingAverage.SimpleMovingAverage(rawData.Select(x => x.Volume).ToList(),40);
            var twenty = MovingAverage.SimpleMovingAverage(rawData.Select(x => x.Close.Mid).ToList(),20);
            var pivs = Pivots.Calculate(rawData);
            

            Satisfied = new bool[data.Count];

            for (int i = 201; i < data.Count; i++) {

                if (rawData[i].Volume * 2 < volAvg[i] && Math.Abs((rawData[i].Open.Mid / rawData[i].Close.Mid) - 1) < 0.01 && rawData[i].Close.Mid > twenty[i]) {
                    var lastPos = ListTools.GetPositionInRange(rawData.ToList().GetRange(i - 30, 30), rawData[i-30].Close.Mid);
                    var currentPos = ListTools.GetPositionInRange(rawData.ToList().GetRange(i - 40, 40), rawData[i].Close.Mid);
                    var perc = Math.Abs(lastPos - currentPos);

                    var lastPivs = pivs.Where(x=>x.Index < i).ToList();

                    //var lastHighPIvs = lastPivs.Where(x => x.HighPivot == 2).ToList();
                    var lowpivs = lastPivs.Where(x => x.LowPivot >= 2).ToList();
                    lowpivs = lowpivs.OrderByDescending(x => x.Index).ToList();


                    //lastHighPIvs.Reverse();
                    //lastHighPIvs = lastHighPIvs.Take(3).ToList();
                    //if (lowpivs.Count < 3) continue;

                    //bool lowpiv = true;
                    //bool highpiv = true;

                    //for (int j = 0; j < 3 && j + 1 < lowpivs.Count ; j++)
                    //    if (rawData[lowpivs[j].Index].Low.Mid < rawData[lowpivs[j + 1].Index].Low.Mid)
                    //        lowpiv = false;


                    //if (!lowpiv)
                    //    break;

                    //for (int j = 0; j < 2; j++) {
                    //    if (rawData[lastHighPIvs[i].Index].High.Mid / rawData[lastHighPIvs[i + 1].Index].High.Mid < 0.05)
                    //        highpiv = false;
                    //}

                    //if (!highpiv)
                    //    break;
                    //if (lastPos > 0.85 && currentPos > 0.85) {
                    //    Trace.TraceInformation($"{rawData[lowpivs[0].Index].Low.Mid:0.000}, {rawData[lowpivs[1].Index].Low.Mid:0.000}, {rawData[lowpivs[2].Index].Low.Mid:0.000}");
                    //    Trace.TraceInformation($"{rawData[lowpivs[0].Index].Low.TicksToTime}, {rawData[lowpivs[1].Index].Low.TicksToTime}, {rawData[lowpivs[2].Index].Low.TicksToTime}");
                    //    Trace.TraceInformation($"{rawData[i].Open.TicksToTime}");
                        Satisfied[i] = true;
                    //}

                }

            }

        }
    }

    public class Century : RuleBase
    {
        public Century() {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];

            for (int i = 70; i < data.Count; i++) {
                var centuryBool = false;
                var pbBool = false;
                var pbBoolDuration = false;
                int centuryPoint = 0;

                var lowest = double.MaxValue;
                var lowestIndex = 0;

                for (int j = i; j >=i-70; j--) {
                    if (data[j].Close.Mid / data[i-70].Close.Mid > 2) {
                        centuryBool = true;
                        centuryPoint = j;
                        break;
                    }
                }

                for (int j = i; j >= centuryPoint; j--) {
                    if (data[j].Low.Mid < lowest) {
                        lowest = data[j].Low.Mid;
                        lowestIndex = j;
                    }
                }

                if (lowestIndex - centuryPoint> 30) continue;
                if (!centuryBool) continue;
                var pbPc = data[centuryPoint].Close.Mid / data[lowestIndex].Close.Mid;
                if (pbPc > 1.4)
                    continue;
                
                Satisfied[i] = true;
            }

        }
    }
}
