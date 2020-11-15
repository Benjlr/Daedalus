using System.Collections.Generic;
using Logic.Utils;
using PriceSeriesCore;

namespace Logic.Analysis.Metrics
{
    public interface ITest
    {
        public double[] FBEResults { get;  }
        public double[] FBEDrawdown { get;  }
        public double[] Durations { get; }
        public ExtendedStats Stats { get; }

        void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null);
    }
}
