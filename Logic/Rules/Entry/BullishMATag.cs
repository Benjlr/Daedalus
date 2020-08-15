using Logic.Calculations;
using PriceSeries.FinancialSeries;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class BullishMATag : RuleBase
    {
        public BullishMATag()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }
        private double spread = 2;


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 20);
            var twohundredMA = MovingAverage.SimpleMovingAverage(data.Select(x => x.Close).ToList(), 50);
            var six = MovingAverage.ExponentialMovingAverage(data.Select(x => x.Close).ToList(), 6);

            Satisfied = new bool[data.Count];

            for (int i = 200; i < data.Count; i++)
            {
                if (data[i].Close > twohundredMA[i] && 
                    data[i].Close > twentyMA[i] && 
                    data[i].High < six[i] &&
                    rawData[i].Open_Ask - rawData[i].Open_Bid <= spread) Satisfied[i] = true;
            }
            
        }
    }
}
