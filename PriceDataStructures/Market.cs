using DataStructures.StatsTools;

namespace DataStructures
{
    public readonly struct Market
    {
        public BidAskData[] RawData { get; }

        private Market(BidAskData[] data) {
            RawData = data;
        }

        public Market Slice(int startIndex, int endIndex) {
            return new Market(ListTools.GetNewArrayByIndex(RawData, startIndex, endIndex));
        }

        public class MarketBuilder
        {
            public static Market CreateMarket(BidAskData[] data) {
                return new Market(data);
            }
        }
    }
}
