using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataStructures.PriceAlgorithms;
using TestUtils;
using Xunit;

namespace DataStructures.Tests.Calculations
{
    public class ATRTests
    {
        private SessionData[] myArray = new SessionData[]
        {
            new SessionData(new DateTime(),12,5,6,4,5  ), 
            new SessionData(new DateTime(),12,10,11,9,10  ), 
            new SessionData(new DateTime(),12,11,12,10,11  ), 
            new SessionData(new DateTime(),12,2,2,2,2  ), 
            new SessionData(new DateTime(),12,3,4,3,3  ),
            new SessionData(new DateTime(),12,3,5,3,4  ),
            new SessionData(new DateTime(),12,4,20,4,15  ),
            new SessionData(new DateTime(),12,15,18,14,14  ),
            new SessionData(new DateTime(),12,14,16,12,13  ),
            new SessionData(new DateTime(),9,9,10,6,7  ),
        };


        [Fact]
        private void ShouldCalculateATRCorrectly() {
            var expected = new List<double>() {2, 2.8, 2.6399999999999997, 3.912, 3.5296, 3.2236800000000003, 5.778944, 5.4231552, 5.13852416, 5.510819328};
            var actual = AverageTrueRange.Calculate(myArray.ToList(), 5);
            Asserters.ListDoublesEqual(expected, actual);
        }

        [Fact]
        private void ShouldCalculateATRPCCorrectly() {
            var expected = new List<double>() {1, 1, 0.79999999999999982, 1, 0.69937106918238989, 0, 1, 0.86076241045934976, 0, 1};
            var actual = AverageTrueRange.CalculateATRPC(myArray.ToList(), 5, 3);
            Asserters.ListDoublesEqual(expected, actual);
        }
    }
}
