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
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData)
        {
            Satisfied = new bool[data.Count];

            for (int i = 2; i < data.Count; i++)
            {
                if (data[i].Low < data[i - 1].Low && data[i - 1].Low < data[i - 2].Low) Satisfied[i] = true;
            }

        }
    }

    public class MAViolation : RuleBase
    {
        public MAViolation()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<SessionData> data, BidAskData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            Satisfied = new bool[data.Count];

            for (int i = 25; i < data.Count; i++)
            {
                if (data[i - 1].Close < twentyMA[i - 1] && data[i].Low < data[i - 1].Low) Satisfied[i] = true;
            }

        }
    }
}
