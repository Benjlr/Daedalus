using DataStructures;
using System.Collections.Generic;
using System.Linq;

namespace RuleSets.Entry
{
    public class DummyEntries : RuleBase
    {
        private int interval { get; set; }
        private int rangeToTest { get; set; }

        public DummyEntries(int intervalInput, int barsToTest)
        {
            interval = intervalInput;
            rangeToTest = barsToTest;
            Dir = MarketSide.Bull;
            Order = ActionPoint.Entry;
        }

        public override void CalculateBackSeries(BidAskData[] rawData)
        {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];
            for (int i = 0; i < rangeToTest && i < Satisfied.Length; i++)
                if (i % interval == 0) Satisfied[i] = true;
        }
    }

    public class DummyExits : RuleBase
    {
        private int interval { get; set; }
        private int rangeToTest { get; set; }

        public DummyExits(int intervalInput, int barsToTest) {
            interval = intervalInput;
            rangeToTest = barsToTest;
            Dir = MarketSide.Bull;
            Order = ActionPoint.Exit;
        }

        public override void CalculateBackSeries(BidAskData[] rawData) {
            var data = rawData.ToList();
            Satisfied = new bool[data.Count];
            for (int i = 0; i < rangeToTest && i < Satisfied.Length; i++)
                if (i % interval == 0) Satisfied[i] = true;
        }
    }
}
