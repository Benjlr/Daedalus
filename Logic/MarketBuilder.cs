using PriceSeries.FinancialSeries;
using System;
using System.IO;

namespace Logic
{
    public class MarketBuilder
    {
        
        public static Market CreateMarket(string data_path)
        {
            var mData = LoadData(data_path);
            var cData = ConvertDataToSession(mData);

            return new Market(mData, cData);
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

            for (int i = 1; i < myArray.Length; i++)
            {
                if (Math.Abs(myArray[i].Open_Bid - myArray[i - 1].Close_Bid) > 50)
                {
                    string ss = "";
                }
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
            }

            return costanzaData;
        }

    }
}
