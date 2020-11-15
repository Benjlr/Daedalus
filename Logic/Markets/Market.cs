using System.Linq;
using PriceSeriesCore;

namespace Logic.Markets
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

        public Market ChopMarket(int max)
        {
            var myRaws = this.RawData.ToList().GetRange(0, max);
            var mySess = this.CostanzaData.ToList().GetRange(0, max);

            return new Market(myRaws.ToArray(),mySess.ToArray());
        }
    }
}
