using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DataStructures;
using Xunit;

namespace TestUtils
{
    public class TradeTimeMocker
    {
        public static DatedResult[] Mock(double[] vals, double[] drawDowns, DateTime startDate ) {
            long _mockTime  = startDate.Ticks;
            DatedResult[] dateResults = new DatedResult[vals.Length];
            iterateTime(_mockTime, vals, drawDowns, dateResults);
            return dateResults;
        }

        public static DatedResult[] Mock(double[] vals) {
            long _mockTime = new DateTime(2020, 01, 01).Ticks;
            DatedResult[] dateResults = new DatedResult[vals.Length];
            iterateTime(_mockTime ,vals, vals, dateResults);
            return dateResults;
        }

        private static void iterateTime(long startDate, double[] vals, double[] drawDowns, DatedResult[] dateResults) {
            for (int i = 0; i < vals.Length; i++) {
                dateResults[i] = new DatedResult(startDate, vals[i], drawDowns[i]);
                startDate += TimeSpan.FromDays(1).Ticks;
            }
        }
    }

    public class Loaders
    {
        
        public static List<List<double>> LoadData(string path, int listDepth) {
            var myLists = InitList(listDepth);
            var files = File.ReadAllLines(path);
            for (var i = 0; i < files.Length; i++) 
                ReadLine(files, i, myLists);
            
            return myLists;
        }

        public static List<double> LoadDataSingleColumn(string path) {
            var myLists = new List<double>();
            var files = File.ReadAllLines(path);
            for (var i = 0; i < files.Length; i++)
                myLists.Add(double.Parse(files[i]));
            
            return myLists;
        }

        private static List<List<double>> InitList(int listDepth) {
            var results = new List<List<double>>();
            for (int i = 0; i < listDepth; i++) results.Add(new List<double>());
            return results;
        }

        private static void ReadLine(string[] files, int i, List<List<double>> myLists) {
            var row = files[i].Split(',').ToList();
            for (int j = 0; j < row.Count; j++)
                myLists[j].Add(double.Parse(row[j]));
        }

    }


    public class RandomBars
    {
        private readonly Random _rand;
        private readonly TimeSpan _interval;
        private readonly List<BidAskData> _myMarket;
        private DateTime _date;

        public RandomBars(TimeSpan interval) {
            _rand = new Random(new Random().Next());
            _interval = interval;
            _myMarket = new List<BidAskData>();
            _date = new DateTime(1, 1, 1, 0, 0, 1);
        }

        public BidAskData[] GenerateRandomMarket(int bars) {
            for (int i = 0; i < bars; i++)
                 GenerateBar();
            return _myMarket.ToArray();
        }

        private void GenerateBar() {
            BidAsk(_date, out var open, out var close, out var high, out var low);
            _myMarket.Add(new BidAskData(open, high, low, close, (int) Math.Round(_rand.NextDouble() * 20)));
            _date = _date.Add(_interval);
        }

        private void BidAsk(DateTime startDate, out BidAsk open, out BidAsk close, out BidAsk high, out BidAsk low ) {
            var openPrice = GeneratePrice(20,3);
            var highPrice = openPrice + GenerateHigherPrice(3);
            var lowPrice = openPrice - GenerateHigherPrice(3);
            var closePrice = CheckClosePrice(highPrice, GeneratePrice(20, 3), lowPrice);
            open = new BidAsk(openPrice, openPrice + 0.5, startDate.Ticks);
            close = new BidAsk(closePrice, closePrice + 0.5, startDate.Ticks);
            high = new BidAsk(highPrice, highPrice + 0.5, startDate.Ticks);
            low = new BidAsk(lowPrice, lowPrice + 0.5, startDate.Ticks);
        }

        private double CheckClosePrice(double highPrice, double closePrice, double lowPrice) {
            if (highPrice < closePrice) closePrice = highPrice;
            if (lowPrice > closePrice) closePrice = lowPrice;
            return closePrice;
        }

        private double GeneratePrice(int basePrice, double variance) {
            return basePrice + (_rand.NextDouble() - 0.5) * variance;
        }

        private double GenerateHigherPrice(double variance) {
            return _rand.NextDouble() * variance;
        }
    }
}
