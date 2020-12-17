using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStructures;
using RuleSets;
using RuleSets.Entry;
using TestUtils;
using Xunit;

namespace Thought.Tests
{

    public class BackTestFixture
    {
        private Universe _universeData { get; }
        public BacktestTests.Backtest TestinObject { get; }
        public List<Trade> TradesGenerated { get; }
        public List<Trade> Trades = new List<Trade>()
        {
            new Trade(new DatedResult[]
            {
                new DatedResult(new DateTime(1,1,1,1,1,1).Ticks, 0.1,-0.1 ),
                new DatedResult(new DateTime(1,1,10,1,1,1).Ticks, 0.15,-0.15 ),
                new DatedResult(new DateTime(1,1,20,1,1,1).Ticks, 0.2,-0.15 ),
                new DatedResult(new DateTime(1,1,30,1,1,1).Ticks, 0.25,-0.2 ),
            }, 0),
            new Trade(new DatedResult[]
            {
                new DatedResult(new DateTime(1,1,1,1,1,1).Ticks, 0.1,-0.05 ),
                new DatedResult(new DateTime(1,1,5,1,1,1).Ticks, 0.05,-0.05 ),
                new DatedResult(new DateTime(1,1,10,1,1,1).Ticks, -0.05,-0.15 ),
                new DatedResult(new DateTime(1,1,15,1,1,1).Ticks, 0.05,-0.15 ),
                new DatedResult(new DateTime(1,1,20,1,1,1).Ticks, 0.1,-0.15 ),
            }, 0),
            new Trade(new DatedResult[]
            {
                new DatedResult(new DateTime(1,1,1,1,1,1).Ticks, 0.1,-0.05 ),
                new DatedResult(new DateTime(1,1,2,1,1,1).Ticks, 0.05,-0.05 ),
                new DatedResult(new DateTime(1,1,3,1,1,1).Ticks, -0.05,-0.1 ),
                new DatedResult(new DateTime(1,1,4,1,1,1).Ticks, 0.05,-0.11 ),
                  new DatedResult(new DateTime(1, 1, 5,1,1,1).Ticks, 0.08, -0.12),
                  new DatedResult(new DateTime(1, 1, 6,1,1,1).Ticks, 0.12, -0.13),
                  new DatedResult(new DateTime(1, 1, 7,1,1,1).Ticks, 0.14, -0.14),
                  new DatedResult(new DateTime(1, 1, 8,1,1,1).Ticks, 0.16, -0.15),
                  new DatedResult(new DateTime(1, 1, 9,1,1,1).Ticks, 0.1, -0.16),
                  new DatedResult(new DateTime(1, 1, 10,1,1,1).Ticks, 0.1, -0.2),
            }, 0),
        };

        public BackTestFixture()
        {
            _universeData  = new Universe(new IRuleSet[1] { new DummyEntries(5, 10000) });
            _universeData.AddMarket(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(6000), "longTest");
            _universeData.AddMarket(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(4500), "mediumMarket");
            _universeData.AddMarket(new RandomBars(new TimeSpan(0, 0, 5)).GenerateRandomMarket(2000), "shortMarket");

            TestinObject = new BacktestTests.Backtest(_universeData);
            TradesGenerated = TestinObject.RunBackTest(new StrategyExecuter(MarketSide.Bull, true, new ExitPrices(0.9,1.1)));
        }
    }

    public class BacktestTests : IClassFixture<BackTestFixture>
    {
        private BackTestFixture _fixture;

        public BacktestTests(BackTestFixture fixt) {
            _fixture = fixt;
        }

        [Fact]
        private void GeneratesResults() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromDays(1), _fixture.TradesGenerated);
            Assert.True(results.Count > 0);
            for (int i = 0; i < results.Count; i++) {
                Assert.True(results[i].Return != 0);
                Assert.True(results[i].Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForDays() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromDays(1), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x => x.Drawdown == 0));
            Assert.False(results.All(x => x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(TimeSpan.FromDays(1).Ticks, results[i].Date - results[i-1].Date);
                Assert.True(results[i].Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForHours() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromHours(1), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x => x.Drawdown == 0));
            Assert.False(results.All(x => x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(TimeSpan.FromHours(1).Ticks, results[i].Date - results[i - 1].Date);
                Assert.True(results[i].Date != 0);
            }
        }

        [Fact]
        private void GeneratesResultsForArbitraryTicks() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromTicks(29556547847), _fixture.TradesGenerated);

