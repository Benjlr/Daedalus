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


        public static Dictionary<double, List<double>> CategoryGenerator(double min, double max, double width)
        {
            if ((max - min) % width > 0.000001) throw new Exception();

            var count = (max - min) / width;
            var myDict = new Dictionary<double, List<double>>();

            for (int i = 0; i <= count; i++) myDict.Add(min + width * i, new List<double>());
            myDict.Add(Double.PositiveInfinity, new List<double>());
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

        public static void CategoriseItem(Dictionary<double, List<double>> myBins, double item, double bin)
        {
            for (int j = 0; j < myBins.Count - 1; j++)
            {
                if (bin < myBins.Keys.ToList()[j])
                {
                    myBins[myBins.Keys.ToList()[j]].Add(item);
                    break;
                }
            }
            if (bin > myBins.Keys.ToList()[myBins.Keys.Count - 2]) myBins[myBins.Keys.ToList()[myBins.Keys.Count - 1]].Add(item);
        }

        public static void CategoriseItem(ConcurrentDictionary<double, List<double>> myBins, double item, double bin)
        {
            for (int j = 0; j < myBins.Count - 1; j++)
            {
                if (bin < myBins.Keys.ToList()[j])
                {
                    myBins[myBins.Keys.ToList()[j]].Add(item);
                    break;
                }
            }
            if (bin > myBins.Keys.ToList()[myBins.Keys.Count - 2]) myBins[myBins.Keys.ToList()[myBins.Keys.Count - 1]].Add(item);
        }

        public static List<List<double>> GenerateHistorgramsFromCategories(Dictionary<double, List<double>> CategorisedLists, Dictionary<double, int> bins )
        {
            List<List<double>> results = new List<List<double>>();
            var categoryKeys = CategorisedLists.Keys.ToList();

            for (int i = 0; i < categoryKeys.Count; i++)
            {
                var DrawddownBins = new Dictionary<double, int>(bins);
                for (int j = 0; j < CategorisedLists[categoryKeys[i]].Count; j++) HistogramTools.CategoriseItem(DrawddownBins, CategorisedLists[categoryKeys[i]][j]);
                results.Add(MakeCumulative(GenerateHistogram(DrawddownBins)));
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
