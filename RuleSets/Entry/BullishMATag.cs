using PriceSeriesCore.FinancialSeries;
using RuleSets.Calculations;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class BullishMATag : RuleBase
    {
        public BullishMATag()
        {
            Dir = MarketSide.Bull;
            Order = Action.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var twohundredMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 60; i < data.Count; i++)
            {
                if (data[i].Close > twohundredMA[i] &&
                    data[i].Close > twentyMA[i] &&
                    data[i].High < six[i]) Satisfied[i] = true;
            }

        }
    }

    public class CrossoverTag : RuleBase
    {
        public CrossoverTag()
        {
            Dir = MarketSide.Bear;
            Order = Action.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var Atr = AverageTrueRange.Calculate(data);

            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fiftyMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var tenMa = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 10);
            var sixMa = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

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
