using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestUtils
{
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
}
