using System;
using System.IO;
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
            public static Market CreateMarket(string data_path) {
                return LoadData(data_path);
            }

            public static Market CreateMarket(BidAskData[] data) {
                return new Market(data, ConvertDataToSession(data));
            }

            public static Market CreateMarket(SessionData[] data) {
                return new Market(ConvertDatatoBidAsk(data), data);
            }

            private static Market LoadData(string data_path) {
                var data = File.ReadAllLines(data_path);
                BidAskData[] myBidAskData;
                SessionData[] myConsolidatedData;

                if (data[0].Split(',').Length == 10) {
                    myBidAskData = LoadBidAskData(data_path);
                    myConsolidatedData = ConvertDataToSession(myBidAskData);
                }
                else {
                    myConsolidatedData = LoadConsolidatedData(data_path);
                    myBidAskData = ConvertDatatoBidAsk(myConsolidatedData);
                }

                return new Market(myBidAskData, myConsolidatedData);
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

            private static SessionData[] LoadConsolidatedData(string location) {
                var fs = File.ReadAllLines(location);
                var myArray = new SessionData[fs.Length];

                for (int i = 0; i < fs.Length; i++) {
                    var myLine = fs[i].Split(',');

                    myArray[i] = new SessionData(
                        cd: DateTime.ParseExact(myLine[0], "yyyy/MM/dd", null),
                        v: double.Parse(myLine[5]),
                        o: double.Parse(myLine[1]),
                        h: double.Parse(myLine[2]),
                        l: double.Parse(myLine[3]),
                        c: double.Parse(myLine[4]));
                }

                return myArray;

            }

            private static BidAskData[] LoadBidAskData(string location) {
                var fs = File.ReadAllLines(location);
                var myArray = new BidAskData[fs.Length];

                for (int i = 0; i < fs.Length; i++) {
                    var myLine = fs[i].Split(',');

                    myArray[i] = new BidAskData(time: DateTime.ParseExact(myLine[0], "yyyy/MM/dd HH:mm:ss", null),
                        o_a: double.Parse(myLine[1]),
                        o_b: double.Parse(myLine[2]),
                        h_a: double.Parse(myLine[3]),
                        h_b: double.Parse(myLine[4]),
                        l_a: double.Parse(myLine[5]),
                        l_b: double.Parse(myLine[6]),
                        c_a: double.Parse(myLine[7]),
                        c_b: double.Parse(myLine[8]),
                        vol: long.Parse(myLine[9]));
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
