using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStructures.StatsTools;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class EpochGeneratorTests
    {
        //private List<ITest[]> myTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";


        public EpochGeneratorTests()
        {
            //var market = Market.MarketBuilder.CreateMarket(marketData);
            //var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            //{
            //    new DummyEntries(1, 98)
            //}, market);

            //var longSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1, MarketSide.Bull));
            //var shortSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 14, 1, MarketSide.Bear));


            //myTests = new List<ITest[]>();
            //for (int i = 0; i < longSide.Count; i++)
            //    myTests.Add(new[] { longSide[i], shortSide[i] });

        }


        [Fact]
        private void ShouldGenerateEpoch() {
            List<double> myList = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, -89, -0.5, 56, 7, 0.1, -0.1};
            var epochOne = EpochGenerator.SplitListIntoEpochs(myList, 5);
            Assert.Equal(5, epochOne.EpochContainer.Count);
            Assert.Equal(new List<double>() {1}, epochOne.EpochContainer[0]);
            Assert.Equal(new List<double>() {23, 2.3, 5}, epochOne.EpochContainer[1]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochOne.EpochContainer[2]);
            Assert.Equal(new List<double>() {-89, -0.5, 56}, epochOne.EpochContainer[3]);
            Assert.Equal(new List<double>() {7, 0.1, -0.1}, epochOne.EpochContainer[4]);
        }

        [Fact]
        private void ShouldGenerateEvenEpoch() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};
            var epochTwo = EpochGenerator.SplitListIntoEpochs(myListTwo, 4);
            Assert.Equal(4, epochTwo.EpochContainer.Count);
            Assert.Equal(new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4}, epochTwo.EpochContainer[0]);
            Assert.Equal(new List<double>() {12, 3, 0.4, -89, -0.5, 56, 12}, epochTwo.EpochContainer[1]);
            Assert.Equal(new List<double>() {3, 0.4, 7, 0.1, -0.1, 12, 3}, epochTwo.EpochContainer[2]);
            Assert.Equal(new List<double>() {0.4, 12, 3, 0.4, 12, 3, 0.4}, epochTwo.EpochContainer[3]);
        }

        [Fact]
        private void ShouldGenerateUnequalEpochs() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};

            var epochThree = EpochGenerator.SplitListIntoEpochs(myListTwo, 9);
            Assert.Equal(9, epochThree.EpochContainer.Count);
            Assert.Equal(new List<double>() {1, 23, 2.3, 5}, epochThree.EpochContainer[0]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[1]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[2]);
            Assert.Equal(new List<double>() {-89, -0.5, 56}, epochThree.EpochContainer[3]);
            Assert.Equal(new List<double>() {12, 3, 0.4}, epochThree.EpochContainer[4]);
            Assert.Equal(new List<double>() {7, 0.1, -0.1}, epochThree.EpochContainer[5]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[6]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[7]);
            Assert.Equal(new List<double>() {12, 3, 0.4,}, epochThree.EpochContainer[8]);
        }

        [Fact]
        private void ShouldGenerateMassiveEpoch() {
            List<double> myListThree = new List<double>();
            for (int i = 0; i < 12345; i++) myListThree.Add(0);
            var epochFour = EpochGenerator.SplitListIntoEpochs(myListThree, 29);
            Assert.Equal(myListThree.Count % (29 - 1), epochFour.EpochContainer[0].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[1].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[21].Count);
            Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[3].Count);
            Assert.Equal(12345, epochFour.EpochContainer.Sum(x => x.Count));
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


