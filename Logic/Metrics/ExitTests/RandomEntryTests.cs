using System;
using System.Collections.Generic;
using DataStructures;
using DataStructures.StatsTools;

namespace Logic.Metrics.ExitTests
{
    public class RandomEntryTests : TestBase
    {

        // Random Entry

        private double _mean { get; }
        private double _sDev { get; }


        public RandomEntryTests(double meanLong, double stdLong)
        {
            _mean= meanLong;
            _sDev = stdLong;
        }

        public void RunRE(BidAskData[] data, bool[] exits)
        {
            throw new NotImplementedException();
        }

        protected override void SetResult(BidAskData[] data, int i)
        {
            throw new NotImplementedException();
        }
    }
}
