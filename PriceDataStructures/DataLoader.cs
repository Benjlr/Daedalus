using System;
using System.IO;
using System.Linq;

namespace DataStructures
{
    public class DataLoader
    {

        public static Type CheckDataType(string file) {
            var fs = File.ReadAllLines(file);
            if (fs?.FirstOrDefault()?.Split(',').Length == 10) return typeof(BidAskData);
            else if(fs?.FirstOrDefault()?.Split(',').Length == 6) return typeof(SessionData);
            else throw new Exception("Not valid data");
        }

        public static SessionData[] LoadConsolidatedData(string location) {
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

        public static BidAskData[] LoadBidAskData(string location) {
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

    }
}