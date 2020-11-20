using System.Collections.Generic;
using DataStructures.StatsTools;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class BoundedSeriesTests
    {

        [Fact]
        private void GeneratesBoundedStats() {
            var myLIst = new List<double>();
            for (int i = 0; i <= 100; i++) myLIst.Add(i);
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(0, myStat.Minimum);
            Assert.Equal(100, myStat.Maximum);
            Assert.Equal(50, myStat.Average);
            Assert.Equal(50, myStat.Median);
            Assert.Equal(90, myStat.Upper);
            Assert.Equal(10, myStat.Lower);
        }
        [Fact]
        private void GeneratesBoundedStatsWithNegs() {
            var myLIst = new List<double>();
            for (int i = -50; i <= 50; i++) myLIst.Add(i);
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(-50, myStat.Minimum);
            Assert.Equal(50, myStat.Maximum);
            Assert.Equal(0, myStat.Average);
            Assert.Equal(0, myStat.Median);
            Assert.Equal(40, myStat.Upper);
            Assert.Equal(-40, myStat.Lower);
        }
        [Fact]
        private void GeneratesBoundedStatsAllNegs() {
            var myLIst = new List<double>();
            for (int i = -100; i <= 0; i++) myLIst.Add(i);
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(-100, myStat.Minimum);
            Assert.Equal(0, myStat.Maximum);
            Assert.Equal(-50, myStat.Average);
            Assert.Equal(-50, myStat.Median);
            Assert.Equal(-10, myStat.Upper);
            Assert.Equal(-90, myStat.Lower);
        }

        [Fact]
        private void GeneratesBoundedStatsSmallRandomList() {
            var myLIst = new List<double>(){-9.8,-5,-1,0.1,2.5,10,12,30,40,120};
            
            var myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(-9.8, myStat.Minimum);
            Assert.Equal(120, myStat.Maximum);
            Assert.Equal(19.880000000000003, myStat.Average);
            Assert.Equal(6.25, myStat.Median);
            Assert.Equal(40, myStat.Upper);
            Assert.Equal(-5, myStat.Lower);
        }

        [Fact]
        private void ShouldGenerateCorrectBoundsUnEvenCount() {
            var myLIst = new List<double>() { -20,-18,-16,-14,-12,-10,-8,-6,-4,-2,0,2,4,6,8,10,12,14,16,18,20 };

           var myStat = new BoundedStat(myLIst, 0.1);
            Assert.Equal(2, myStat.Upper);
            Assert.Equal(-2, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.2);
            Assert.Equal(4, myStat.Upper);
            Assert.Equal(-4, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.3);
            Assert.Equal(6, myStat.Upper);
            Assert.Equal(-6, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.4);
            Assert.Equal(8, myStat.Upper);
            Assert.Equal(-8, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.5);
            Assert.Equal(10, myStat.Upper);
            Assert.Equal(-10, myStat.Lower);
             myStat = new BoundedStat(myLIst, 0.6);
            Assert.Equal(12, myStat.Upper);
            Assert.Equal(-12, myStat.Lower);
             myStat = new BoundedStat(myLIst, 0.7);
            Assert.Equal(14, myStat.Upper);
            Assert.Equal(-14, myStat.Lower);
             myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(16, myStat.Upper);
            Assert.Equal(-16, myStat.Lower);
             myStat = new BoundedStat(myLIst, 0.9);
            Assert.Equal(18, myStat.Upper);
            Assert.Equal(-18, myStat.Lower);
             myStat = new BoundedStat(myLIst, 1);
            Assert.Equal(20, myStat.Upper);
            Assert.Equal(-20, myStat.Lower);
        }

        [Fact]
        private void ShouldGenerateCorrectBoundsEvenCount() {
            var myLIst = new List<double>() { -20, -18, -16, -14, -12, -10, -8, -6, -4, -2, 0, 2, 4, 6, 8, 10, 12, 14, 16, 18 };

            var myStat = new BoundedStat(myLIst, 0.3);
            Assert.Equal(4, myStat.Upper);
            Assert.Equal(-6, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.4);
            Assert.Equal(6, myStat.Upper);
            Assert.Equal(-8, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.5);
            Assert.Equal(8, myStat.Upper);
            Assert.Equal(-10, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.6);
            Assert.Equal(10, myStat.Upper);
            Assert.Equal(-12, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.7);
            Assert.Equal(12, myStat.Upper);
            Assert.Equal(-14, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.8);
            Assert.Equal(14, myStat.Upper);
            Assert.Equal(-16, myStat.Lower);
            myStat = new BoundedStat(myLIst, 0.9);
            Assert.Equal(16, myStat.Upper);
            Assert.Equal(-18, myStat.Lower);
            myStat = new BoundedStat(myLIst, 1);
            Assert.Equal(18, myStat.Upper);
            Assert.Equal(-20, myStat.Lower);
        }
    }
}


//var tempavg = tenEpoch.Select(x => x.AverageExpectancy).ToList();
//var tempmed = tenEpoch.Select(x => x.MedianExpectancy).ToList();
//System.Text.StringBuilder t = new System.Text.StringBuilder();
//for (int i = 0; i<tempavg.Count; i++) {
//t.AppendLine($"{tempavg[i]},{tempmed[i]}");
//}
//File.WriteAllText(@"C:\Temp\res.txt", t.ToString());


