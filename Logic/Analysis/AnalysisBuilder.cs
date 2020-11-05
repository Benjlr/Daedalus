using System.Collections.Concurrent;
using Logic.Metrics;
using Logic.Utils;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;

namespace Logic.Analysis
{
    public class AnalysisBuilder
    {
        

        private ConcurrentDictionary<int,double> ExpectancyAverage;
        private ConcurrentDictionary<int,double> ExpectancyMedian;
        private ConcurrentDictionary<int,double> Winpercentage;
         
        private ConcurrentDictionary<int, List<List<double>>> ReturnByFbe;
        private ConcurrentDictionary<int, List<List<double>>> DrawdownByFbe;
        private ConcurrentDictionary<int, List<List<double>>> ReturnByDrawdown;
        private ConcurrentDictionary<int,List<double>> RollingExpectancy;

        private ConcurrentDictionary<double, List<double>> _returnByDrawdown;

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
                var histoStats = new HistogramStatitics(results[i],_lowerBound,_upperBound, _width);
                histoStats.GenerateHistogramStats(_returnByDrawdown);
                ReturnByFbe.TryAdd(i, HistogramTools.GenerateHistogram(histoStats.ResultHistogram));
                DrawdownByFbe.TryAdd(i,HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(histoStats.DrawdownHistogram)));
                AddGeneralStats(i, results[i]);
            }
            AddCategorisedAndBoundedStats();
        }
        
        private void InitListsAndLabels() {
            ExpectancyAverage = new ConcurrentDictionary<int, double>();
            ExpectancyMedian = new ConcurrentDictionary<int, double>() ;

            Winpercentage = new ConcurrentDictionary<int, double>();
            _returnByDrawdown = HistogramTools.CategoryGenerator(_lowerBound, _upperBound, _width);

            ReturnByFbe = new ConcurrentDictionary<int, List<List<double>>>();
            DrawdownByFbe = new ConcurrentDictionary<int, List<List<double>>>();
            ReturnByDrawdown = new ConcurrentDictionary<int, List<List<double>>>();
            RollingExpectancy = new ConcurrentDictionary<int, List<double>>();

            X_label = new List<string>();
            Y_label = new List<string>();
            X_label_categorised = new List<string>();
            Y_label_categorised = new List<string>();

            for (double i = _lowerBound; i <= _upperBound; i += _width) X_label.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _width; i += _width) X_label_categorised.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _upperBound; i += _width) Y_label_categorised.Add($"<{i:0.0%}");                       
        }
        

        private void AddGeneralStats(int i, ITest results) {
            ExpectancyAverage.TryAdd(i, results.ExpectancyAverage);
            ExpectancyMedian.TryAdd(i, results.ExpectancyMedian);
            Winpercentage.TryAdd(i, results.WinPercentage);
            RollingExpectancy.TryAdd(i, EntryTestDrilldown.GetRollingExpectancy(results.FBEResults.ToList(), 300));
        }



        private void AddCategorisedAndBoundedStats() {


            ReturnByDrawdown =  (HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdown, HistogramTools.BinGenerator(_lowerBound, 0, _width)));HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdown, HistogramTools.BinGenerator(_lowerBound, 0, _width));
        }
    }


    public class HistogramStatitics
    {
        private ITest _test { get; set; }
        public Dictionary<double, int> ResultHistogram { get; private set; }
        public Dictionary<double, int> DrawdownHistogram { get; private set; }

        public HistogramStatitics(ITest results, double lowerbound, double upperbound, double width)
        {
            _test = results;
            ResultHistogram = HistogramTools.BinGenerator(lowerbound, upperbound, width);
            DrawdownHistogram = HistogramTools.BinGenerator(lowerbound, 0, width);

        }

        public void GenerateHistogramStats(ConcurrentDictionary<double, List<double>> returnByDrawdown)
        {
            for (int j = 0; j < _test.FBEResults.Length; j++)
                if (_test.FBEResults[j] != 0) {
                    HistogramTools.CategoriseItem(returnByDrawdown, _test.FBEDrawdown[j], _test.FBEResults[j]);
                    HistogramTools.CategoriseItem(ResultHistogram, _test.FBEResults[j]);
                    if (_test.FBEResults[j] > 0) HistogramTools.CategoriseItem(DrawdownHistogram, _test.FBEDrawdown[j]);
                }
        }
    }


}
