using System.Collections.Generic;

namespace DataStructures.StatsTools
{
    public class EpochGenerator
    {
        public int Period { get; set; }
        public int Remainder { get; set; }
        public List<List<double>> EpochContainer { get; set; }

        private EpochGenerator(int count, int divisions) {
            if (count % divisions == 0) {
                Remainder = 0;
                Period = count / divisions;
            }
            else {
                Period = count / (divisions - 1);
                Remainder = count % (divisions - 1);
            }
            EpochContainer = new List<List<double>>();
        }

        public static EpochGenerator SplitListIntoEpochs(List<double> list, int divisions) {
            var myEpoch = new EpochGenerator(list.Count, divisions);
            myEpoch.GenerateEpochs(list);
            return myEpoch;
        }

        private void GenerateEpochs(List<double> list) {
            InitialiseFirstEpoch(list);
            GenerateRemainingEpochs(list);
        }

        private void InitialiseFirstEpoch(List<double> list) {
            if (Remainder > 0)
                EpochContainer.Add(ListTools.GetNewListByStartIndexAndCount(list, 0, Remainder));
        }

        private void GenerateRemainingEpochs(List<double> list) {
            for (int i = Remainder; i < list.Count; i += Period)
                EpochContainer.Add(ListTools.GetNewListByIndex(list, i, i + Period - 1));
        }

    }
}