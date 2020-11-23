using System;
using System.Collections.Generic;
using System.Text;
using DataStructures.PriceAlgorithms;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class PivotTests
    {
        [Fact]
        private void ShouldCalculatePivotCorrectly() {
            var pivs = Pivots.Calculate(data);
            Assert.Equal(new List<PivotStruct>()
            {
                new PivotStruct(1,1,1),
                new PivotStruct(0,2,3),
                new PivotStruct(2,1,4),
                new PivotStruct(1,0,6),
                new PivotStruct(3,3,8),
                new PivotStruct(1,1,10),
                new PivotStruct(2,2,12),
                new PivotStruct(1,1,14),
            }, pivs );
        }


        public List<SessionData> data = new List<SessionData>()
        {
            new SessionData(new DateTime(2020,01,01), 7, 5,7,5,5),
            new SessionData(new DateTime(2020,01,2),  7, 5,8,4,5), //1  -1
            new SessionData(new DateTime(2020,01,2),  7, 5,6,5,5), //   
            new SessionData(new DateTime(2020,01,3),  7, 5,7,3,5), //   -2
            new SessionData(new DateTime(2020,01,4),  7, 5,9,3,5), //2  -1
            new SessionData(new DateTime(2020,01,5),  7, 5,8,4,5), //
            new SessionData(new DateTime(2020,01,5),  7, 5,9,5,5), //1
            new SessionData(new DateTime(2020,01,5),  7, 5,8,4,5), //
            new SessionData(new DateTime(2020,01,6),  7, 5,10,2,5),//3  -3
            new SessionData(new DateTime(2020,01,7),  7, 5,6,4,5), //
            new SessionData(new DateTime(2020,01,8),  7, 5,7,3,5), //1  -1
            new SessionData(new DateTime(2020,01,9),  7, 5,6,4,5), //
            new SessionData(new DateTime(2020,01,10), 7, 5,9,2,5), //2  -2
            new SessionData(new DateTime(2020,01,11), 7, 5,7,5,5), //
            new SessionData(new DateTime(2020,01,12), 7, 5,8,4,5), //1  -1
            new SessionData(new DateTime(2020,01,13), 7, 5,7,5,5), //
        };
    }
}
