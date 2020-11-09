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
        public List<List<double>> ReturnByTest { get; private set; }
        public List<List<double>> DrawdownByTest { get; private set; }
        public List<List<double>> RollingExpectancy { get; private set; }
        public List<List<double>> ReturnByDrawdown { get; private set; }

        public List<string> X_label_categorised;
        public List<string> Y_label_categorised;
        public List<string> X_label;
        public List<string> Y_label;

        private List<AnalysisState> _analyses { get; set; }
        private System.Action UpdateOnProgress { get; set; }

        private static BinDescriptor _binSizing = new BinDescriptor(-0.02, 0.02, 1.0 / 1000);

        public AnalysisBuilder(System.Action subscriber) {
            UpdateOnProgress = subscriber;
            _analyses = new List<AnalysisState>();
        }

        public void GenerateFixedBarResults(List<ITest> results) {
            InitListsAndLabels();
            Parallel.For(0, results.Count, (i) => {
                  _analyses.Add(new AnalysisState(results[i], _binSizing, i));
                  UpdateOnProgress?.Invoke();
              });
            AddCategorisedAndBoundedStats();
            InitialiseAndSortPublicLists();
        }

        private void InitListsAndLabels()
        {
            ReturnByDrawdown = new List<List<double>>();

            X_label = new List<string>();
            Y_label = new List<string>();
            X_label_categorised = new List<string>();
            Y_label_categorised = new List<string>();

            //for (double i = _lowerBound; i <= _upperBound; i += _width) X_label.Add($"<{i:0.0%}");
            //for (double i = _lowerBound; i <= _width; i += _width) X_label_categorised.Add($"<{i:0.0%}");
            //for (double i = _lowerBound; i <= _upperBound; i += _width) Y_label_categorised.Add($"<{i:0.0%}");
        }


        private void AddCategorisedAndBoundedStats() {
            ReturnByDrawdown = HistogramTools.GenerateHistorgramsFromCategories(
                HistogramTools.CollateCategories(_analyses.Select(x=>x._histoStats.DrawdownByReturn).ToList(), _binSizing),
                _binSizing);
        }

        private void InitialiseAndSortPublicLists(){
            _analyses = _analyses.OrderBy(x => x.Position).ToList();
            ExpectancyAverage = _analyses.Select(x => x.ExpectancyAverage).ToList();
            ExpectancyMedian = _analyses.Select(x => x.ExpectancyMedian).ToList();
            WinPercentage = _analyses.Select(x => x.WinPercentage).ToList();
            ReturnByTest = _analyses.Select(x => x._histoStats.ResultHistogram).ToList();
            DrawdownByTest = _analyses.Select(x => x._histoStats.DrawddownHistogram).ToList();
            RollingExpectancy = _analyses.Select(x => x.RollingExpectancy).ToList();
        }
    }


    public class AnalysisState
    {
        public int Position { get; }
        public HistogramStatistics _histoStats { get; }

        public double ExpectancyAverage { get; private set; }
        public double ExpectancyMedian { get; private set; }
        public double WinPercentage { get; private set; }

        public List<double> RollingExpectancy { get; private set; }


        public AnalysisState(ITest result, BinDescriptor bin, int i)
        {
            Position = i;
            _histoStats = new HistogramStatistics(result, bin);
            AddGeneralStats(result);
        }

        private void AddGeneralStats(ITest results)
        {
            ExpectancyAverage = results.ExpectancyAverage;
            ExpectancyMedian = results.ExpectancyMedian;
            WinPercentage = (results.WinPercentage);
            RollingExpectancy = (EntryTestDrilldown.GetRollingExpectancy(results.FBEResults.ToList(), 100));
        }
    }

    public class HistogramStatistics
    {
        private Dictionary<double, int> _resultHistogramBuilder { get; set; }
        private Dictionary<double, int> _drawdownHistogramBuilder { get; set; }
        public List<double> ResultHistogram { get; set; }
        public List<double> DrawddownHistogram { get; set; }
        public Dictionary<double, List<double>> DrawdownByReturn { get; set; }

        public HistogramStatistics(ITest results, BinDescriptor bin)
        {
            _resultHistogramBuilder = HistogramTools.BinGenerator(bin);
            _drawdownHistogramBuilder = HistogramTools.BinGenerator(bin);
            DrawdownByReturn = HistogramTools.CategoryGenerator(bin);

            GenerateHistogramStats(results);
            Finalise();

        }

        private void GenerateHistogramStats(ITest results)
        {
            for (int j = 0; j < results.FBEResults.Length; j++)
                if (results.FBEResults[j] != 0) {
                    HistogramTools.CategoriseItem(DrawdownByReturn, results.FBEDrawdown[j], results.FBEResults[j]);
                    HistogramTools.CategoriseItem(_resultHistogramBuilder, results.FBEResults[j]);
                    if (results.FBEResults[j] > 0) HistogramTools.CategoriseItem(_drawdownHistogramBuilder, results.FBEDrawdown[j]);
                }
        }

        private void Finalise()
        {
            ResultHistogram = HistogramTools.GenerateHistogram(_resultHistogramBuilder);
            DrawddownHistogram = HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownHistogramBuilder));
        }
    }


}
