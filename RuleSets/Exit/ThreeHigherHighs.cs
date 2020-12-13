using DataStructures;
using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Exit
{
    public class ThreeHigherHighs : RuleBase
    {
        public ThreeHigherHighs()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<BidAskData> data, BidAskData[] rawData)
        {
            var dailys = SessionCollate.CollateTo24HrDaily(data);
            Satisfied = new bool[data.Count];

            for (int i = 2; i < dailys.Count; i++)
            {
                if (dailys[i].High.Mid > dailys[i - 1].High.Mid /*&& dailys[i - 1].High > dailys[i - 2].High*/)
                {
                    var start = data.IndexOf(data.First(x => x.Open.Time == dailys[i].Open.Time));
                    var last = data.IndexOf(data.First(x => x.Close.Time == dailys[i].Close.Time));

                    for (int j = start; j < last; j++)
                    {
                        if (data[j].High.Mid > dailys[i - 1].High.Mid)
                        {
                            Satisfied[j] = true;
                            break;
                        }

                    }
                }
            }
        }
    }
}
