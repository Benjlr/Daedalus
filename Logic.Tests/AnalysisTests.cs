using Logic.Utils;
using System;
using System.Collections.Generic;
using Xunit;

namespace Logic.Tests
{
    public class AnalysisTests
    {
        private List<double> numbers = new List<double>() { -15.4, -45, -25.2, -22.5, -19.1, 35.78, 5.6, 0, 2.78, 3.6, 12,  -6.4, -8.8, 1, 5.41, 16.20, 17.1, -13, -7, 3, 22, 6.2, 0.2, -2, -1.3, -4.6, -0.2 };
        private List<double> result = new List<double>() {0.111111111  ,  0.074074074 ,0.037037037, 0.111111111, 0.148148148 ,0.222222222 ,0.111111111 ,0.037037037, 0.074074074 ,0.074074074};
        private List<double> resultCumulative = new List<double>() { 0.111111111, 0.185185185, 0.222222222, 0.333333333, 0.481481481, 0.703703704, 0.814814815, 0.851851852, 0.925925926 ,1};
        private List<Tuple<double,double>> ReturnsAndDrawdown = new List<Tuple<double, double>>()
        {
            new Tuple<double,double>(12.2, -2.8),
            new Tuple<double,double>(-5.2, -77),
            new Tuple<double,double>(0.1, 0),
            new Tuple<double,double>(3.29, -16.9),
            new Tuple<double,double>(12.8, -23.1),
            new Tuple<double,double>(42.366, -22.9),
            new Tuple<double,double>(-47.2, -2.3),
            new Tuple<double,double>(-6, -5),
            new Tuple<double,double>(15.2, -12.9)
        };

        private List<List<double>> RanDResults = new List<List<double>>()
        {
            new List<double>() {0, 0, 0, 0, 1, 0},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0.5, 0, 0, 0, 0.5, 0},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0, 0.5, 0, 0, 0, 0.5},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0.5, 0, 0, 0, 0.5, 0},
            new List<double>() {0, 0, 1, 0, 0, 0},
            new List<double>() {1, 0, 0, 0, 0, 0},
        };



        private double _round(double x) => Math.Round(x, _precision);
        private int _precision = 6;


        [Fact]
        public void ShouldInitHistogram()
        {
            
            Assert.True(false);
        }

        [Fact]
        public void ShouldInitCategories()
        {

            Assert.True(false);
        }

        [Fact]
        public void ShouldCategoriseItemInHistogram()
        {

            Assert.True(false);
        }

        [Fact]
        public void ShouldCategoriseItemInCategory()
        {

            Assert.True(false);
        }

        [Fact]
        public void ShouldGenerateHistogram()
        {
            var bins = HistogramTools.BinGenerator(-20, 20, 5);
            foreach (var t in numbers) HistogramTools.CategoriseItem(bins,t);
            var hist = HistogramTools.GenerateHistogram(bins);
            for (int i = 0; i < hist.Count; i++) hist[i] = _round(hist[i]);
            for (int i = 0; i < result.Count; i++) result[i] = _round(result[i]);
            Assert.Equal(hist,result);
        }

        [Fact]
        public void ShouldGenerateCumulativeHistogram()
        {
            var bins = HistogramTools.BinGenerator(-20, 20, 5);
            foreach (var t in numbers) HistogramTools.CategoriseItem(bins, t);
            var hist = HistogramTools.GenerateHistogram(bins);
            hist = HistogramTools.MakeCumulative(hist);
            for (int i = 0; i < hist.Count; i++) hist[i] = _round(hist[i]);
            for (int i = 0; i < resultCumulative.Count; i++) resultCumulative[i] = _round(resultCumulative[i]);
            Assert.Equal(hist, resultCumulative);
        }


        [Fact]
        public void ShouldGenerateCategorisedSeries()
        {
            var categoriseDrawdown = HistogramTools.CategoryGenerator(-20, 20, 5);
            foreach (var t in ReturnsAndDrawdown) HistogramTools.CategoriseItem(categoriseDrawdown, t.Item2, t.Item1);
            var results = HistogramTools.GenerateHistorgramsFromCategories(categoriseDrawdown, HistogramTools.BinGenerator(-20, 0, 5));

            for (int i = 0; i < RanDResults.Count; i++)
            for (int j = 0; j < RanDResults[i].Count; j++)
                RanDResults[i][j] = _round(RanDResults[i][j]);

            for (int i = 0; i < results.Count; i++)
            for (int j = 0; j < results[i].Count; j++)
                results[i][j] = _round(results[i][j]);

            for (int i = 0; i < results.Count; i++) Assert.Equal(results[i], RanDResults[i]);

        }
    }
}
