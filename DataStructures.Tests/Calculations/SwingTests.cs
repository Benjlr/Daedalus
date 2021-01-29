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

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveHighs(1, 0,barOne, barTwo  ));
            Assert.True(swings.CheckForConsecutiveHighs(1, 0, barOne, barThree ));
            Assert.False(swings.CheckForConsecutiveHighs(1, 0, barThree, barOne ));
            Assert.True(swings.CheckForConsecutiveHighs(1, 0, barThree, barOne,barTwo ));
            Assert.False(swings.CheckForConsecutiveHighs(1, 0, barThree, barOne,barTwo,barOne ));

        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighs() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);
            var barFive = new BidAskData(9);

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveHighs(2, 0,  barOne, barTwo, barThree  ));
            Assert.True(swings.CheckForConsecutiveHighs(2, 0, barOne, barThree,barFive ));
            Assert.False(swings.CheckForConsecutiveHighs(2, 0, barOne, barThree, barOne, barThree,barOne));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveHighs() {
            var bidaskList = new BidAskData[100];
            for (int i = 0; i < 100; i++) 
                bidaskList[i] = new BidAskData(i);
            

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveHighs(50, 0 , bidaskList));
            Assert.True(swings.CheckForConsecutiveHighs(75, 0, bidaskList));
            Assert.True(swings.CheckForConsecutiveHighs(99, 0, bidaskList));
        }

        [Fact]
        private void ShouldDetectConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveHighs(1, 1, barOne, barTwo, barThree ));
            Assert.False(swings.CheckForConsecutiveHighs(1, 1, barOne,barThree, barOne));
            Assert.True(swings.CheckForConsecutiveHighs(2, 1, barTwo,barOne, barThree ));
            Assert.False(swings.CheckForConsecutiveHighs(1, 1, barThree, barTwo, barOne  ));
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

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveHighs(4, 2,barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix,barOne,barOne,barSeven  ));
            Assert.True(swings.CheckForConsecutiveHighs(3, 1, barThree, barTwo, barFour,barOne,barFive  ));

            Assert.False(swings.CheckForConsecutiveHighs(4, 4, barOne, barThree, barOne  ));
            Assert.False(swings.CheckForConsecutiveHighs(4, 4, barSix,barFive, barFour, barThree,barTwo,barOne ));
            Assert.False(swings.CheckForConsecutiveHighs(4, 1, barThree,barFour,barFive, barSix, barOne,barOne,barSeven));
        }

        [Fact]
        private void ShouldDetectConsecutiveLows() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveLows(1, 0, barOne, barTwo));
            Assert.True(swings.CheckForConsecutiveLows(1, 0, barOne, barThree));
            Assert.False(swings.CheckForConsecutiveLows(1, 0, barThree, barOne));
            Assert.True(swings.CheckForConsecutiveLows(1, 0, barThree, barOne, barTwo));
            Assert.False(swings.CheckForConsecutiveLows(1, 0, barThree, barOne, barTwo, barOne));
        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLows() {
            var barOne = new BidAskData(9);
            var barTwo = new BidAskData(7);
            var barThree = new BidAskData(6);
            var barFive = new BidAskData(5);

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveLows(2, 0, barOne, barTwo, barThree));
            Assert.True(swings.CheckForConsecutiveLows(2, 0, barOne, barThree, barFive));
            Assert.False(swings.CheckForConsecutiveLows(2, 0,barOne, barThree, barOne, barThree, barOne));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveLows() {
            var bidaskList = new BidAskData[100];
            for (int i = 100; i > 0; i--) {
                bidaskList[100-i] = new BidAskData(i);
            }

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveLows(50, 0, bidaskList));
            Assert.True(swings.CheckForConsecutiveLows(75, 0, bidaskList));
            Assert.True(swings.CheckForConsecutiveLows(99, 0, bidaskList));
        }

        [Fact]
        private void ShouldDetectConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings(0, 4, 2, 0, 4, 2);
            Assert.True(swings.CheckForConsecutiveLows(1, 1,  barOne, barTwo, barThree  ));
            Assert.False(swings.CheckForConsecutiveLows(1, 1, barOne, barThree, barOne  ));
            Assert.True(swings.CheckForConsecutiveLows(2, 1,  barTwo, barOne, barThree  ));
            Assert.False(swings.CheckForConsecutiveLows(1, 1,  barThree, barTwo, barOne));
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

            var swings = new Swings(0,4,2,0,4,2);
            Assert.True(swings.CheckForConsecutiveLows(4,2, barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix, barOne, barOne, barSeven ));
            Assert.True(swings.CheckForConsecutiveLows(3,1, barThree, barTwo, barFour, barOne, barFive ));

            Assert.False(swings.CheckForConsecutiveLows(4,4,barOne, barThree, barOne ));
            Assert.False(swings.CheckForConsecutiveLows(4, 4, barSix, barFive, barFour, barThree, barTwo, barOne ));
            Assert.False(swings.CheckForConsecutiveLows(4,1, barThree, barFour, barFive, barSix, barOne, barOne, barSeven ));
        }

    }

    public class Swings
    {
        private double _atrLimitHighs { get; }
        private double _atrLimitLows { get; }
        private int _consecutiveHighExtremes { get; }
        private int _consecutiveLowExtremes { get; }
        private int _neighboursAllowedHigh { get; }
        private int _neighboursAllowedLow { get; }

        public Swings(double atrLimitHigh, int consecutiveHighs, int neighboursAllowedHigh, double atrLimitLow, int consecutiveLows, int neighboursAllowedLows) {
            _atrLimitHighs = atrLimitHigh;
            _atrLimitLows = atrLimitLow;
            _consecutiveHighExtremes = consecutiveHighs;
            _consecutiveLowExtremes = consecutiveLows;
            _neighboursAllowedHigh = neighboursAllowedHigh;
            _neighboursAllowedLow = neighboursAllowedLows;
        }

        private double _atr { get; set; }

        public void Calculate(BidAskData price) {
            //var baseAtrs = AverageTrueRange.Calculate(price,);
            //var atrReversals = new double[prices.Count];
            //var swings = new List<Swing>();

            //for (var i = 0; i < prices.Count; i++)
            //    atrReversals[i] = baseAtrs[i] * atrLimit;

            //int currentTrend = 0;
            //double lastHigh = 0.0;
            //double lastLow = 0.0;


            //for (int i = 1; i < prices.Count; i++) {
            //    if (currentTrend == 0) {
            //        if (CheckForConsecutiveHighs(prices))
            //    }



            //    if (currentTrend == 1) {
            //        if (prices[i].High.Mid > prices[i - 1].High.Mid) {
            //            if (prices[i].High.Mid > lastHigh) {
            //                if (CheckForConsecutiveHighs(prices, i, isolationLimit)) {

            //                }
            //            }

            //        }
            //    }


            //}

        }

        public bool CheckForConsecutiveHighs(int consecutiveHighs, int skipsAllowed, params BidAskData[] prices) {
            var end = prices.Length - 1 - consecutiveHighs - skipsAllowed;

            for (int j = prices.Length - 1; j > end && j > 0; j--) {
                bool consecutiveHigh = false;
                    for (int i = j; i < prices.Length && i - (j + 1) < skipsAllowed; i++)
                        if (prices[i].High.Mid > prices[j - 1].High.Mid) {
                            consecutiveHigh = true;
                            break;
                        }
                if (!consecutiveHigh) 
                        return false;
            }
            return true;
        }

        public bool CheckForConsecutiveLows(int consecutiveLows, int skipsAllowed, params BidAskData[] prices) {
            var end = prices.Length - 1 - consecutiveLows - skipsAllowed;

            for (int j = prices.Length - 1; j > end && j > 0; j--) {
                bool consecutivelow = false;
                for (int i = j; i < prices.Length && i - (j + 1) < skipsAllowed; i++)
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
