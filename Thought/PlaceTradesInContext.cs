using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using DataStructures.PriceAlgorithms;
using DataStructures.StatsTools;

namespace Thought
{
    public class PlaceTradesInContext
    {
        public static List<MarketTrade> GenerateMarketTradesFromResulst(List<MarketResults> results, Universe myUniverse) {
            var myMarketTrades = new List<MarketTrade>();
            
            foreach (var result in results) {
                foreach (var trade in result.Trades) {
                    var choppedPriceList = myUniverse.Elements.FirstOrDefault(x => x.MarketData.Id.Equals(result.MarketName)).MarketData.PriceData;
                    choppedPriceList = ListTools.GetNewArrayByIndex(choppedPriceList, trade.MarketStart - 200, trade.MarketEnd + 200);
                    var myaverages = new double[6][];
                    myaverages[0] = MovingAverage.ExponentialMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 6).ToArray();
                    myaverages[1] = MovingAverage.SimpleMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 10).ToArray();
                    myaverages[2] = MovingAverage.ExponentialMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 20).ToArray();
                    myaverages[3] = MovingAverage.SimpleMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 50).ToArray();
                    myaverages[4] = MovingAverage.ExponentialMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 65).ToArray();
                    myaverages[5] = MovingAverage.SimpleMovingAverage(choppedPriceList.Select(x=>x.Close.Mid).ToList(), 200).ToArray();

                    myMarketTrades.Add(new MarketTrade(myaverages, choppedPriceList,200 , trade.Duration + 200));
                }
            }

            return myMarketTrades;
        }
    }

    public readonly struct MarketTrade
    {
        public MarketTrade(double[][] movingAverages, BidAskData[] prices, int entryIndex, int exitIndex) {
            MovingAverages = movingAverages;
            Prices = prices;
            EntryIndex = entryIndex;
            ExitIndex = exitIndex;
            //EntryPrice = entryPrice;
            //ExitPrice = exitPrice;
        }

        public double[][] MovingAverages { get; }
        public BidAskData[] Prices { get; }
        public int EntryIndex { get; }
        public int ExitIndex { get; }

        //public double EntryPrice { get; }
        //public double ExitPrice { get; }

    }
}
