using System.Collections.Generic;
using System.Linq;
using PriceSeriesCore.Calculations;

namespace Logic.Utils
{
    public class ExpectancyTools
    {

         public static List<TradeStatistics> GetRollingExpectancy(List<double> resultList, int lookbackPeriod)
        {
            return RunThroughResultSet(resultList, lookbackPeriod);
        }

        public static List<TradeStatistics> GetExpectancyByEpoch(List<double> resultList, int divisions)
        {
            return IterateThroughEpochs(EpochGenerator.SplitListIntoEpochs(resultList, divisions).EpochContainer);
        }

        private static List<TradeStatistics> IterateThroughEpochs(List<List<double>> epochs)
        {
            var retval = new List<TradeStatistics>();
            foreach (var epoch in epochs)
            {
                var vals = epoch.Where(x => x != 0).ToList();
                retval.Add(IterateExpectancy(vals));
            }
            return retval;
        }

        private static List<TradeStatistics> AddOnes(int initPeriod)
        {
            var myList = new List<TradeStatistics>();
            for (int i = 0; i < initPeriod; i++)
                myList.Add(new TradeStatistics(new List<double>()));
            return myList;
        }

        private static List<TradeStatistics> RunThroughResultSet(List<double> resultList, int lookbackPeriod)
        {
            var indexThresh = ListTools.GetIndexAtThresholdNonZeroes(lookbackPeriod, resultList);
            var retVal = AddOnes(indexThresh);
            List<double> validResults = new List<double>();

            for (int i = 0; i < indexThresh; i++)
            {
                if (resultList[i] != 0) validResults.Add(resultList[i]);
            }

            for (int i = retVal.Count; i < resultList.Count; i++)
                if (resultList[i] == 0) retVal.Add(retVal.Last());
                else
                {
                    if (validResults.Count >= lookbackPeriod) validResults.RemoveAt(0);
                    validResults.Add(resultList[i]);
                    retVal.Add(IterateExpectancy(validResults));
                }
            return retVal;
        }

        private static TradeStatistics IterateExpectancy(List<double> resultsList)
        {
            if (resultsList.Count == 0) return new TradeStatistics(new List<double>());
            return new TradeStatistics(resultsList);
        }
    }

    public class EpochGenerator
    {
        public int Period { get; set; }
        public int Remainder { get; set; }
        public List<List<double>> EpochContainer { get; set; }

        private EpochGenerator(int count, int divisions)
        {
            if (count % divisions == 0)
            {
                Remainder = 0;
                Period = count / divisions;
            }
            else
            {
                Period = count / (divisions - 1);
                Remainder = count % (divisions - 1);
            }
            EpochContainer = new List<List<double>>();
        }

        public static EpochGenerator SplitListIntoEpochs(List<double> list, int divisions)
        {
            var myEpoch = new EpochGenerator(list.Count, divisions);
            myEpoch.GenerateEpochs(list);
            return myEpoch;
        }

        private void GenerateEpochs(List<double> list)
        {
            InitialiseFirstEpoch(list);
            GenerateRemainingEpochs(list);
        }

        private void InitialiseFirstEpoch(List<double> list)
        {
            if (Remainder > 0)
                EpochContainer.Add(ListTools.GetNewListByStartIndexAndCount(list, 0, Remainder));
        }

        private void GenerateRemainingEpochs(List<double> list)
        {
            for (int i = Remainder; i < list.Count; i += Period)
            {
                EpochContainer.Add(ListTools.GetNewListByIndex(list, i, i + Period - 1));
            }
        }

    }
}