            Assert.True(results.Count > 0);
            Assert.False(results.All(x=>x.Drawdown == 0));
            Assert.False(results.All(x=>x.Return == 0));
            for (int i = 1; i < results.Count; i++) {
                Assert.Equal(29556547847, results[i].Date - results[i - 1].Date);
                Assert.True(results[i].Date != 0);
            }
        }


        [Fact]
        private void ShouldGenerateResultsInCorrectOrder() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromDays(10), _fixture.Trades);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 11).Ticks, 0.15 - 0.05 + 0.1, (-0.3 + -0.2)/3), results[0]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 21).Ticks, 0.2 + 0.1, -0.15), results[1]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 31).Ticks, 0.25, -0.2), results[2]);
        }

        [Fact]
        private void ShouldGenerateResultsInCorrectOrderOnSmallScale() {
            var results = _fixture.TestinObject.ParseResults(TimeSpan.FromDays(5), _fixture.Trades);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 6).Ticks, 0.1 + 0.05 + 0.08, (-0.1-0.05 -0.12)/3), results[0]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 11).Ticks, 0.15 - 0.05 + 0.1, (-0.15-0.15 -0.2)/3), results[1]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 16).Ticks, 0.15 + 0.05, (-0.15-0.15 )/2), results[2]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 21).Ticks, 0.2 + 0.1 , (-0.15- 0.15) /2), results[3]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 26).Ticks, 0.2, (-0.15)/1), results[4]);
            AssertDatedResult(new DatedResult(new DateTime(1, 1, 31).Ticks, 0.25, (-0.2)/1), results[5]);
        }

        private void AssertDatedResult(DatedResult expected, DatedResult results) {
            Assert.Equal(expected.Date, results.Date);
            Assert.Equal(expected.Return, results.Return,6);
            Assert.Equal(expected.Drawdown, results.Drawdown,6);
        }



        public class Backtest
        {
            public Universe Markets { get; }

            public Backtest(Universe markets) {
                Markets = markets;

            }

            public List<Trade> RunBackTest(StrategyExecuter exec) {
                var results = new List<Trade>();
                foreach (var element in Markets.Elements) {
                    var trades = exec.Execute(element);
                    foreach (var trade in trades)
                        results.Add(trade);
                }

                return results;
            }

            private List<Trade> _orderedTrades;
            private List<DatedResult> _categorisedResults;
            private long _totalSpan;

            public List<DatedResult> ParseResults(TimeSpan time, List<Trade> results) {
                Initialise(results);
                for (long i = 0; i < _totalSpan; i += time.Ticks) 
                    SumAndAdd(ParseTrades(time, i),  i+ time.Ticks);
                return _categorisedResults;
            }

            private void Initialise(List<Trade> results) {
                _orderedTrades = results.OrderBy(x => x.Results.Last().Date).ToList();
                _totalSpan = (_orderedTrades.Last().Results.Last().Date - _orderedTrades.First().Results.First().Date);
                _categorisedResults = new List<DatedResult>();
            }

            private List<DatedResult> ParseTrades(TimeSpan time, long i) {
                var resultsTw = new List<DatedResult>();
                foreach (var t in _orderedTrades) {
                    if (CheckRelevant(time, t, i)) 
                        continue;
                    AddRelevantResult(time, t, i, resultsTw);
                }
                return resultsTw;
            }

            private void SumAndAdd(List<DatedResult> resultsTw, long date) {
                var avgDD = GetDrawdown(resultsTw);
                var sum = resultsTw.Sum(x => x.Return);
                _categorisedResults.Add(new DatedResult(date, sum, avgDD));
            }

            private static double GetDrawdown(List<DatedResult> resultsTw) {
                if (resultsTw.Any(x => x.Drawdown < 0))
                    return resultsTw.Where(x => x.Drawdown < 0).Average(x => x.Drawdown);
                else
                    return  0;
            }

            private void AddRelevantResult(TimeSpan time, Trade t, long i, List<DatedResult> resultsTw) {
                var relevantResult = t.Results.LastOrDefault(x => x.Date < i + time.Ticks);
                if (relevantResult.Date > 0)
                    resultsTw.Add(relevantResult);
            }

            private bool CheckRelevant(TimeSpan time, Trade t, long i) {
                if (t.Results.Last().Date < i)
                    return true;
                if (t.Results.First().Date > i + time.Ticks)
                    return true;
                return false;
            }
        }
    }
}