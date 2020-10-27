using PriceSeriesCore.FinancialSeries;

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
