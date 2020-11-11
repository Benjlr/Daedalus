using PriceSeriesCore.FinancialSeries;
using RuleSets;

namespace Logic
{
    public class Market
    {
        public MarketData[] RawData { get;  }
        public Session[] CostanzaData { get;  }

        public Market(MarketData[] data, Session[] costanza)
        {
            RawData = data;
            CostanzaData = costanza;
        }
    }
}
