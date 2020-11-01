using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Logic.Utils
{
    public class Markets
    {
        public static readonly string ASX200_Cash_5_Min = GetDropboxLocation() + "\\Testing\\asx200cash";
        public static readonly string AUD_USD_5_Min = GetDropboxLocation() + "\\Testing\\AUDUSD";
        public static readonly string ASX200_Cash_Daily = @"C:\Applications\Trading Data\CSV\Indices\XJO.csv";
        public static readonly string Bitcoin = GetDropboxLocation() + "\\Testing\\bitcoin";
        public static readonly string test_data = GetDropboxLocation() + "\\Testing\\testdata";
        //public static readonly string ASX200_Cash_5_Min = @"C:\Users\Ben Roberts\Dropbox\Testing\asx200cash";
        public static readonly string SP500_Cash_5_Min = GetDropboxLocation() + "\\Testing\\sp500cash";
        public static readonly string APT_Daily = @"C:\Applications\Trading Data\CSV\Equities\APT.csv";
        public static readonly string CBA_Daily = @"C:\Applications\Trading Data\CSV\Equities\CBA.csv";

        public static List<string> ASX300()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 300.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x=>myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities",x +".csv" )));
            return myList;
        }
        public static List<string> AllASX()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\ASX Equities Operating Company.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASXAllOrds()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\ASX All Ordinaries.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASX200()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 200.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASXSmallOrds()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX Small Ordinaries.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }
        public static List<string> ASX300Minus200()
        {
            var vals = File.ReadAllLines(@"C:\Applications\Trading Data\Stocks\ASX\Lists\S&P ASX 300 excl S&P ASX 200.asx.txt").ToList();
            var myList = new List<string>();
            vals.ForEach(x => myList.Add(Path.Combine($"C:\\Applications\\Trading Data\\CSV\\Equities", x + ".csv")));
            return myList;
        }


        private static string GetDropboxLocation()
        {
            //See https://www.dropbox.com/help/desktop-web/locate-dropbox-folder
            var patheOne = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Dropbox\\info.json");
            var patheTwo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Dropbox\\info.json");

            if (File.Exists(patheOne)) return ReturnDropBoxLocation(patheOne);
            if (File.Exists(patheTwo)) return ReturnDropBoxLocation(patheTwo);

            throw new Exception("Dropbox hasn't been set up");
        }
        private static readonly object _lock = new object();

        private static string ReturnDropBoxLocation(string jsonFileLocation)
        {
            //lock (_lock)
            //{
                TextReader textReader = new StreamReader(new FileStream(jsonFileLocation, FileMode.Open));

                var jsonReader = new JsonTextReader(textReader);
                var jobj = JObject.Load(jsonReader);
                var f = jobj.First.First.Value<string>("path");
                textReader.Dispose();
                return f;
            //}
        }


    }


}
