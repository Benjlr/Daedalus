using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataStructures.StatsTools;
using TestUtils;
using Xunit;

namespace DataStructures.Tests.Stats
{
    public class EpochGeneratorTests
    {
        [Fact]
        private void ShouldGenerateEpoch() {
            List<double> myList = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, -89, -0.5, 56, 7, 0.1, -0.1};
            var epochOne = EpochGenerator.SplitListIntoEpochs(myList, 5);
            var expected = new List<List<double>>()
            {
                new List<double>() { 1 },
                new List<double>() { 23, 2.3, 5 },
                new List<double>() { 12, 3, 0.4 },
                new List<double>() { -89, -0.5, 56 },
                new List<double>() { 7, 0.1, -0.1 },
            };
            Asserters.ListListDoubleEquals(expected, epochOne.EpochContainer);
        }

        [Fact]
        private void ShouldGenerateEvenEpoch() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};
            var epochTwo = EpochGenerator.SplitListIntoEpochs(myListTwo, 4);
            var expected = new List<List<double>>()
            {
                new List<double>() { 1, 23, 2.3, 5, 12, 3, 0.4 },
                new List<double>() { 12, 3, 0.4, -89, -0.5, 56, 12 },
                new List<double>() { 3, 0.4, 7, 0.1, -0.1, 12, 3 },
                new List<double>() { 0.4, 12, 3, 0.4, 12, 3, 0.4 },
            };
            Asserters.ListListDoubleEquals(expected, epochTwo.EpochContainer);
        }

        [Fact]
        private void ShouldGenerateUnequalEpochs() {
            List<double> myListTwo = new List<double>() {1, 23, 2.3, 5, 12, 3, 0.4, 12, 3, 0.4, -89, -0.5, 56, 12, 3, 0.4, 7, 0.1, -0.1, 12, 3, 0.4, 12, 3, 0.4, 12, 3, 0.4};
            var epochThree = EpochGenerator.SplitListIntoEpochs(myListTwo, 9);
            var expected = new List<List<double>>()
            {
                new List<double>() { 1, 23, 2.3, 5 },
                new List<double>() { 12, 3, 0.4 }   ,
                new List<double>() { 12, 3, 0.4 }   ,
                new List<double>() { -89, -0.5, 56 },
                new List<double>() { 12, 3, 0.4 }   ,
                new List<double>() { 7, 0.1, -0.1 } ,
                new List<double>() { 12, 3, 0.4, }  ,
                new List<double>() { 12, 3, 0.4, }  ,
                new List<double>() { 12, 3, 0.4, }  ,
            };

            Asserters.ListListDoubleEquals(expected, epochThree.EpochContainer);
        }

        [Fact]
        private void ShouldGenerateMassiveEpoch() {
            List<double> myListThree = new List<double>();
            for (int i = 0; i < 12345; i++) myListThree.Add(0);
            var epochFour = EpochGenerator.SplitListIntoEpochs(myListThree, 29);
            Assert.Equal(myListThree.Count % (29 - 1), epochFour.EpochContainer[0].Count);
            Assert.Equal(12345, epochFour.EpochContainer.Sum(x => x.Count));

            for (int i = 1; i < epochFour.EpochContainer.Count; i++) 
                Assert.Equal(myListThree.Count / (29 - 1), epochFour.EpochContainer[i].Count);
        }
    }
}
