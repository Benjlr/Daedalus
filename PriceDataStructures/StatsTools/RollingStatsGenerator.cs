using System.Collections.Generic;
using System.Linq;

namespace DataStructures.StatsTools
{
    public class RollingStatsGenerator
    {
        public static List<TradeStatistics> GetRollingStats(List<double> resultList, int lookbackPeriod) {
            return RunThroughResultSet(resultList, lookbackPeriod);
        }

        public static List<TradeStatistics> GetStatsByEpoch(List<double> resultList, int divisions) {
            return IterateThroughEpochs(EpochGenerator.SplitListIntoEpochs(resultList, divisions).EpochContainer);
        }

        private static List<TradeStatistics> IterateThroughEpochs(List<List<double>> epochs) {
            var retval = new List<TradeStatistics>();
            foreach (var epoch in epochs) {
                var vals = epoch.Where(x => x != 0).ToList();
                retval.Add(IterateStats(vals));
            }
            return retval;
        }

        private static List<TradeStatistics> AddOnes(int initPeriod) {
            var myList = new List<TradeStatistics>();
            for (int i = 0; i < initPeriod; i++)
                myList.Add(new TradeStatistics(new List<double>()));
            return myList;
        }

        private static List<TradeStatistics> RunThroughResultSet(List<double> resultList, int lookbackPeriod) {
            var indexThresh = ListTools.GetIndexAtThresholdNonZeroes(lookbackPeriod, resultList);
            var retVal = AddOnes(indexThresh);
            List<double> validResults = new List<double>();

            for (int i = 0; i < indexThresh; i++)
                if (resultList[i] != 0) validResults.Add(resultList[i]);

            for (int i = retVal.Count; i < resultList.Count; i++)
                if (resultList[i] == 0) retVal.Add(retVal.Last());
                else {
                    if (validResults.Count >= lookbackPeriod) validResults.RemoveAt(0);
                    validResults.Add(resultList[i]);
                    retVal.Add(IterateStats(validResults));
                }
            return retVal;
        }

        private static TradeStatistics IterateStats(List<double> resultsList)
        {
            if (resultsList.Count == 0) return new TradeStatistics(new List<double>());
            return new TradeStatistics(resultsList);
        }
    }
}
