using PriceSeries.FinancialSeries;
using System.Collections.Generic;

namespace Logic.Rules.Entry
{
    public class DummyEntries : RuleBase
    {
        private int interval { get; set; }
        private int rangeToTest { get; set; }

        public DummyEntries(int intervalInput, int barsToTest)
        {
            interval = intervalInput;
            rangeToTest = barsToTest;
            Dir = Thesis.Bull;
            Order = Pos.Entry;
        }

        public override void CalculateBackSeries(List<Session> data, MarketData[] rawData)
        {
            Satisfied = new bool[data.Count];
            for (int i = 0; i < rangeToTest; i++)
                if (i % interval == 0) Satisfied[i] = true;
        }
    }
}
