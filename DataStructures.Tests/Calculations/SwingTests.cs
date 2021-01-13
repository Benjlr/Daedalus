using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class SwingTests
    {
        [Fact]
        private void ShouldCalculateUpSwingWithtwoConsecutive() {

        }

        [Fact]
        private void ShouldDetectConsecutiveHighs (){
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() {barOne, barTwo},  1,0));
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barThree },  1, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree, barOne },  1, 0));
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree, barOne,barTwo },  1, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree, barOne,barTwo,barOne }, 1, 0));

        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighs() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);
            var barFive = new BidAskData(9);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barTwo, barThree }, 2, 0));
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barThree,barFive },  2, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barThree, barOne, barThree,barOne },  2, 0));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveHighs() {
            var bidaskList = new List<BidAskData>();
            for (int i = 0; i < 100; i++) {
                bidaskList.Add(new BidAskData(i));
            }

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 50, 0));
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 75, 0));
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 99, 0));
        }

        [Fact]
        private void ShouldDetectConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barTwo, barThree }, 1,1));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne,barThree, barOne}, 1,1));
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barTwo,barOne, barThree }, 2,1));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree, barTwo, barOne }, 1,1));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);
            var barFour = new BidAskData(9);
            var barFive = new BidAskData(10);
            var barSix = new BidAskData(11);
            var barSeven = new BidAskData(12);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() {barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix,barOne,barOne,barSeven }, 4, 2));
            Assert.True(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree, barTwo, barFour,barOne,barFive }, 3, 1));

            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barOne, barThree, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barSix,barFive, barFour, barThree,barTwo,barOne}, 4, 4));
            Assert.False(swings.CheckForConsecutiveHighs(new List<BidAskData>() { barThree,barFour,barFive, barSix, barOne,barOne,barSeven}, 4, 1));
        }

        [Fact]
        private void ShouldDetectConsecutiveLows() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barTwo }, 1, 0));
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barThree }, 1, 0));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barOne }, 1, 0));
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barOne, barTwo }, 1, 0));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barOne, barTwo, barOne }, 1, 0));

        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLows() {
            var barOne = new BidAskData(9);
            var barTwo = new BidAskData(7);
            var barThree = new BidAskData(6);
            var barFive = new BidAskData(5);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barTwo, barThree }, 2, 0));
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barThree, barFive }, 2, 0));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barThree, barOne, barThree, barOne }, 2, 0));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveLows() {
            var bidaskList = new List<BidAskData>();
            for (int i = 100; i > 0; i--) {
                bidaskList.Add(new BidAskData(i));
            }

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 50, 0));
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 75, 0));
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 99, 0));
        }

        [Fact]
        private void ShouldDetectConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barTwo, barThree }, 1, 1));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barThree, barOne }, 1, 1));
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barTwo, barOne, barThree }, 2, 1));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barTwo, barOne }, 1, 1));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(12);
            var barTwo = new BidAskData(11);
            var barThree = new BidAskData(10);
            var barFour = new BidAskData(9);
            var barFive = new BidAskData(7);
            var barSix = new BidAskData(6);
            var barSeven = new BidAskData(5);

            var swings = new Swings();
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix, barOne, barOne, barSeven }, 4, 2));
            Assert.True(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barTwo, barFour, barOne, barFive }, 3, 1));

            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barOne, barThree, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barSix, barFive, barFour, barThree, barTwo, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveLows(new List<BidAskData>() { barThree, barFour, barFive, barSix, barOne, barOne, barSeven }, 4, 1));
        }

    }

    public class Swings
    {
        private double _atrLimitHighs { get; set; }
        private double _atrLimitLows { get; set; }
        private int _consecutiveHighExtremes { get; set; }
        private int _consecutiveLowExtremes { get; set; }
        private int _neighboursAllowedHigh { get; set; }
        private int _neighboursAllowedLow { get; set; }

        public Swings(double atrLimitHigh, int consecutiveHighs, int neighboursAllowedHigh, double atrLimitLow, int consecutiveLows, int neighboursAllowedLows) {
            _atrLimitHighs = atrLimitHigh;
            _atrLimitLows = atrLimitLow;
            _consecutiveExtremes = consecutiveHighsOrLows
        }

        public void Calculate(BidAskData price, ) {
            var baseAtrs = AverageTrueRange.Calculate(prices);
            var atrReversals = new double[prices.Count];
            var swings = new List<Swing>();

            for (var i = 0; i < prices.Count; i++)
                atrReversals[i] = baseAtrs[i] * atrLimit;

            int currentTrend = 0;
            double lastHigh = 0.0;
            double lastLow = 0.0;


            for (int i = 1; i < prices.Count; i++) {
                if (currentTrend == 0) {
                    if(CheckForConsecutiveHighs(prices))
                }



                if (currentTrend == 1) {
                    if (prices[i].High.Mid > prices[i - 1].High.Mid) {
                        if (prices[i].High.Mid > lastHigh) {
                            if (CheckForConsecutiveHighs(prices,  i,isolationLimit)) {

                            }
                        }

                    }
                }


            }

        }

        public bool CheckForConsecutiveHighs(List<BidAskData> prices, int consecutiveHighs, int skipsAllowed) {
            var end = prices.Count - 1 - consecutiveHighs - skipsAllowed;

            for (int j = prices.Count - 1; j > end && j > 0; j--) {
                bool consecutiveHigh = false;
                    for (int i = j; i < prices.Count && i - (j + 1) < skipsAllowed; i++)
                        if (prices[i].High.Mid > prices[j - 1].High.Mid) {
                            consecutiveHigh = true;
                            break;
                        }
                if (!consecutiveHigh) 
                        return false;
            }
            return true;
        }

        public bool CheckForConsecutiveLows(List<BidAskData> prices, int consecutiveHighs, int skipsAllowed) {
            var end = prices.Count - 1 - consecutiveHighs - skipsAllowed;

            for (int j = prices.Count - 1; j > end && j > 0; j--) {
                bool consecutivelow = false;
                for (int i = j; i < prices.Count && i - (j + 1) < skipsAllowed; i++)
                    if (prices[i].Low.Mid < prices[j - 1].Low.Mid) {
                        consecutivelow = true;
                        break;
                    }
                if (!consecutivelow)
                    return false;
            }
            return true;
        }


    }
    public enum SwingPoint
    {
        continuation,
        peak,
        trough,
        Range
    }

    public readonly struct Swing
    {
        public SwingPoint SwingDir { get; }
        public int Index { get; }

        public Swing(SwingPoint swingpoint, int index) {
            SwingDir = swingpoint;
            Index = index;
        }
    }
}
