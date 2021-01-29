using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class ExtremeFinderTests
    {
        [Fact]
        private void ShouldDetectConsecutiveHighs (){
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var finder = new ExtremesFinder(false, 1, 0);
            Assert.True(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barTwo.High.Mid));

            finder = new ExtremesFinder(false, 1, 0);
            Assert.True(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));

            finder = new ExtremesFinder(false, 1, 0);
            Assert.True(finder.CheckExtreme(barThree.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barTwo.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighs() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);
            var barFive = new BidAskData(9);

            var finder = new ExtremesFinder(false, 2, 0);
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barTwo.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));

            finder = new ExtremesFinder(false, 2, 0);
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));
            Assert.True(finder.CheckExtreme(barFive.High.Mid));

            finder = new ExtremesFinder(false, 2, 0);
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.False(finder.CheckExtreme(barThree.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveHighs() {
            var finderFifty = new ExtremesFinder(false, 50, 0);
            var finderSeventyFive = new ExtremesFinder(false, 75, 0);
            var finderNinetyNine = new ExtremesFinder(false, 99, 0);

            for (int i = 0; i < 100; i++) {
                if (i >= 49)
                    Assert.True(finderFifty.CheckExtreme(new BidAskData(i).High.Mid));
                else
                    Assert.False(finderFifty.CheckExtreme(new BidAskData(i).High.Mid));
                if (i >= 74)
                    Assert.True(finderSeventyFive.CheckExtreme(new BidAskData(i).High.Mid));
                else
                    Assert.False(finderSeventyFive.CheckExtreme(new BidAskData(i).High.Mid));
                if (i >= 98)
                    Assert.True(finderNinetyNine.CheckExtreme(new BidAskData(i).High.Mid));
                else
                    Assert.False(finderNinetyNine.CheckExtreme(new BidAskData(i).High.Mid));
            }
        }

        [Fact]
        private void ShouldDetectConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var finder = new ExtremesFinder(false, 1, 1);
            Assert.True(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barTwo.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));

            finder = new ExtremesFinder(false, 1, 1);
            Assert.True(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));

            finder = new ExtremesFinder(false, 2, 1);
            Assert.False(finder.CheckExtreme(barTwo.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.True(finder.CheckExtreme(barThree.High.Mid));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(1);
            var barTwo = new BidAskData(2);
            var barThree = new BidAskData(3);
            var barFour = new BidAskData(4);
            var barFive = new BidAskData(5);
            var barSix = new BidAskData(6);
            var barSeven = new BidAskData(7);
            var barEight = new BidAskData(8);
            var barnine = new BidAskData(9);

            var finder = new ExtremesFinder(false, 4,2);
            Assert.False(finder.CheckExtreme(barTwo.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.False(finder.CheckExtreme(barThree.High.Mid));
            Assert.False(finder.CheckExtreme(barFour.High.Mid));
            Assert.False(finder.CheckExtreme(barOne.High.Mid));
            Assert.False(finder.CheckExtreme(barFive.High.Mid));
            Assert.True(finder.CheckExtreme(barSix.High.Mid));
            Assert.True(finder.CheckExtreme(barSeven.High.Mid));
            Assert.False(finder.CheckExtreme(barSeven.High.Mid));
            Assert.True(finder.CheckExtreme(barEight.High.Mid));
            Assert.False(finder.CheckExtreme(barSeven.High.Mid));
            Assert.False(finder.CheckExtreme(barnine.High.Mid));
        }

        [Fact]
        private void ShouldDetectConsecutiveLows() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var finder = new ExtremesFinder(true, 1, 0);
            Assert.True(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barTwo.Low.Mid));

            finder = new ExtremesFinder(true, 1, 0);
            Assert.True(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barThree.Low.Mid));

            finder = new ExtremesFinder(true, 1, 0);
            Assert.True(finder.CheckExtreme(barThree.Low.Mid));
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barTwo.Low.Mid));
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLows() {
            var barOne = new BidAskData(9);
            var barTwo = new BidAskData(7);
            var barThree = new BidAskData(6);
            var barFive = new BidAskData(5);

            var finder = new ExtremesFinder(true, 2, 0);
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barTwo.Low.Mid));
            Assert.True(finder.CheckExtreme(barThree.Low.Mid));

            finder = new ExtremesFinder(true, 2, 0);
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barThree.Low.Mid));
            Assert.True(finder.CheckExtreme(barFive.Low.Mid));
            Assert.False(finder.CheckExtreme(barThree.Low.Mid));
            Assert.False(finder.CheckExtreme(barFive.Low.Mid));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveLows() {
            var finderFifty = new ExtremesFinder(true, 50, 0);
            var finderSeventyFive = new ExtremesFinder(true, 75, 0);
            var finderNinetyNine = new ExtremesFinder(true, 99, 0);

            for (int i = 100; i > 0; i--) {
                if(100-i>=49)
                    Assert.True(finderFifty.CheckExtreme(new BidAskData(i).Low.Mid));
                else
                    Assert.False(finderFifty.CheckExtreme(new BidAskData(i).Low.Mid));
                if (100-i >= 74)
                    Assert.True(finderSeventyFive.CheckExtreme(new BidAskData(i).Low.Mid));
                else
                    Assert.False(finderSeventyFive.CheckExtreme(new BidAskData(i).Low.Mid));
                if (100-i >= 98)
                    Assert.True(finderNinetyNine.CheckExtreme(new BidAskData(i).Low.Mid));
                else
                    Assert.False(finderNinetyNine.CheckExtreme(new BidAskData(i).Low.Mid));
            }
        }

        [Fact]
        private void ShouldDetectConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);
            var barFour = new BidAskData(1);

            var finder = new ExtremesFinder(true, 1, 1);
            Assert.True(finder.CheckExtreme(barTwo.Low.Mid));
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
            Assert.False(finder.CheckExtreme(barTwo.Low.Mid));
            Assert.True(finder.CheckExtreme(barThree.Low.Mid));
            Assert.False(finder.CheckExtreme(barOne.Low.Mid));
            Assert.True(finder.CheckExtreme(barFour.Low.Mid));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(12);
            var barTwo = new BidAskData(11);
            var barThree = new BidAskData(10);
            var barFour = new BidAskData(9);
            var barFive = new BidAskData(7);
            var barSeven = new BidAskData(5);

            var finder = new ExtremesFinder(true,3,1);

           Assert.False(finder.CheckExtreme(barThree.Low.Mid));
           Assert.False(finder.CheckExtreme(barTwo.Low.Mid));
           Assert.False(finder.CheckExtreme(barOne.Low.Mid));
           Assert.False(finder.CheckExtreme(barTwo.Low.Mid));
           Assert.False(finder.CheckExtreme(barThree.Low.Mid));
           Assert.True(finder.CheckExtreme(barFive.Low.Mid));
           Assert.False(finder.CheckExtreme(barFour.Low.Mid));
           Assert.True(finder.CheckExtreme(barSeven.Low.Mid));
           Assert.False(finder.CheckExtreme(barOne.Low.Mid));
        }
    }

 
}
