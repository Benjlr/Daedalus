using Logic.Utils.Calculations;
using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class BearishMATage : RuleBase
    {
        public BearishMATage()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var fiftyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 50);

            Satisfied = new bool[data.Count];

            for (int i = 55; i < data.Count; i++)
            {
                if (twentyMA[i - 1] > fiftyMA[i - 1] && twentyMA[i] < fiftyMA[i]) Satisfied[i] = true;
            }

        }
    }
}
