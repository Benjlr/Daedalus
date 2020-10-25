using Logic.Metrics;
using Logic.Utils;
using System.Collections.Generic;

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

        private Dictionary<double, int> _returnByFbe;
        private Dictionary<double, int> _drawdownByFbe;
        private Dictionary<double, int> _returnByFbeShort;
        private Dictionary<double, int> _drawdownByFbeShort;
        private Dictionary<double, List<double>> _returnByDrawdown;
        private Dictionary<double, List<double>> _returnByDrawdownShort;

        private double _lowerBound = -0.05;
        private double _upperBound = 0.05;
        private double _width = 1.0 / 500;

        public List<string> X_label_categorised;
        public List<string> Y_label_categorised;
        public List<string> X_label;
        public List<string> Y_label;

        public void GenerateFixedBarResults(ITest[] results)
        {
            InitListsAndLabels();
            for (int i = 0; i < results.Length; i++)
            {

                InitHistogramsForCurrentTest();
                GetLongStatistics(results[i]);
                GetShortStatistics(results[i]);
                AddGeneralStats(results, i);
                AddHistogramStats();
            }

            AddCategorisedStats();
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
        
        private void GetLongStatistics(ITest result)
        {
            for (int j = 0; j < result.FBELong.Length; j++)
                if (result.FBELong[j] != 0)
                    LongHistogramOperations(result, j);
        }

        private void LongHistogramOperations(ITest result, int j)
        {
            HistogramTools.CategoriseItem(_returnByDrawdown, result.FBEDrawdownLong[j], result.FBELong[j]);
            HistogramTools.CategoriseItem(_returnByFbe, result.FBELong[j]);
            HistogramTools.CategoriseItem(_drawdownByFbe, result.FBEDrawdownLong[j]);
        }

        private void GetShortStatistics(ITest results)
        {
            for (int j = 0; j < results.FBEShort.Length; j++)
                if (results.FBEShort[j] != 0)
                    ShortHistogramOperations(results, j);
        }

        private void ShortHistogramOperations(ITest results, int j)
        {
            HistogramTools.CategoriseItem(_returnByDrawdownShort, results.FBEDrawdownShort[j], results.FBEShort[j]);
            HistogramTools.CategoriseItem(_returnByFbeShort, results.FBEShort[j]);
            HistogramTools.CategoriseItem(_drawdownByFbeShort, results.FBEDrawdownShort[j]);
        }

        private void AddGeneralStats(ITest[] results, int i)
        {
            ExpectancyLongAvg.Add(results[i].ExpectancyLongAverage);
            ExpectancyShortAvg.Add(results[i].ExpectancyShortAverage);
            ExpectancyLongMed.Add(results[i].ExpectancyLongMedian);
            ExpectancyShortMed.Add(results[i].ExpectancyShortMedian);
            WinpercentageLong.Add(results[i].WinPercentageLong);
            WinpercentageShort.Add(results[i].WinPercentageShort);
        }

        private void AddHistogramStats()
        {
            ReturnByFbeLong.Add(HistogramTools.GenerateHistogram(_returnByFbe));
            DrawdownByFbeLong.Add(HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownByFbe)));
            ReturnByFbeShort.Add(HistogramTools.GenerateHistogram(_returnByFbeShort));
            DrawdownByFbeShort.Add(HistogramTools.MakeCumulative(HistogramTools.GenerateHistogram(_drawdownByFbeShort)));
        }

        private void AddCategorisedStats()
        {
            ReturnByDrawdownLong = HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdown, HistogramTools.BinGenerator(_lowerBound, 0, _width));
            ReturnByDrawdownShort = HistogramTools.GenerateHistorgramsFromCategories(_returnByDrawdownShort, HistogramTools.BinGenerator(_lowerBound, 0, _width));
        }
    }
}
