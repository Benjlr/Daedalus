using System;
using System.Collections.Generic;
using DataStructures;

namespace TestUtils
{
    public class FSTETestsBars
    {
        public static BidAskData[] DataLong = new BidAskData[]
        {
            new BidAskData(new DateTime(1,1,1),10,10,10,10,10,10,9.5,9.5,620 ), //  s           0           s   
            new BidAskData(new DateTime(1,1,2),10.5,10.5,10.5,10.5,10.5,10.5,10.5,10.5,400 ), //    b9          0               b9
            new BidAskData(new DateTime(1,1,2),11,11,11,11,11,11,11,11,400 ), //    b9          0               b9
            new BidAskData(new DateTime(1,1,2),11.5,11.5,11.5,11.5,11.5,11.5,11.5,11.5,400 ), //   b9          0               b9
            new BidAskData(new DateTime(1,1,3),11,11,14,14,11,11,11,11,48 ),
            new BidAskData(new DateTime(1,1,3),10,10,11,11,10,10,11,11,48 ),
            new BidAskData(new DateTime(1,1,3),15,15,11,11,11,11,11,11,48 ),
            new BidAskData(new DateTime(1,1,3),11,11,11,11,11,11,11,11,48 ),
            new BidAskData(new DateTime(1,1,3),9,9,9,9,9,9,9,9,48),//ddd
            new BidAskData(new DateTime(1,1,3),8,8,8,8,8,8,8,8,48),
            new BidAskData(new DateTime(1,1,3),8,8,8,8,6,6,8,8,48),
        };

        private List<Trade> _ListOne = new List<Trade>()
        {
            new Trade(new double[]{ 0, 0.05, 0.1}, 3),
            new Trade(new double[]{ 0, 0.05/10.5, 1/10.5, 3.5/10.5}, 4),
            new Trade(new double[]{ 0, 0.5/11,3/11.0}, 3),
            new Trade(new double[]{ 0,2.5/11 }, 2),
            new Trade(new double[]{ 3/11.0 }, 1),
            new Trade(new double[]{ 0, 5/10.0}, 2),
            new Trade(new double[]{ 0, -11/15.0}, 2),
            new Trade(new double[]{ 0, -9/11.0}, 2),
            new Trade(new double[]{ 0, -8/9.0, -6/9.0}, 3),
        };

    }
}
