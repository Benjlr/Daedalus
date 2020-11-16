using System;
using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using Logic.Utils;
using PriceSeriesCore;

namespace Logic.Analysis.Metrics
{
    public abstract class TestBase : ITest
    {
        public double[] FBEResults { get; protected set; }
        public double[] FBEDrawdown { get; protected set; }
        public int[] Durations { get; protected set; }
        public ExtendedStats Stats { get; protected set; }

        protected int _endIndex { get; set; }

        public void Run(MarketData[] data, bool[] entries, List<Session> myInputs = null) {
            initLists(data.Length);
            IterateEntries(data, entries);
            GenerateStats();
        }

        protected void initLists(int length) {
            FBEResults = new double[length];
            FBEDrawdown = new double[length];
            Durations = new int[length];
        }

        protected void IterateEntries(MarketData[] data, bool[] entries) {
            for (int i = 1; i < entries.Length - _endIndex; i++) 
                if (entries[i - 1])
                    PerformEntryActions(data, i);
        }

        protected void PerformEntryActions(MarketData[] data, int i) {
            SetResult(data, i);
            FindDrawdown(data, i);
        }

        protected abstract void SetResult(MarketData[] data, int i);

        protected void FindDrawdown(MarketData[] data, int i) {
            IterateTime(data, i);
            if (FBEResults[i] < FBEDrawdown[i])
                FBEDrawdown[i] = FBEResults[i];
        }

        protected abstract void IterateTime(MarketData[] data, int i);

        private void GenerateStats() {
            Stats = new ExtendedStats(FBEResults.ToList(), FBEDrawdown.ToList());
        }

        public void RemoveLeakage() {
            var testCopy = (double[])FBEResults.Clone();
            var drawdwonCopy = (double[])FBEDrawdown.Clone();
            for (int i = 0; i < FBEResults.Length; i++) {
                FBEResults[i] = 0;
                FBEDrawdown[i] = 0;
            }
            for (int i = 0; i < FBEResults.Length; i++)
                if (testCopy[i] != 0 && Durations[i] + i < FBEResults.Length) {
                    FBEResults[i + Durations[i]] += testCopy[i];
                    FBEDrawdown[i + Durations[i]] += drawdwonCopy[i];
                }

            GenerateStats();
        }
    }
}


