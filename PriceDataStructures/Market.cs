using System.Linq;
using DataStructures.StatsTools;

namespace DataStructures
{
    public readonly struct Market
    {
        public BidAskData[] RawData { get; }
        public SessionData[] CostanzaData { get; }

        private Market(BidAskData[] data, SessionData[] costanza) {
            RawData = data;
            CostanzaData = costanza;
        }

        public Market Slice(int startIndex, int endIndex) {
            return new Market(ListTools.GetNewArrayByIndex(RawData, startIndex, endIndex),
                ListTools.GetNewArrayByIndex(CostanzaData, startIndex, endIndex));
        }

        public class MarketBuilder
        {
            public static Market CreateMarket(BidAskData[] data) {
                return new Market(data, ConvertDataToSession(data));
            }

            public static Market CreateMarket(SessionData[] data) {
                return new Market(ConvertDatatoBidAsk(data), data);
            }

            private static BidAskData[] ConvertDatatoBidAsk(SessionData[] data) {
                var myArray = new BidAskData[data.Length];

                for (int i = 0; i < data.Length; i++) {

                    myArray[i] = new BidAskData(time: data[i].CloseDate,
                        o_a: data[i].Open,
                        o_b: data[i].Open,
                        h_a: data[i].High,
                        h_b: data[i].High,
                        l_a: data[i].Low,
                        l_b: data[i].Low,
                        c_a: data[i].Close,
                        c_b: data[i].Close,
                        vol: data[i].Volume);
                }
                return myArray;

            }

            private static SessionData[] ConvertDataToSession(BidAskData[] rawData) {
                var costanzaData = new SessionData[rawData.Length];

                for (int i = 0; i < rawData.Length; i++) {
                    costanzaData[i] = new SessionData(
                        rawData[i].Time,
                        rawData[i].volume,
                        (rawData[i].Open_Bid + rawData[i].Open_Ask) / 2.0,
                        (rawData[i].High_Bid + rawData[i].High_Ask) / 2.0,
                        (rawData[i].Low_Bid + rawData[i].Low_Ask) / 2.0,
                        (rawData[i].Close_Bid + rawData[i].Close_Ask) / 2.0);
                }

                return costanzaData;
            }
        }
    }
}
