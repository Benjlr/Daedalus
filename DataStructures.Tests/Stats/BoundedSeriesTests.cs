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

    }
}


//var tempavg = tenEpoch.Select(x => x.AverageExpectancy).ToList();
//var tempmed = tenEpoch.Select(x => x.MedianExpectancy).ToList();
//System.Text.StringBuilder t = new System.Text.StringBuilder();
//for (int i = 0; i<tempavg.Count; i++) {
//t.AppendLine($"{tempavg[i]},{tempmed[i]}");
//}
//File.WriteAllText(@"C:\Temp\res.txt", t.ToString());


