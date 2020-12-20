using DataStructures;
using RuleSets.Entry;
using System.Linq;
using Xunit;

namespace RuleSets.Tests
{
    public class DummyRules
    {
        [Fact]
        public void ShouldCreateDummyEntries() {
            var dummies = new DummyEntries(2,20);
            dummies.CalculateBackSeries( _dataBA);
            Assert.Equal(10,dummies.Satisfied.Count(x=>x));
        }

        [Fact]
        public void ShouldCreateDummyExits() {
            var dummies = new DummyExits(3, 20);
            dummies.CalculateBackSeries( _dataBA);
            Assert.Equal(7, dummies.Satisfied.Count(x => x));
        }

        private readonly BidAskData[] _dataBA = new BidAskData[]
        {
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
            new BidAskData(),
        };
    }
}
