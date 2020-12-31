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
                var boolOne = data[i].Close.Mid > twoHundred[i];
                //var booltwo = twoHundred[i-21] < twoHundred[i];
                //var boolThree = FiftyMa[i] > twoHundred[i];
                //var boolFour = data[i].Close.Mid / data.GetRange(i - 180, 180).ToList().Max(x=>x.Close.Mid) > 0.66;

                if (boolOne) 
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
            var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);
            Satisfied = new bool[data.Count];

            for (int i = 40; i < data.Count; i++) {
                if (ListTools.GetPositionInRange(volAvg, rawData[i].Volume) < 0.6
                && (rawData[i].Close.Mid / twentyMA[i]) -1 < 0.015 
                && Math.Abs((rawData[i].Open.Mid / rawData[i].Close.Mid) -1) < 0.025
                && data[i].Close.Mid > twoHundred[i]
                && FiftyMa[i] > twoHundred[i]) Satisfied[i] = true;
            }

        }
    }
}
