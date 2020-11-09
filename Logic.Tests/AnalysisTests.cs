using Logic.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
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
            new List<double>() {0, 0, 0, 0, 1, 1},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0.5, 0.5, 0.5, 0.5, 1, 1},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0, 0.5, 0.5, 0.5, 0.5, 1},
            new List<double>() {double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN},
            new List<double>() {0.5, 0.5, 0.5, 0.5, 1.0, 1},
            new List<double>() {0, 0, 1, 1, 1, 1},
            new List<double>() {1, 1, 1, 1, 1, 1},
        };

        private List<Dictionary<double, List<double>>> CollationInput = new List<Dictionary<double, List<double>>>()
        {
            new Dictionary<double, List<double>>(){ {0.0, new List<double>() { } },{1.5, new List<double>() {96,2000 } },{3.0, new List<double>() {1 } } },
            new Dictionary<double, List<double>>(){ {0.0, new List<double>() {5,6 } },{1.5, new List<double>() { -78,0.0} },{ 3.0, new List<double>() { -1} } },
            new Dictionary<double, List<double>>(){ {0.0, new List<double>() {-1,23 } },{1.5, new List<double>() {-1 } },{ 3.0, new List<double>() { } } },
            new Dictionary<double, List<double>>(){ {0.0, new List<double>() { 0,0} },{1.5, new List<double>() { 10,-23} },{ 3.0, new List<double>() {0.8 } } },
            new Dictionary<double, List<double>>(){ {0.0, new List<double>() { } },{1.5, new List<double>() {4 } },{ 3.0, new List<double>() { } } },
        };



        private Dictionary<double, List<double>> CollationResults = new Dictionary<double, List<double>>()
        {
            {0.0, new List<double>() { 5,6,-1,23,0,0} },{1.5, new List<double>() {96,2000,-78,0.0,-1,10,-23,4 } },{3.0, new List<double>() {1,-1,0.8 } } ,{double.PositiveInfinity, new List<double>() {} }
        };



        private double _round(double x) => Math.Round(x, _precision);
        private int _precision = 6;

        [Fact]
        public void ShouldInitHistogram()
        {
            var myhistogram = HistogramTools.BinGenerator(new BinDescriptor(-100, 100, 25));
            var expected = new Dictionary<double, int>()
            {
                {-100,0},
                {-75,0 },
                {-50,0 },
                {-25,0 },
                {0,0 },
                {25,0 },
                {50,0 },
                {75,0 },
                {100,0 },
                {Double.PositiveInfinity, 0 },
            };
            
            Assert.Equal(expected, myhistogram);
        }

        [Fact]
        public void ShouldInitCategories()
        {
            var myhistogram = HistogramTools.CategoryGenerator(new BinDescriptor(-66, 33, 11));
            var expected = new Dictionary<double, List<double>>()
            {
                {-66,new List<double>()},
                {-55,new List<double>() },
                {-44,new List<double>() },
                {-33,new List<double>() },
                {-22,new List<double>()},
                {-11,new List<double>()},
                {0,new List<double>()},
                {11,new List<double>()},
                {22,new List<double>()},
                {33,new List<double>() },
                {Double.PositiveInfinity, new List<double>() },
            };

            Assert.Equal(expected, myhistogram);
        }

        [Fact]
        public void ShouldCategoriseItemInHistogram()
        {
            var myhistogram = HistogramTools.BinGenerator(new BinDescriptor(-100, 100, 25));
            HistogramTools.CategoriseItem(myhistogram, 72);
            HistogramTools.CategoriseItem(myhistogram, 0);
            HistogramTools.CategoriseItem(myhistogram, -0.001);
            HistogramTools.CategoriseItem(myhistogram, 200);
            HistogramTools.CategoriseItem(myhistogram, 52);
            Assert.Equal(myhistogram[75], 2);
            Assert.Equal(myhistogram[Double.PositiveInfinity], 1);
            Assert.Equal(myhistogram[0], 1);
            Assert.Equal(myhistogram[25], 1);
            Assert.Equal(myhistogram[-50], 0);
        }

        [Fact]
        public void ShouldCategoriseItemInCategory()
        {
            var myhistogram = HistogramTools.CategoryGenerator(new BinDescriptor(-66, 33, 11));
            HistogramTools.CategoriseItem(myhistogram,7,-45);
            HistogramTools.CategoriseItem(myhistogram,12365.022,-54);
            HistogramTools.CategoriseItem(myhistogram,2.3,-45);
            HistogramTools.CategoriseItem(myhistogram,-0.2,235);
            HistogramTools.CategoriseItem(myhistogram,-6.5,-0);
            
            Assert.Equal(myhistogram[-44], new List<double>(){7,12365.022,2.3});
            Assert.Equal(myhistogram[11], new List<double>(){-6.5});
            Assert.Equal(myhistogram[-66], new List<double>());
            Assert.Equal(myhistogram[double.PositiveInfinity], new List<double>(){-0.2});
        }

        [Fact]
        public void ShouldGenerateHistogram()
        {
            var bins = HistogramTools.BinGenerator(new BinDescriptor(-20, 20, 5));
            foreach (var t in numbers) HistogramTools.CategoriseItem(bins,t);
            var hist = HistogramTools.GenerateHistogram(bins);
            for (int i = 0; i < hist.Count; i++) hist[i] = _round(hist[i]);
            for (int i = 0; i < result.Count; i++) result[i] = _round(result[i]);
            Assert.Equal(hist,result);
        }

        [Fact]
        public void ShouldGenerateCumulativeHistogram()
        {
            var bins = HistogramTools.BinGenerator(new BinDescriptor(-20, 20, 5));
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
            var categoriseDrawdown = HistogramTools.CategoryGenerator(new BinDescriptor(-20, 20, 5));
            foreach (var t in ReturnsAndDrawdown) HistogramTools.CategoriseItem(categoriseDrawdown, t.Item2, t.Item1);
            var results = HistogramTools.GenerateHistorgramsFromCategories(categoriseDrawdown, new BinDescriptor(-20, 0, 5));

            for (int i = 0; i < RanDResults.Count; i++)
            for (int j = 0; j < RanDResults[i].Count; j++)
                RanDResults[i][j] = _round(RanDResults[i][j]);

            for (int i = 0; i < results.Count; i++)
            for (int j = 0; j < results[i].Count; j++)
                results[i][j] = _round(results[i][j]);

            Assert.Equal(results, RanDResults);

        }

        [Fact]
        public void ShouldCollateCategorisedLists()
        {
            var categorisedList = HistogramTools.CollateCategories(CollationInput, new BinDescriptor(0,3,1.5));
            Assert.Equal(CollationResults, categorisedList);
        }
    }
}
