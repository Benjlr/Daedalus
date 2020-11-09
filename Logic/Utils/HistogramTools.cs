using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Utils
{
    public  class HistogramTools
    {
        public static Dictionary<double, int> BinGenerator(BinDescriptor bin)
        {
            if ((bin.UpperBound - bin.LowerBound) % bin.Width> 0.000001) throw new Exception();

            var count = (bin.UpperBound - bin.LowerBound) / bin.Width;
            var myDict = new Dictionary<double, int>();
            for (int i = 0; i <= count; i++) myDict.Add(bin.LowerBound+ bin.Width * i, 0);
            myDict.Add(Double.PositiveInfinity, 0);
            return myDict;
        }


        public static Dictionary<double, List<double>> CategoryGenerator(BinDescriptor bin) {
            if ((bin.UpperBound - bin.LowerBound) % bin.Width> 0.000001) throw new Exception();

            var count = (bin.UpperBound - bin.LowerBound) / bin.Width;
            var myDict = new Dictionary<double, List<double>>();

            for (int i = 0; i <= count; i++) myDict.TryAdd(bin.LowerBound + bin.Width * i, new List<double>());
            myDict.TryAdd(Double.PositiveInfinity, new List<double>());
            return myDict;
        }


        public static void CategoriseItem(Dictionary<double, int> myBins, double item) {
            var keys = myBins.Keys.ToList();
            for (int j = 0; j < myBins.Count; j++)
                if (item < keys[j]) {
                    myBins[keys[j]]++;
                    break;
                }
        }

        public static void CategoriseItem(Dictionary<double, List<double>> myBins, double item, double bin) {
            var keys = myBins.Keys.ToList();
            for (int j = 0; j < myBins.Count - 1; j++) 
                if (bin < keys[j]) {
                    myBins[keys[j]].Add(item);
                    break;
                }
            if (bin > keys[^2]) myBins[keys[^1]].Add(item);
        }

        public static List<List<double>> GenerateHistorgramsFromCategories(Dictionary<double, List<double>> CategorisedLists, BinDescriptor bins) {
            List<List<double>> results = new List<List<double>>();
            var categoryKeys = CategorisedLists.Keys.ToList();
            for (int i = 0; i < categoryKeys.Count; i++)
                results.Add(MakeCumulative(CrosslinkCategories(CategorisedLists[categoryKeys[i]], BinGenerator(bins))));
            return results;
        }
        private static List<double> CrosslinkCategories(List<double> values, Dictionary<double, int> bins)        {
            for (int j = 0; j < values.Count; j++)
                CategoriseItem(bins, values[j]);
            return GenerateHistogram(bins);
        }

        public static Dictionary<double, List<double>> CollateCategories(List<Dictionary<double, List<double>>> ListOfCategorisedLists, BinDescriptor bin) {
            var retVal = CategoryGenerator(bin);
            for (int i = 0; i < ListOfCategorisedLists.Count; i++)
                foreach (var item in ListOfCategorisedLists[i])
                    retVal[item.Key].AddRange(item.Value);

            return retVal;
        }

        public static List<double> GenerateHistogram(Dictionary<double, int> myBins) {
            var totalItems = myBins.Values.Sum();
            var allBins = myBins.Keys.ToList();
            var list = new List<double>();
            for (int i = 0; i < allBins.Count; i++) list.Add(myBins[allBins[i]] / (double)totalItems);
            return list;
        }

        public static List<double> MakeCumulative(List<double> list) {
            for (int i = 1; i < list.Count; i++) list[i] = list[i-1] + list[i];
            return list;
        }
    }

    public struct BinDescriptor
    {
        public double LowerBound { get; set; }
        public double UpperBound { get; set; }
        public double Width { get; set; }

        public BinDescriptor(double lowerBound, double upperBound, double width)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
            Width = width;
        }
    }
}
