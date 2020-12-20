using System.IO;
using DataStructures.StatsTools;

namespace DataStructures
{
    public readonly struct Market
    {
        public string Id { get; }
        public BidAskData[] PriceData { get; }
        public Market(BidAskData[] data, string id) {
            Id = id;
            PriceData = data;
        }

        public Market(string name) {
            Id = Path.GetFileNameWithoutExtension(name);
            PriceData = DataLoader.LoadData(name);
        }

        public Market Slice(int startIndex, int endIndex) {
            return new Market(ListTools.GetNewArrayByIndex(PriceData, startIndex, endIndex), this.Id);
        }
    }
}
