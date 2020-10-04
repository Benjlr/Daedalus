using PriceSeries;
using PriceSeries.FinancialSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Logic
{
    public class MarketBuilder
    {
        
        public static Market CreateMarket(string data_path)
        {
            var mData = LoadData(data_path);
            var cData = ConvertDataToSession(mData);


            StringBuilder t = new StringBuilder();
            for (int i = 0; i < cData.Length; i++)
            {
                t.AppendLine($"{cData[i].CloseDate},{cData[i].Open},{cData[i].High},{cData[i].Low},{cData[i].Close}");
            }
            File.WriteAllText(@"C:\Temp\Market.csv",t.ToString());


            return new Market(mData, cData);
        }

        public static List<Market> CreateStockMarkets()
        {
            List<Market> retvals = new List<Market>();
            var Ticks = new List<string>(File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 200.asx.txt").ToList());
            foreach (var t in Ticks)
            {
                var fs = File.ReadAllLines(t);
                var stock = IOFunctions.EquityData.Instance.Loaddata(t, MetaStockGrouping.ASXEquities, false);
                var cut = stock.GetTradeList(Interval.FastFrame).GetRange(stock.GetTradeList(Interval.FastFrame).Count - 300, 300);

                retvals.Add(new Market(BuildFromCoszData(cut), cut.ToArray()));
            }

            return retvals;
        }

        private static MarketData[] BuildFromCoszData(List<Session> data)
        {
            var myArray = new MarketData[data.Count];

            for (int i = 0; i < data.Count; i++)
            {

                myArray[i] = new MarketData(time: data[i].CloseDate,
                    o_a: data[i].Open,
                    o_b: data[i].Open,
                    h_a: data[i].High,
                    h_b: data[i].High,
                    l_a: data[i].Low,
                    l_b: data[i].Low,
                    c_a: data[i].Close,
                    c_b: data[i].Close,
                    vol: (long)data[i].Volume);
            }
            return myArray;

        }

        private static MarketData[] LoadData(string location)
        {
            var fs = File.ReadAllLines(location);
            var myArray = new MarketData[fs.Length];

            for (int i = 0; i < fs.Length; i++)
            {
                var myLine = fs[i].Split(',');

                myArray[i] = new MarketData(time: DateTime.ParseExact(myLine[0], "yyyy/MM/dd HH:mm:ss", null),
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

        private static Session[] ConvertDataToSession(MarketData[] rawData)
        {
           var costanzaData = new Session[rawData.Length];

            for (int i = 0; i < rawData.Length; i++)
            {
                costanzaData[i] = new Session(
                   rawData[i].Time, 
                   rawData[i].volume, 
                   rawData[i].Open_Bid, 
                   rawData[i].Open_Ask, 
                   rawData[i].High_Bid, 
                   rawData[i].High_Ask, 
                   rawData[i].Low_Bid, 
                   rawData[i].Low_Ask,
                   rawData[i].Close_Bid, 
                   rawData[i].Close_Ask);
                if(i > 1) costanzaData[i].ReturnSeries = costanzaData[i].Close / costanzaData[i - 1].Close - 1;
            }

            return costanzaData;
        }

    }
}
