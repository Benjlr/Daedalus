using DataStructures;
using System.Collections.Generic;
using DataStructures.StatsTools;

namespace Logic.Metrics
{
    public interface ITest
    {
        public List<Trade> Trades { get; }

        public ExtendedStats Stats { get; }

        void Run(BidAskData[] data, bool[] entries, List<SessionData> myInputs = null);
    }
}
