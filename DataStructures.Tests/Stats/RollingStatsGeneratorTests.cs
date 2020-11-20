using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStructures.StatsTools;
using TestUtils;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class RollingStatsGeneratorTests
    {
        private List<double> GenerateList(int length) {
            var retval = new List<double>();
            for (int i = 0; i < length/2; i++)
            for (int x = 0; x < 10; x++)
                retval.Add(-0.6+x*0.1);

            for (int i = 0; i < length / 2; i++)
            for (int x = 0; x < 10; x++)
                retval.Add(-0.3 + x * 0.1);

            return retval;
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodAvgExp() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>(){}, ten.Select(x => x.AverageExpectancy));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodMedExp() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.MedianExpectancy));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodAvgGain() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.AvgGain));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodAvgLoss() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.AvgLoss));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodMedGain() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.MedianGain));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodMedLoss() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.MedianLoss));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodWin() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.WinPercent));
        }

        [Fact]
        private void ShouldGenerateRollingStatsOverSmallPeriodSortino() {
            var ten = RollingStatsGenerator.GetRollingStats(GenerateList(10), 10);
            Assert.Equal(new List<double>() { }, ten.Select(x => x.Sortino));
        }





        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodAvgExp() {
             var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
             Assert.Equal(new List<double>() { }, thirteen.Select(x => x.AverageExpectancy));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodMedExp() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.MedianExpectancy));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodAvgGain() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.AvgGain));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodAvgLoss() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.AvgLoss));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodMedGain() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.MedianGain));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodMedLoss() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.MedianLoss));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodWin() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.WinPercent));
        }
        [Fact]
        private void ShouldGenerateRollingStatsOverLongPeriodSortino() {
            var thirteen = RollingStatsGenerator.GetRollingStats(GenerateList(55), 30);
            Assert.Equal(new List<double>() { }, thirteen.Select(x => x.Sortino));
        }






        [Fact]
        private void ShouldGenerateStatsOverFewEpochAvgExp() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { },ThreeEpoch.Select(x => x.AverageExpectancy));
        }

        [Fact]
        private void ShouldGenerateStatsOverFewEpochMedExp() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.MedianExpectancy));
        }
        [Fact]
        private void ShouldGenerateStatsOverFewEpochAvgGain() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.AvgGain));
        }
        [Fact]
        private void ShouldGenerateStatsOverFewEpochMedGain() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.MedianGain));
        }

        [Fact]
        private void ShouldGenerateStatsOverFewEpochAvgLoss() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.AvgLoss));
        }
        [Fact]
        private void ShouldGenerateStatsOverFewEpochMedLoss() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.MedianLoss));
        }

        [Fact]
        private void ShouldGenerateStatsOverFewEpochWin() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.WinPercent));
        }
        [Fact]
        private void ShouldGenerateStatsOverFewEpochSortino() {
            var ThreeEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(15), 3);
            Assert.Equal(new List<double>() { }, ThreeEpoch.Select(x => x.Sortino));
        }





        [Fact]
        private void ShouldGenerateStatsOverManyEpochAvgExp() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x=>x.AverageExpectancy));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochMedExp() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.MedianExpectancy));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochAvgGain() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.AvgGain));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochMedGain() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.MedianGain));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochAvgLoss() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.AvgLoss));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochMedLoss() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.MedianLoss));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochWin() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.WinPercent));
        }
        [Fact]
        private void ShouldGenerateStatsOverManyEpochSortino() {
            var tenEpoch = RollingStatsGenerator.GetStatsByEpoch(GenerateList(20), 12);
            Assert.Equal(new List<double>() { }, tenEpoch.Select(x => x.Sortino));
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


