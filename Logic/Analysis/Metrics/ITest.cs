using Logic.Utils;
using PriceSeriesCore;
using System.Collections.Generic;

namespace Logic.Analysis.Metrics
{
    public interface ITest
    {
        public List<Trade> Trades { get; }

        public ExtendedStats Stats { get; }

        void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null);
    }
}
