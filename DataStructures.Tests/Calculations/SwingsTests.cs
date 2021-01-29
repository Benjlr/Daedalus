using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public  class SwingsTests
    {
        List<BidAskData> myData = new List<BidAskData>()
        {
            new BidAskData(5),
            new BidAskData(6),
            new BidAskData(7),
            new BidAskData(6),
            new BidAskData(5),
            new BidAskData(6),
        };


        [Fact]
        private void ShouldFindPeak() {
            var sings  = new Swings(0, new ExtremesFinder(false,1,0),0, new ExtremesFinder(true, 1, 0));
            var results = new List<Swing>();
            for (int i = 0; i < myData.Count; i++) 
                results.Add(sings.Calculate(myData[i]));
            
            Assert.Equal(new List<Swing>()
            {
                new Swing(SwingPoint.continuation,0),
                new Swing(SwingPoint.continuation,1),
                new Swing(SwingPoint.peak,2),
                new Swing(SwingPoint.continuation,3),
                new Swing(SwingPoint.trough,4)
            }, results);
        }
    }

    public class Swings
    {
        private double _atrLimitHighs { get; }
        private double _atrLimitLows { get; }
        private ExtremesFinder _highs { get; }
        private ExtremesFinder _lows { get; }

        public Swings(double atrLimitHigh, ExtremesFinder highs, double atrLimitLow, ExtremesFinder lows) {
            _atrLimitHighs = atrLimitHigh;
            _atrLimitLows = atrLimitLow;
            _highs = highs;
            _lows = lows;
        }

        private double _atr { get; set; }
        private BidAskData _prevValue { get; set; }
        private int currentTrend { get; set; }
        private double lastHigh { get; set; }
        private double lastLow { get; set; }

        private int _index = -1;

        public Swing Calculate(BidAskData price) {
            _atr = AverageTrueRange.Calculate(price, _prevValue.Close.Mid, _atr);
            var atrReversalHigh = _atr * _atrLimitHighs;
            var atrReversalLow = _atr * _atrLimitLows;
            SwingPoint mySwing = SwingPoint.continuation;

            if(currentTrend == 0)
                InitialiseTrend(atrReversalHigh, atrReversalLow, price);

            if (currentTrend == 1) {
                if (price.High.Mid > lastHigh)
                    lastHigh = price.High.Mid;
                else
                    mySwing = SwingPoint.peak;

                if (_lows.CheckExtreme(price.Low.Mid)) {
                    if (atrReversalLow < lastHigh - price.Low.Mid) {
                        lastLow = price.Low.Mid;
                        currentTrend = -1;
                        //mySwing = SwingPoint.trough;
                    }
                }
            }
            else {
                if (price.Low.Mid < lastLow) 
                    lastLow = price.Low.Mid;
                else
                    mySwing = SwingPoint.trough;

                if (_highs.CheckExtreme(price.High.Mid)) {
                    if (atrReversalHigh < lastHigh - price.Low.Mid) {
                        lastHigh = price.High.Mid;
                        currentTrend = 1;
                        //mySwing = SwingPoint.peak;
                    }
                }
            }

            _prevValue = price;
            return new Swing(mySwing, _index++);
        }

        public void InitialiseTrend(double atrReversalHigh, double atrReversalLow, BidAskData price) {
            if (currentTrend == 0) {
                if (_highs.CheckExtreme(price.High.Mid)) {
                    if (atrReversalHigh < price.High.Mid - lastLow)
                        currentTrend = 1;
                }
                else if (_lows.CheckExtreme(price.Low.Mid)) {
                    if (atrReversalLow < lastHigh - price.Low.Mid)
                        currentTrend = -1;
                }
            }
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
