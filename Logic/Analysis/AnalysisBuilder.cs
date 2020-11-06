using Logic.Analysis.Metrics.EntryTests.TestsDrillDown;
using Logic.Metrics;
using Logic.Utils;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Analysis
{
    public class AnalysisBuilder
    {
        public List<double> ExpectancyAverage { get; private set; }
        public List<double> ExpectancyMedian { get; private set; }
        public List<double> WinPercentage { get; private set; }
        public List<List<double>> ReturnByFbe { get; private set; }
        public List<List<double>> DrawdownByFbe { get; private set; }
        public List<List<double>> RollingExpectancy { get; private set; }

        private System.Action UpdateOnProgress;

        private ConcurrentDictionary<int, double> _expectancyAverage;
        private ConcurrentDictionary<int, double> _expectancyMedian;
        private ConcurrentDictionary<int, double> _winpercentage;

        private ConcurrentDictionary<int, List<double>> _returnByFbe;
        private ConcurrentDictionary<int, List<double>> _drawdownByFbe;
        private ConcurrentDictionary<int, List<double>> _rollingExpectancy;
        public List<List<double>> ReturnByDrawdown;

        private ConcurrentDictionary<double, List<double>> _returnByDrawdown;

        private double _lowerBound = -0.02;
        private double _upperBound = 0.02;
        private double _width = 1.0 / 1000;

        public List<string> X_label_categorised;
        public List<string> Y_label_categorised;
        public List<string> X_label;
        public List<string> Y_label;

        public AnalysisBuilder(System.Action subscriber)
        {
            UpdateOnProgress = subscriber;
        }

        public void GenerateFixedBarResults(List<ITest> results)
        {

            InitListsAndLabels();
            Parallel.For(0, results.Count, (i) =>
              {
                  var histoStats = new HistogramStatistics(results[i], _lowerBound, _upperBound, _width);
                  histoStats.GenerateHistogramStats(_returnByDrawdown);
                  _returnByFbe.TryAdd(i, HistogramTools.GenerateHistogram(histoStats.ResultHistogram));
                  _drawdownByFbe.TryAdd(i, HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(histoStats.DrawdownHistogram)));
                  AddGeneralStats(i, results[i]);
                  UpdateOnProgress?.Invoke();
              });
            AddCategorisedAndBoundedStats();
            InitialiseAndSortPublicLists(results.Count);
        }

        private void InitListsAndLabels()
        {
            _expectancyAverage = new ConcurrentDictionary<int, double>();
            _expectancyMedian = new ConcurrentDictionary<int, double>();

            _winpercentage = new ConcurrentDictionary<int, double>();
            _returnByDrawdown = new ConcurrentDictionary<double, List<double>>(HistogramTools.CategoryGenerator(_lowerBound, _upperBound, _width));

            _returnByFbe = new ConcurrentDictionary<int, List<double>>();
            _drawdownByFbe = new ConcurrentDictionary<int, List<double>>();
            _rollingExpectancy = new ConcurrentDictionary<int, List<double>>();
            ReturnByDrawdown = new List<List<double>>();

            X_label = new List<string>();
            Y_label = new List<string>();
            X_label_categorised = new List<string>();
            Y_label_categorised = new List<string>();

            for (double i = _lowerBound; i <= _upperBound; i += _width) X_label.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _width; i += _width) X_label_categorised.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _upperBound; i += _width) Y_label_categorised.Add($"<{i:0.0%}");
        }


        private void AddGeneralStats(int i, ITest results)        {
            _expectancyAverage.TryAdd(i, results.ExpectancyAverage);
            _expectancyMedian.TryAdd(i, results.ExpectancyMedian);
            _winpercentage.TryAdd(i, results.WinPercentage);
            _rollingExpectancy.TryAdd(i, EntryTestDrilldown.GetRollingExpectancy(results.FBEResults.ToList(), 600));
        }

        private void AddCategorisedAndBoundedStats()        {
            ReturnByDrawdown = HistogramTools.GenerateHistorgramsFromCategories(
                new Dictionary<double, List<double>>(_returnByDrawdown),
                HistogramTools.BinGenerator(_lowerBound, 0, _width));
        }

        private void InitialiseAndSortPublicLists(int count)
        {
            ExpectancyAverage = new List<double>();
            ExpectancyMedian = new List<double>();
            WinPercentage = new List<double>();
            ReturnByFbe = new List<List<double>>();
            DrawdownByFbe = new List<List<double>>();
            RollingExpectancy = new List<List<double>>();

            for (int i = 0; i < count; i++)
            {
                ExpectancyAverage.Add(_expectancyAverage[i]);
                ExpectancyMedian.Add(_expectancyMedian[i]);
                WinPercentage.Add(_winpercentage[i]);
                ReturnByFbe.Add(_returnByFbe[i]);
                DrawdownByFbe.Add(_drawdownByFbe[i]);
                RollingExpectancy.Add(_rollingExpectancy[i]);
            }
        }
    }


    public class HistogramStatistics
    {
        private ITest _test { get; set; }
        public Dictionary<double, int> ResultHistogram { get; private set; }
        public Dictionary<double, int> DrawdownHistogram { get; private set; }

        public HistogramStatistics(ITest results, double lowerbound, double upperbound, double width)
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
