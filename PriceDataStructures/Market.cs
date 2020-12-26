using System;
using System.IO;
using System.Linq;
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

            var myTicks = new DateTime(2019,05,01).Ticks;
            var myPriceList = PriceData.ToList();
            for (int i = myPriceList .Count- 1; i >=0; i--) {
                if(myPriceList[i].Open.Ticks < myTicks)
                    myPriceList.RemoveAt(i);
            }

            PriceData = myPriceList.ToArray();
        }

        public Market Slice(int startIndex, int endIndex) {
            return new Market(ListTools.GetNewArrayByIndex(PriceData, startIndex, endIndex), this.Id);
        }
    }
}
