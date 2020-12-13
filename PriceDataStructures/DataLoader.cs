using System;
using System.IO;
using System.Linq;

namespace DataStructures
{
    public class DataLoader
    {

        public static BidAskData[] LoadData(string file) {
            var fs = File.ReadAllLines(file);
            if (fs?.FirstOrDefault()?.Split(',').Length == 10) return LoadBidAskData(fs);
            else if(fs?.FirstOrDefault()?.Split(',').Length == 6) return LoadConsolidatedData(fs);
            else throw new Exception("Not valid data");
        }

        private static BidAskData[] LoadConsolidatedData(string[] lines) {
            var myArray = new BidAskData[lines.Length];

            for (int i = 0; i < lines.Length; i++) {
                var myLine = lines[i].Split(',');
                var date = DateTime.ParseExact(myLine[0], "yyyy/MM/dd", null);

                myArray[i] = new BidAskData(
                    new BidAsk(double.Parse(myLine[1]), double.Parse(myLine[1]),date),
                    new BidAsk(double.Parse(myLine[2]), double.Parse(myLine[2]),date),
                    new BidAsk(double.Parse(myLine[3]), double.Parse(myLine[3]), date),
                    new BidAsk(double.Parse(myLine[4]), double.Parse(myLine[4]), date),
                    double.Parse(myLine[5]));
            }

            return myArray;

        }

        private static BidAskData[] LoadBidAskData(string[] lines) {
            var myArray = new BidAskData[lines.Length];

            for (int i = 0; i < lines.Length; i++) {
                var myLine = lines[i].Split(',');
                var date = DateTime.ParseExact(myLine[0], "yyyy/MM/dd HH:mm:ss", null);
                var open = new BidAsk(double.Parse(myLine[2]), double.Parse(myLine[1]), date);
                var high = new BidAsk(double.Parse(myLine[4]), double.Parse(myLine[3]), date);
                var low = new BidAsk(double.Parse(myLine[6]), double.Parse(myLine[5]), date);
                var close = new BidAsk(double.Parse(myLine[8]), double.Parse(myLine[7]), date) ;
                myArray[i] = new BidAskData(open,high,low,close, double.Parse(myLine[9]));
            }

            return myArray;
        }

    }
}