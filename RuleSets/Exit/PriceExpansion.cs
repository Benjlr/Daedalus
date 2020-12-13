using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class PriceExpansion : RuleBase
    {
        public PriceExpansion()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        private double _multiple = 3;

        public override void CalculateBackSeries(List<BidAskData> data, BidAskData[] rawData)
        {
            Satisfied = new bool[data.Count];
            var sixEMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close.Mid).ToList(), 6);
            var atr = AverageTrueRange.Calculate(data);


            for (int i = 6; i < data.Count; i++)
            {
                if (data[i].High.Mid > (_multiple * atr[i]) + sixEMA[i]) Satisfied[i] = true;
            }
        }
    }
}
