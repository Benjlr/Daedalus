using Logic.Metrics;
using Logic.Metrics.EntryTests.TestsDrillDown;
using Logic.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Analysis
{
    public class PlaceholderName
    {
        public List<double> ExpectancyLongAvg;
        public List<double> ExpectancyShortAvg;
        public List<double> ExpectancyLongMed;
        public List<double> ExpectancyShortMed;
        public List<double> WinpercentageLong;
        public List<double> WinpercentageShort;

        public List<List<double>> ReturnByFbeLong;
        public List<List<double>> DrawdownByFbeLong;
        public List<List<double>> ReturnByFbeShort;
        public List<List<double>> DrawdownByFbeShort;
        public List<List<double>> ReturnByDrawdownShort;
        public List<List<double>> ReturnByDrawdownLong;
        public List<List<double>> RollingExpectancyLong;
        public List<List<double>> RollingExpectancyShort;

        private Dictionary<double, int> _returnByFbe;
        private Dictionary<double, int> _drawdownByFbe;
        private Dictionary<double, int> _returnByFbeShort;
        private Dictionary<double, int> _drawdownByFbeShort;
        private Dictionary<double, List<double>> _returnByDrawdown;
        private Dictionary<double, List<double>> _returnByDrawdownShort;


        private double _lowerBound = -0.02;
        private double _upperBound = 0.02;
        private double _width = 1.0 / 1000;

        public List<string> X_label_categorised;
        public List<string> Y_label_categorised;
        public List<string> X_label;
        public List<string> Y_label;

        public void GenerateFixedBarResults(List<ITest[]> results)
        {
            InitListsAndLabels();
            for (int i = 0; i < results.Count; i++)
            {
                InitHistogramsForCurrentTest();
                GetStatistics(results[i]);
                AddGeneralStats(results[i]);
                AddHistogramStats();
            }

            AddCategorisedAndBoundedStats();
        }
        
        private void InitListsAndLabels()
        {
            ExpectancyLongAvg = new List<double>();
            ExpectancyShortAvg = new List<double>();
            ExpectancyLongMed = new List<double>();
            ExpectancyShortMed = new List<double>();
            
            WinpercentageLong = new List<double>();
            WinpercentageShort = new List<double>();

            _returnByDrawdown = HistogramTools.CategoryGenerator(_lowerBound, _upperBound, _width);
            _returnByDrawdownShort = HistogramTools.CategoryGenerator(_lowerBound, _upperBound, _width);

            ReturnByFbeLong = new List<List<double>>();
            DrawdownByFbeLong = new List<List<double>>();
            ReturnByFbeShort = new List<List<double>>();
            DrawdownByFbeShort = new List<List<double>>();
            ReturnByDrawdownShort = new List<List<double>>();
            ReturnByDrawdownLong = new List<List<double>>();
            RollingExpectancyLong = new List<List<double>>();
            RollingExpectancyShort = new List<List<double>>();


            X_label = new List<string>();
            Y_label = new List<string>();
            X_label_categorised = new List<string>();
            Y_label_categorised = new List<string>();

            for (double i = _lowerBound; i <= _upperBound; i += _width) X_label.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _width; i += _width) X_label_categorised.Add($"<{i:0.0%}");
            for (double i = _lowerBound; i <= _upperBound; i += _width) Y_label_categorised.Add($"<{i:0.0%}");

        }

        private void InitHistogramsForCurrentTest()
        {
            _returnByFbe = HistogramTools.BinGenerator(_lowerBound, _upperBound, _width);
            _drawdownByFbe = HistogramTools.BinGenerator(_lowerBound, 0, _width);
            _returnByFbeShort = HistogramTools.BinGenerator(_lowerBound, _upperBound, _width);
            _drawdownByFbeShort = HistogramTools.BinGenerator(_lowerBound, 0, _width);
        }
        private void GetStatistics(ITest[] results)
        {
            for (int i = 0; i < 2; i++)
            for (int j = 0; j < results[i].FBEResults.Length; j++)
                if (results[i].FBEResults[j] != 0)
                    HistogramOperations(results[i], j, i);
        }

        private void HistogramOperations(ITest results, int j, int i)
        {
            if(i == 0)
            {
                HistogramTools.CategoriseItem(_returnByDrawdown, results.FBEDrawdown[j], results.FBEResults[j]);
                HistogramTools.CategoriseItem(_returnByFbe, results.FBEResults[j]);
                if (results.FBEResults[j] > 0) HistogramTools.CategoriseItem(_drawdownByFbe, results.FBEDrawdown[j]);
            }
            else
            {
                HistogramTools.CategoriseItem(_returnByDrawdownShort, results.FBEDrawdown[j], results.FBEResults[j]);
                HistogramTools.CategoriseItem(_returnByFbeShort, results.FBEResults[j]);
                if (results.FBEResults[j] > 0) HistogramTools.CategoriseItem(_drawdownByFbeShort, results.FBEDrawdown[j]);
            }

        }

        private void AddGeneralStats(ITest[] results)
        {
            ExpectancyLongAvg.Add(results[0].ExpectancyAverage);
            ExpectancyLongMed.Add(results[0].ExpectancyMedian);
            WinpercentageLong.Add(results[0].WinPercentage);
            RollingExpectancyLong.Add(EntryTestDrilldown.GetExpectancyByEpoch(results[0].FBEResults.ToList(), 30));

            ExpectancyShortAvg.Add(results[1].ExpectancyAverage);
            ExpectancyShortMed.Add(results[1].ExpectancyMedian);
            WinpercentageShort.Add(results[1].WinPercentage);
            RollingExpectancyShort.Add(EntryTestDrilldown.GetExpectancyByEpoch(results[1].FBEResults.ToList(), 30));
        }

        private void AddHistogramStats()
        {
            ReturnByFbeLong.Add(HistogramTools.GenerateHistogram(_returnByFbe));
            DrawdownByFbeLong.Add(HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownByFbe)));
            ReturnByFbeShort.Add(HistogramTools.GenerateHistogram(_returnByFbeShort));
            DrawdownByFbeShort.Add(HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownByFbeShort)));
        }

        private void AddCategorisedAndBoundedStats()
        {
            ReturnByDrawdownLong = HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdown, HistogramTools.BinGenerator(_lowerBound, 0, _width));
            ReturnByDrawdownShort = HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdownShort, HistogramTools.BinGenerator(_lowerBound, 0, _width));
        }
    }
}
