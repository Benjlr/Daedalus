using PriceSeries.FinancialSeries;
using PriceSeries.Indicators.Derived;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Rules.Entry
{
    public class PriceContraction : RuleBase
    {
        public PriceContraction()
        {
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }


        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            var twentyMA = ExponentialMovingAverage.Calculate(data.Select(x=>x.Close).ToList(), 20);
            var twohundredMA = ExponentialMovingAverage.Calculate(data.Select(x => x.Close).ToList(), 200);

            Satisfied = new bool[data.Count];

            for (int i = 200; i < data.Count; i++)
            {
                if (data[i].High > twohundredMA[i] && data[i].Close < twentyMA[i] && rawData[i].Open_Ask - rawData[i].Open_Bid < 2) Satisfied[i] = true;
            }
            
        }
    }
}
