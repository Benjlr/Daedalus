using DataStructures.PriceAlgorithms;
using System.Collections.Generic;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class SwingTests
    {

        [Fact]
        private void ShouldDetectConsecutiveHighs (){
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[2] {barOne, barTwo},  1,0));
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barThree },  1, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barOne },  1, 0));
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barOne,barTwo },  1, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barOne,barTwo,barOne }, 1, 0));

        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveHighs() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);
            var barFive = new BidAskData(9);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barTwo, barThree }, 2, 0));
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barThree,barFive },  2, 0));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barThree, barOne, barThree,barOne },  2, 0));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveHighs() {
            var bidaskList = new BidAskData[100];
            for (int i = 0; i < 100; i++) 
                bidaskList[i] = new BidAskData(i);            

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 50, 0));
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 75, 0));
            Assert.True(swings.CheckForConsecutiveHighs(bidaskList, 99, 0));
        }

        [Fact]
        private void ShouldDetectConsecutiveHighsWithNeighbourAllowance() {
            var barOne = new BidAskData(5);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(7);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barTwo, barThree }, 1,1));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne,barThree, barOne}, 1,1));
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barTwo,barOne, barThree }, 2,1));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barTwo, barOne }, 1,1));
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

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix,barOne,barOne,barSeven }, 4, 2));
            Assert.True(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree, barTwo, barFour,barOne,barFive }, 3, 1));

            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barOne, barThree, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barSix,barFive, barFour, barThree,barTwo,barOne}, 4, 4));
            Assert.False(swings.CheckForConsecutiveHighs(new BidAskData[] { barThree,barFour,barFive, barSix, barOne,barOne,barSeven}, 4, 1));
        }

        [Fact]
        private void ShouldDetectConsecutiveLows() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barTwo }, 1, 0));
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barThree }, 1, 0));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barOne }, 1, 0));
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barOne, barTwo }, 1, 0));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barOne, barTwo, barOne }, 1, 0));

        }

        [Fact]
        private void ShouldDetectMultipleConsecutiveLows() {
            var barOne = new BidAskData(9);
            var barTwo = new BidAskData(7);
            var barThree = new BidAskData(6);
            var barFive = new BidAskData(5);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barTwo, barThree }, 2, 0));
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barThree, barFive }, 2, 0));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barThree, barOne, barThree, barOne }, 2, 0));
        }

        [Fact]
        private void ShouldDetectManyConsecutiveLows() {
            var bidaskList = new BidAskData[100];
            for (int i = 99; i >= 0; i--) 
                bidaskList[i] = new BidAskData(100-i);            

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 50, 0));
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 75, 0));
            Assert.True(swings.CheckForConsecutiveLows(bidaskList, 99, 0));
        }

        [Fact]
        private void ShouldDetectConsecutiveLowsWithNeighbourAllowance() {
            var barOne = new BidAskData(7);
            var barTwo = new BidAskData(6);
            var barThree = new BidAskData(5);

            var swings = new Swings(0, 0, 0, 0, 0, 0);
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barTwo, barThree }, 1, 1));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barThree, barOne }, 1, 1));
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barTwo, barOne, barThree }, 2, 1));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barTwo, barOne }, 1, 1));
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

            var swings = new Swings(0,0,0,0,0,0);
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barOne, barOne, barFour, barOne, barOne, barFive, barOne, barOne, barSix, barOne, barOne, barSeven }, 4, 2));
            Assert.True(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barTwo, barFour, barOne, barFive }, 3, 1));

            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barOne, barThree, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barSix, barFive, barFour, barThree, barTwo, barOne }, 4, 4));
            Assert.False(swings.CheckForConsecutiveLows(new BidAskData[] { barThree, barFour, barFive, barSix, barOne, barOne, barSeven }, 4, 1));
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
            _consecutiveHighExtremes = consecutiveHighs;
            _consecutiveLowExtremes = consecutiveLows;
            _neighboursAllowedHigh = neighboursAllowedHigh;
            _neighboursAllowedLow = neighboursAllowedLows;
            _prevValues = new BidAskData[20];

        }
                
        private double _atr { get; set; }
        private BidAskData[] _prevValues { get; set; }
        private int currentTrend  { get; set; }
        private double lastHigh  { get; set; }
        private double lastLow  { get; set; }
        private int _index = 0;

        public Swing Calculate(BidAskData price) {
            _atr = AverageTrueRange.Calculate(price, _prevValues[^1].Close.Mid, _atr);
            var atrReversalHigh = _atr * _atrLimitHighs;
            var atrReversalLow = _atr * _atrLimitLows;
            SwingPoint mySwing = SwingPoint.continuation;

            InitialiseTrend(atrReversalHigh, atrReversalLow, price);

            if (currentTrend == 1) {
                if (price.High.Mid > lastHigh) {
                    lastHigh = price.High.Mid;
                    mySwing = SwingPoint.peak;
                }


                if (CheckForConsecutiveLows(_prevValues, _consecutiveLowExtremes, _neighboursAllowedLow)) {
                    if (atrReversalLow < lastHigh - price.Low.Mid) {
                        lastLow = price.Low.Mid;
                        currentTrend = -1;
                        mySwing = SwingPoint.trough;
                    }
                }
            }

            if (currentTrend == -1) {
                if (price.Low.Mid < lastLow) {
                    lastLow = price.Low.Mid;
                    mySwing = SwingPoint.trough;
                }

                if (CheckForConsecutiveHighs(_prevValues, _consecutiveHighExtremes, _neighboursAllowedHigh)) {
                    if (atrReversalHigh < lastHigh - price.Low.Mid) {
                        lastHigh = price.High.Mid;
                        currentTrend = 1;
                        mySwing = SwingPoint.peak;
                    }
                }
            }

            return new Swing(mySwing, _index++);
        }
        
        public void InitialiseTrend(double atrReversalHigh, double atrReversalLow, BidAskData price) {
            if (currentTrend == 0) {
                if (CheckForConsecutiveHighs(_prevValues, _consecutiveHighExtremes, _neighboursAllowedHigh)) {
                    if (atrReversalHigh < price.High.Mid - lastLow)
                        currentTrend = 1;
                }
                else if (CheckForConsecutiveLows(_prevValues, _consecutiveLowExtremes, _neighboursAllowedLow)) {
                    if (atrReversalLow < lastHigh - price.Low.Mid)
                        currentTrend = -1;
                }
            }
        }


        public bool CheckForConsecutiveHighs(BidAskData[] prices, int consecutiveHighs, int skipsAllowed) {
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

        public bool CheckForConsecutiveLows(BidAskData[] prices, int consecutiveHighs, int skipsAllowed) {
            var end = prices.Length - 1 - consecutiveHighs - skipsAllowed;

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
        trough
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
