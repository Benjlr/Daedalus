using DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class TrendDay : RuleBase
    {
        public TrendDay()
        {
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Open.TicksToTime.Hour == 10)
                {
                    var prevI = i;
                    i += 10;
                    if (i > data.Count - 1) return;

                    var rangeBars = data.GetRange(prevI, 11).ToList();

                    if (rangeBars.Count(x => x.Close.Mid >= x.Open.Mid) > 7) Satisfied[i] = true;


                }
            }

        }
    }
}
