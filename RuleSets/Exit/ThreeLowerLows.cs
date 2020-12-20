using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class ThreeLowerLows : RuleBase
    {
        public ThreeLowerLows()
        {
            Dir = MarketSide.Bear;
            Order = ActionPoint.Exit;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];

            for (int i = 2; i < data.Count; i++)
            {
                if (data[i].Low.Mid < data[i - 1].Low.Mid && data[i - 1].Low.Mid < data[i - 2].Low.Mid) Satisfied[i] = true;
            }

        }
    }

    public class MAViolation : RuleBase
    {
        public MAViolation()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 20);
            Satisfied = new bool[data.Count];

            for (int i = 25; i < data.Count; i++)
            {
                if (data[i - 1].Close.Mid < twentyMA[i - 1] && data[i].Low.Mid < data[i - 1].Low.Mid) Satisfied[i] = true;
            }

        }
    }
}
