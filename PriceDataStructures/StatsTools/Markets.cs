using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataStructures.StatsTools
{
    public class Markets
    {
        public static string asx200_cash_5 => GetDropboxLocation() + "\\Testing\\asx200cash";
        public static string aud_usd_5 => GetDropboxLocation() + "\\Testing\\AUDUSD";
        public static string asx200_cash_daily => @"C:\Applications\Trading Data\CSV\Indices\XJO.csv";
        public static string bitcoin_5 => GetDropboxLocation() + "\\Testing\\bitcoin";
        public static string sp500_cash_5 => GetDropboxLocation() + "\\Testing\\sp500cash";
        public static string futures_cotton_5 => GetDropboxLocation() + "\\Testing\\Cotton";
        public static string futures_iron_5 => GetDropboxLocation() + "\\Testing\\Iron";
        public static string futures_wheat_5 => GetDropboxLocation() + "\\Testing\\Wheat";
        public static string futures_gold_5 => GetDropboxLocation() + "\\Testing\\Gold";
        public static string futures_oil_us_5 => GetDropboxLocation() + "\\Testing\\Oil_US_Crude";
        public static string APT_Daily => @"C:\Applications\Trading Data\CSV\Equities\APT.csv";
        public static string CBA_Daily => @"C:\Applications\Trading Data\CSV\Equities\CBA.csv";
        public static string test_data => GetDropboxLocation() + "\\Testing\\testdata";

        public static List<string> ASX300() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 300.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x=>myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities",x +".csv" )));
            return myList;
        }
        public static List<string> AllASX() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\ASX Equities Operating Company.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASXAllOrds() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\ASX All Ordinaries.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASX200() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 200.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }

        public static List<string> ASX50() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 50.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }

        public static List<string> ASX20() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 20.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }

        public static List<string> ASXSmallOrds() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX Small Ordinaries.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASX300Minus200() {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 300 excl S&P ASX 200.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }


        private static string GetDropboxLocation() {
            var patheOne = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dropbox\\info.json");
            var patheTwo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dropbox\\info.json");

            if (File.Exists(patheOne)) return ReturnDropBoxLocation(patheOne);
            if (File.Exists(patheTwo)) return ReturnDropBoxLocation(patheTwo);

            throw new Exception("Dropbox hasn't been set up");
        }

        private static string ReturnDropBoxLocation(string jsonFileLocation) {
            using var textReader = new StreamReader(new FileStream(jsonFileLocation, FileMode.Open));
            var jobj = JObject.Load(new JsonTextReader(textReader));
            var f = jobj?.First?.First?.Value<string>("path");
            return f;
        }
    }
}
