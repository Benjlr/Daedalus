using PriceSeriesCore.FinancialSeries;
using System.Collections.Generic;
using System.Linq;
using Logic.Strategies.Rules;
using Logic.Utils.Calculations;

namespace Logic.Rules.Exit
{
    public class ThreeHigherHighs : RuleBase
    {
        public ThreeHigherHighs()
        {
            Dir = MarketSide.Bull;
            Order = Action.Exit;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var dailys = SessionCollate.CollateTo24HrDaily(data);
            Satisfied =new bool[data.Count];

            for (int i = 2; i < dailys.Count; i++)
            {
                if (dailys[i].High > dailys[i - 1].High /*&& dailys[i - 1].High > dailys[i - 2].High*/)
                {
                    var start = data.IndexOf(data.First(x => x.OpenDate == dailys[i].OpenDate));
                    var last = data.IndexOf(data.First(x => x.CloseDate == dailys[i].CloseDate));

                    for (int j = start; j < last; j++)
                    {
                        if (data[j].High > dailys[i-1].High)
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
