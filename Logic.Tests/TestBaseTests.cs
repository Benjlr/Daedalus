using Logic.Analysis.Metrics;
using Logic.Metrics;
using RuleSets;
using RuleSets.Entry;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Logic.Tests
{
    public class TestBaseTests
    {
        private List<ITest[]> myTests { get; set; }
        private List<ITest[]> myUnLeakedTests { get; set; }
        private ITest myInvalidTests { get; set; }
        private string marketData => Directory.GetCurrentDirectory() + "\\FBEData\\TestMarketData.txt";

        public TestBaseTests()
        {
            var market = Market.MarketBuilder.CreateMarket(marketData);
            PrepareInvalidTests(market);
            PrepareValidTests(market);
            PrepareUnleakedTests(market);
        }

        private void PrepareInvalidTests(Market market) {
            var invalidstrat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(65, 98)
            }, market);
            myInvalidTests = TestFactory.GenerateFixedBarExitTest(invalidstrat, market, new FixedBarExitTestOptions(10, 13, 1, MarketSide.Bull))[0];
        }

        private void PrepareValidTests(Market market) {
            var strat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(1, 98)
            }, market);

            myTests = RunTests(market, strat);
        }

        private List<ITest[]> RunTests(Market market, Strategy strat) {
            var longSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 13, 1, MarketSide.Bull));
            var shortSide = TestFactory.GenerateFixedBarExitTest(strat, market, new FixedBarExitTestOptions(10, 13, 1, MarketSide.Bear));
            var tests = new List<ITest[]>();
            for (int i = 0; i < longSide.Count; i++)
                tests.Add(new[] {longSide[i], shortSide[i]});
            return tests;
        }

        private void PrepareUnleakedTests(Market market) {
            var unleakedstrat = Strategy.StrategyBuilder.CreateStrategy(new IRuleSet[]
            {
                new DummyEntries(5, 40)
            }, market);
            myUnLeakedTests = RunTests(market, unleakedstrat);
        }

        [Fact]
        public void ShouldGenerateCorrectLongAverages() {
            var avgGainsLong = new List<double>() { 0.01244771690797802, 0.014400505006063926, 0.015737763390891203, 0.018204759291922126 };
            var avgLossLong = new List<double>() { -0.018697242609358886, -0.020910917132935993, -0.023876578515158727, -0.02579140283497501 };
            var avgddLong = new List<double>() { -0.01838329093446867, -0.020101533188082804, -0.021778819228926948, -0.023411082029849106 };
            var avgddLongWinners = new List<double>() { -0.005618829265011208, -0.0057970859853173915, -0.006336135906326168, -0.006447398422215011 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][0].Stats.AvgGain, avgGainsLong[i]);
                Assert.Equal(myTests[i][0].Stats.AvgLoss, avgLossLong[i]);
                Assert.Equal(myTests[i][0].Stats.AverageDrawdown, avgddLong[i]);
                Assert.Equal(myTests[i][0].Stats.AverageDrawdownWinners, avgddLongWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectLongMedians() {
            var medianGainsLong = new List<double>() { 0.008864467385570663, 0.011036578233805206, 0.012710493943708445, 0.014713179421444776 };
            var medianLossLong = new List<double>() { -0.01214586468802871, -0.012681162089391709, -0.014312125546770709, -0.013911713188377887 };
            var medianddLong = new List<double>() { -0.010322003245703971, -0.011785830867070052, -0.012173816540066605, -0.013141636581465086 };
            var medianddLongWinners = new List<double>() { -0.0044049153184673936, -0.0044582760971514724, -0.00529928325665651, -0.00529928325665651 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][0].Stats.MedianGain, medianGainsLong[i]);
                Assert.Equal(myTests[i][0].Stats.MedianLoss, medianLossLong[i]);
                Assert.Equal(myTests[i][0].Stats.MedianDrawDown, medianddLong[i]);
                Assert.Equal(myTests[i][0].Stats.MedianDrawDownWinners, medianddLongWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortAverages() {
            var avgGainsShort = new List<double>() { 0.019621273540185995, 0.022190526223525554, 0.023245650158239272, 0.024879940179388021 };
            var avgLossShort = new List<double>() { -0.017182528059217973, -0.019445720571213108, -0.022436378857612859, -0.025377204864198862 };
            var avgddShort = new List<double>() { -0.018960876367252549, -0.021162181006161282, -0.023436320195020403, -0.025774816792660877 };
            var avgddShortWinners = new List<double>() { -0.0054592014443324921, -0.0055568575873372657, -0.0071542863760534134, -0.00723796863364926 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][1].Stats.AvgGain, avgGainsShort[i]);
                Assert.Equal(myTests[i][1].Stats.AvgLoss, avgLossShort[i]);
                Assert.Equal(myTests[i][1].Stats.AverageDrawdown, avgddShort[i]);
                Assert.Equal(myTests[i][1].Stats.AverageDrawdownWinners, avgddShortWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateCorrectShortMedians() {
            var medianGainsShort = new List<double>() { 0.010225505933879374, 0.011623407866219744, 0.011926267378336896, 0.015550258436265525 };
            var medianLossShort = new List<double>() { -0.013055959058833876, -0.016036301636998651, -0.019640629215600246, -0.022846196866666828 };
            var medianddShort = new List<double>() { -0.014196025165265871, -0.015789088963183925, -0.017030408796471184, -0.017593707656965815 };
            var medianddShortWinners = new List<double>() { -0.00302215419474395, -0.0029988855832527575, -0.00302215419474395, -0.0037233267991365264 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][1].Stats.MedianGain, medianGainsShort[i]);
                Assert.Equal(myTests[i][1].Stats.MedianLoss, medianLossShort[i]);
                Assert.Equal(myTests[i][1].Stats.MedianDrawDown, medianddShort[i]);
                Assert.Equal(myTests[i][1].Stats.MedianDrawDownWinners, medianddShortWinners[i]);
            }
        }

        [Fact]
        public void ShouldGenerateWinRatios() {
            var longRatios = new List<double>() { 0.4183673469, 0.4742268041, 0.5051546392, 0.4895833333 };
            var shortRatios = new List<double>() { 0.2783505155, 0.2783505155, 0.2886597938, 0.2989690722 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][0].Stats.WinPercent, longRatios[i]);
                Assert.Equal(myTests[i][1].Stats.WinPercent, shortRatios[i]);
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyLong() {
            var avgExp = new List<double>() { -0.0071620724177526244, -0.0076286390806516195, -0.0080308417527387536, -0.0083514827126013741 };
            var medianExp = new List<double>() { -0.004364260216325239, -0.0037598102247029608, -0.0035030777505790462, -0.0025649089106103444 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][0].Stats.AverageExpectancy, avgExp[i]);
                Assert.Equal(myTests[i][0].Stats.MedianExpectancy, medianExp[i]);
            }
        }

        [Fact]
        public void ShouldGenerateExpectancyShort() {
            var avgExp = new List<double>() { -0.0079815776593669813, -0.0087501709358673956, -0.0095623524985999873, -0.010435891472862219 };
            var medianExp = new List<double>() { -0.0072355928106555643, -0.0089310551591076856, -0.010744503811854326, -0.011431034479308558 };
            for (var i = 0; i < myTests.Count; i++) {
                Assert.Equal(myTests[i][1].Stats.AverageExpectancy, avgExp[i]);
                Assert.Equal(myTests[i][1].Stats.MedianExpectancy, medianExp[i]);
            }
        }


    }
}
