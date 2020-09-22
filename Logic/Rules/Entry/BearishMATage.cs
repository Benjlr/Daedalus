using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;
using Logic.Utils.Calculations;

namespace Logic.Rules.Entry
{
    public class BearishMATage : RuleBase
    {
        public BearishMATage()
        {
            Dir = Thesis.Bear;
            Order = Pos.Entry;
        }
        private double spread = 2;


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var twohundredMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 200);
            var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 200; i < data.Count; i++)
            {
                if (data[i].High < twohundredMA[i] && 
                    data[i].High < twentyMA[i] && 
                    data[i].Low > six[i] &&
                    rawData[i].Open_Ask - rawData[i].Open_Bid <= spread) Satisfied[i] = true;
            }
            
        }
    }
}
