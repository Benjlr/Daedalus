using System;
using System.Collections.Generic;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class NRWRTests
    {
        [Fact]
        private void ShouldCalculateNRWRCorrectly() {

            var nrs = NRWRBars.Calculate(data);
            Assert.Equal(new List<int>()
            {
                0, -1, -2, 3, 4, -3, -4, 2, 4
            }, nrs);
        }


        public List<SessionData> data = new List<SessionData>()
        {
            new SessionData(new DateTime(2020,01,01), 7, 5,7,3,5),
            new SessionData(new DateTime(2020,01,01), 7, 5,6,4,5), //-1
            new SessionData(new DateTime(2020,01,01), 7, 5,5,5,5), //-2
            new SessionData(new DateTime(2020,01,01), 7, 5,8,2,5), //3
            new SessionData(new DateTime(2020,01,01), 7, 5,9,1,5), //4
            new SessionData(new DateTime(2020,01,01), 7, 5,6,3,5), //-3
            new SessionData(new DateTime(2020,01,01), 7, 5,5,5,5), //-4
            new SessionData(new DateTime(2020,01,01), 7, 5,6,4,5), //2
            new SessionData(new DateTime(2020,01,01), 7, 5,8,2,5), //4
        };

    }
}
