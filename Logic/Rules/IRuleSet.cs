using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Rules
{
    public interface IRuleSet 
    {
        Thesis Dir { get; }
        Pos Order { get; }
        bool[] Satisfied { get; }
        void CalculateBackSeries(List<Session> data, MarketData[] rawData);
    }
}
