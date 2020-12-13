using DataStructures.PriceAlgorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using TestUtils;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class NRWRTests
    {
        [Fact]
        private void ShouldCalculateNRWRCorrectly() {

            var actual = NRWRBars.Calculate(data).Select(x=>(double)x).ToList();
            var expected = new List<double>()
            {
                0, -1, -2, 3, 4, -3, -4, 2, 4
            };
            Asserters.ListDoublesEqual(expected, actual);
        }


        public List<BidAskData> data = new List<BidAskData>()
        {
            new BidAskData(new DateTime(2020,01,01), 7, 5,7,3,5),
            new BidAskData(new DateTime(2020,01,01), 7, 5,6,4,5), //-1
            new BidAskData(new DateTime(2020,01,01), 7, 5,5,5,5), //-2
            new BidAskData(new DateTime(2020,01,01), 7, 5,8,2,5), //3
            new BidAskData(new DateTime(2020,01,01), 7, 5,9,1,5), //4
            new BidAskData(new DateTime(2020,01,01), 7, 5,6,3,5), //-3
            new BidAskData(new DateTime(2020,01,01), 7, 5,5,5,5), //-4
            new BidAskData(new DateTime(2020,01,01), 7, 5,6,4,5), //2
            new BidAskData(new DateTime(2020,01,01), 7, 5,8,2,5), //4
        };

    }
}
