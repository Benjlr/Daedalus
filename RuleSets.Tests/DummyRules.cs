using System;
using System.Collections.Generic;
using System.Linq;
using DataStructures;
using Logic;
using RuleSets.Entry;
using Xunit;

namespace RuleSets.Tests
{
    public class DummyRules
    {
        [Fact]
        public void ShouldCreateDummyEntries() {
            var dummies = new DummyEntries(2,20);
            dummies.CalculateBackSeries(_data, _dataBA);
            Assert.Equal(10,dummies.Satisfied.Count(x=>x));
        }

        [Fact]
        public void ShouldCreateDummyExits() {
            var dummies = new DummyExits(3, 20);
            dummies.CalculateBackSeries(_data, _dataBA);
            Assert.Equal(7, dummies.Satisfied.Count(x => x));
        }


        private List<SessionData> _data = new List<SessionData>()
        {
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
            new SessionData(),
        };
        private BidAskData[] _dataBA = new BidAskData[]
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
