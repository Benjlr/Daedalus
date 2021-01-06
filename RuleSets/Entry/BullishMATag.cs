using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Linq;

namespace RuleSets.Entry
{
    public class BullishMATag : RuleBase
    {
        public BullishMATag()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            var twoHundred = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 200);
            //var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            //var FiftyMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            //var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 60; i < data.Count; i++)
            {
                if (data[i-1].Close.Mid < twoHundred[i-1] && data[i].Close.Mid > twoHundred[i]) Satisfied[i] = true;
            }

        }
    }

    public class BearishTage : RuleBase
    {
        public BearishTage() {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }


        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            var twoHundred = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 200);
            //var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            //var FiftyMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            //var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 60; i < data.Count; i++) {
                if (data[i - 1].Close.Mid > twoHundred[i - 1] && data[i].Close.Mid < twoHundred[i]) Satisfied[i] = true;
            }

        }
    }

    public class CrossoverTag : RuleBase
    {
        public CrossoverTag()
        {
            Dir = MarketSide.Bear;
            Order = ActionPoint.Entry;
        }


        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            var Atr = AverageTrueRange.Calculate(data);

            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            var fiftyMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 50);
            var tenMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close.Mid).ToList(), 10);
            var sixMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);

            Satisfied = new bool[data.Count];
            int count = 0;

            for (int i = 55; i < data.Count; i++)
            {


                if (twentyMA[i] > fiftyMA[i] &&
                    tenMa[i] - twentyMA[i] < 1.2 * Atr[i]) count++;
                else count = count > 0 ? count - 1 : 0;

                if (sixMa[i - 1] < tenMa[i - 1] && sixMa[i] >= tenMa[i] && count > 3) Satisfied[i] = true;

            }

        }
    }
}
