using System;
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
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var twoHundred = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 200);
            var FiftyMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            Satisfied = new bool[data.Count];

            for (int i = 70; i < data.Count; i++) {
                if (ListTools.GetPositionInRange(volAvg, rawData[i].Volume) < 0.6
                && (rawData[i].Close.Mid / twentyMA[i]) -1 < 0.015 
                && Math.Abs((rawData[i].Open.Mid / rawData[i].Close.Mid) -1) < 0.025
                && data[i].Close.Mid > twoHundred[i]
                && FiftyMa[i] > twoHundred[i]) Satisfied[i] = true;
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
