using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Utils
{
    public  class HistogramTools
    {
        public static Dictionary<double, int> BinGenerator(double min, double max, double width)
        {
            if ((max - min) % width > 0.000001) throw new Exception();

            var count = (max - min) / width;
            var myDict = new Dictionary<double, int>();
            for (int i = 0; i <= count; i++) myDict.Add(min + width * i, 0);
            myDict.Add(Double.PositiveInfinity, 0);
            return myDict;
        }


        public static ConcurrentDictionary<double, ConcurrentBag<double>> CategoryGenerator(double min, double max, double width)
        {
            if ((max - min) % width > 0.000001) throw new Exception();

            var count = (max - min) / width;
            var myDict = new ConcurrentDictionary<double, ConcurrentBag<double>>();

            for (int i = 0; i <= count; i++) myDict.TryAdd(min + width * i, new ConcurrentBag<double>());
            myDict.TryAdd(Double.PositiveInfinity, new ConcurrentBag<double>());
            return myDict;
        }


        public static void CategoriseItem(Dictionary<double, int> myBins, double item)
        {
            for (int j = 0; j < myBins.Count; j++)
            {
                if (item < myBins.Keys.ToList()[j])
                {
                    myBins[myBins.Keys.ToList()[j]]++;
                    break;
                }
            }
        }

        public static void CategoriseItem(ConcurrentDictionary<double, ConcurrentBag<double>> myBins, double item, double bin) {
            var keys = myBins.Keys.ToList();
            for (int j = 0; j < myBins.Count - 1; j++) 
                if (bin < keys[j]) {
                    myBins[keys[j]].Add(item);
                    break;
                }
            if (bin > keys[^2]) myBins[keys[^1]].Add(item);
        }

        public static List<List<double>> GenerateHistorgramsFromCategories(ConcurrentDictionary<double, ConcurrentBag<double>> CategorisedLists, Dictionary<double, int> bins )
        {
            List<List<double>> results = new List<List<double>>();
            var categoryKeys = CategorisedLists.Keys.ToList();

            for (int i = 0; i < categoryKeys.Count; i++) {
                var values = CategorisedLists[categoryKeys[i]].ToList();
                for (int j = 0; j < CategorisedLists[categoryKeys[i]].Count; j++)
                    HistogramTools.CategoriseItem(bins, values[j]);
                
                results.Add(MakeCumulative(GenerateHistogram(bins)));
            }

            return results;
        }

        public static List<double> GenerateHistogram(Dictionary<double, int> myBins)
        {
            var totalItems = myBins.Values.Sum();
            var allBins = myBins.Keys.ToList();
            var list = new List<double>();
            for (int i = 0; i < allBins.Count; i++) list.Add(myBins[allBins[i]] / (double)totalItems);
            return list;
        }

        public static List<double> MakeCumulative(List<double> list)
        {
            for (int i = 1; i < list.Count; i++) list[i] = list[i-1] + list[i];
            return list;
        }
    }
}
