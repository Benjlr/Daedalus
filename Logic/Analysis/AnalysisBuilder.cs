using Logic.Metrics;
using Logic.Utils;
using System.Collections.Generic;
using System.Linq;
using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;

namespace Logic.Analysis
{
    public class AnalysisBuilder
    {
        public List<double> ExpectancyAverage;
        public List<double> ExpectancyMedian;
        public List<double> Winpercentage;

        public List<List<double>> ReturnByFbe;
        public List<List<double>> DrawdownByFbe;
        public List<List<double>> ReturnByDrawdown;
        public List<List<double>> RollingExpectancy;

        private Dictionary<double, int> _returnByFbe;
        private Dictionary<double, int> _drawdownByFbe;
        private Dictionary<double, List<double>> _returnByDrawdown;

        private double _lowerBound = -0.02;
        private double _upperBound = 0.02;
        private double _width = 1.0 / 1000;

        public List<string> X_label_categorised;
        public List<string> Y_label_categorised;
        public List<string> X_label;
        public List<string> Y_label;

        public void GenerateFixedBarResults(List<ITest> results) {
            InitListsAndLabels();
            for (int i = 0; i < results.Count; i++) {
                InitHistogramsForCurrentTest();
                GetStatistics(results[i]);
                AddGeneralStats(results[i]);
                AddHistogramStats();
            }
            AddCategorisedAndBoundedStats();
        }
        
        private void InitListsAndLabels() {
            ExpectancyAverage = new List<double>();
            ExpectancyMedian = new List<double>();

            Winpercentage = new List<double>();
            _returnByDrawdown = HistogramTools.CategoryGenerator(_lowerBound, _upperBound, _width);

            ReturnByFbe = new List<List<double>>();
            DrawdownByFbe = new List<List<double>>();
            ReturnByDrawdown = new List<List<double>>();
            RollingExpectancy = new List<List<double>>();

            X_label = new List<string>();
            Y_label = new List<string>();
            X_label_categorised = new List<string>();
            Y_label_categorised = new List<string>();

            for (double i = _lowerBound; i <= _upperBound; i += _width) X_label.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _width; i += _width) X_label_categorised.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _upperBound; i += _width) Y_label_categorised.Add($"<{i:0.0%}");                       
        }

        private void InitHistogramsForCurrentTest() {
            _returnByFbe = HistogramTools.BinGenerator(_lowerBound, _upperBound, _width);
            _drawdownByFbe = HistogramTools.BinGenerator(_lowerBound, 0, _width);
        }
        private void GetStatistics(ITest results) {
            for (int j = 0; j < results.FBEResults.Length; j++)
                if (results.FBEResults[j] !=  0)
                    HistogramOperations(results, j);
        }

        private void HistogramOperations(ITest results, int j) {
            HistogramTools.CategoriseItem(_returnByDrawdown, results.FBEDrawdown[j], results.FBEResults[j]);
            HistogramTools.CategoriseItem(_returnByFbe, results.FBEResults[j]);
            if (results.FBEResults[j] > 0) HistogramTools.CategoriseItem(_drawdownByFbe, results.FBEDrawdown[j]);
        }

        private void AddGeneralStats(ITest results) {
            ExpectancyAverage.Add(results.ExpectancyAverage);
            ExpectancyMedian.Add(results.ExpectancyMedian);
            Winpercentage.Add(results.WinPercentage);
            RollingExpectancy.Add(EntryTestDrilldown.GetRollingExpectancy(results.FBEResults.ToList(), 300));
        }

        private void AddHistogramStats() {
            ReturnByFbe.Add(HistogramTools.GenerateHistogram(_returnByFbe));
            DrawdownByFbe.Add(HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownByFbe)));
        }

        private void AddCategorisedAndBoundedStats() {
            ReturnByDrawdown = HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdown, HistogramTools.BinGenerator(_lowerBound, 0, _width));
        }
    }
}
