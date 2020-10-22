using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Logic.Utils
{
    public class Markets
    {
        public static readonly string ASX200_Cash_5_Min = @"C:\Users\rober\Dropbox\Testing\asx200cash";
        public static readonly string AUD_USD_5_Min = @"C:\Users\rober\Dropbox\Testing\AUDUSD";
        public static readonly string ASX200_Cash_Daily = @"C:\Applications\Trading Data\CSV\Indices\XJO.csv";
        public static readonly string Bitcoin = @"C:\Users\rober\Dropbox\Testing\bitcoin";
        public static readonly string test_data = @"C:\Users\rober\Dropbox\Testing\testdata";
        //public static readonly string ASX200_Cash_5_Min = @"C:\Users\Ben Roberts\Dropbox\Testing\asx200cash";
        public static readonly string SP500_Cash_5_Min = @"C:\Users\rober\Dropbox\Testing\sp500cash";
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

    }


}
