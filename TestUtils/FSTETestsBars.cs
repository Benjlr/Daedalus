using System;
using System.Collections.Generic;
using DataStructures;

namespace TestUtils
{
    public class FSTETestsBars
    {
        public static BidAskData[] DataLong = new BidAskData[]
        {
            new BidAskData(new DateTime(1,1,1),10,9.5,10,9.5,10,9.5,9.5,9.0,620 ), //  s          0           s   
            new BidAskData(new DateTime(1,1,2),11,10.5,11,10.5,11,10.5,11,10.5,400 ), //    b9          0               b9
            new BidAskData(new DateTime(1,1,2),11,10.5,11,10.5,11,10.5,11,10.5,400 ), //    b9          0               b9
            new BidAskData(new DateTime(1,1,2),12,11.5,12,11.5,12,11.5,12,11.5,400 ), //   b9          0               b9
            new BidAskData(new DateTime(1,1,3),11,10.5,14,13.5,11,10.5,11,10.5,48 ),
            new BidAskData(new DateTime(1,1,3),10,9.5,11,10.5,10,9.5,11,10.5,48 ),
            new BidAskData(new DateTime(1,1,3),15,14.5,15,14.5,11,10.5,11,10.5,48 ),
            new BidAskData(new DateTime(1,1,3),11,10.5,11,10.5,11,10.5,11,10.5,48 ),
            new BidAskData(new DateTime(1,1,3),9,8.5,10,9.5,8,7.5,8,7.5,48),//ddd
            new BidAskData(new DateTime(1,1,3),8,7.5,9,8.5,7,6.5,8,7.5,48),
            new BidAskData(new DateTime(1,1,3),8,7.5,8,7.5,6,5.5,6,5.5,48),
            new BidAskData(new DateTime(1,1,3),7,6.5,10,9.5,7,6.5,8,7.5,48),
        };

        public static List<Trade> _shortSmallStopTarget = new List<Trade>()
        {
            new Trade(new double[]{
                (10.5 / 11.0) - 1,
                (10.5 / 11.0) - 1,
                (11.5/11.0) -1,
                (11*1.15/11.0) -1,

            }, 1),
            new Trade(new double[]{
                (11.5/12) -1,
                (10.5/12) -1,
                (9.5/12) -1,
            }, 3),
            new Trade(new double[]{
                (10.5/10) - 1,
                (14.5/10) - 1,
            }, 5),
            new Trade(new double[]{
                (10.5/11) -1,
                (8.5/11) -1,

            }, 7),
            new Trade(new double[]{
                (8*0.85/8) -1,
            }, 9),
            new Trade(new double[]{
                (7*1.15/7) -1,
            }, 11),
        };

        public static List<Trade> _shortLargerStopTarget = new List<Trade>()
        {
            new Trade(new double[]{
                (10.5 / 11.0) - 1,
                (10.5 / 11.0) - 1,
                (11.5/11.0) -1,
                (10.5/11.0) -1,
                (10.5/11.0) -1,
                (14.5/11.0) -1,
            }, 1),
            new Trade(new double[]{
                (11.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (12*0.7/12) -1,
            }, 3),
            new Trade(new double[]{
                (10.5/10) - 1,
                (14.5/10) - 1,
            }, 5),
            new Trade(new double[]{
                (10.5/11) -1,
                (11*0.7/11) -1,

            }, 7),
            new Trade(new double[]{
                (7.5/8) -1,
                (0.7*8/8) -1,
            }, 9),
            new Trade(new double[]{
                (7*1.3/7) -1,
            }, 11),
        };


        public static List<Trade> _longSmallStopTarget = new List<Trade>()
        {
            new Trade(new double[]{
                (10.5 / 11.0) - 1, 
                (10.5 / 11.0) - 1,
                (11.5/11.0) -1, 
                (11*1.15/11.0) -1,
                
            }, 1),
            new Trade(new double[]{
                (11.5/12) -1,
                (10.5/12) -1,
                (9.5/12) -1,
            }, 3),
            new Trade(new double[]{
                (10.5/10) - 1,
                (14.5/10) - 1,
            }, 5),
            new Trade(new double[]{
                (10.5/11) -1,
                (8.5/11) -1,

            }, 7),
            new Trade(new double[]{
                (8*0.85/8) -1,
            }, 9),
            new Trade(new double[]{
                (7*1.15/7) -1,
            }, 11),
        };

        public static List<Trade> _longLargerStopTarget = new List<Trade>()
        {
            new Trade(new double[]{
                (10.5 / 11.0) - 1,
                (10.5 / 11.0) - 1,
                (11.5/11.0) -1,
                (10.5/11.0) -1,
                (10.5/11.0) -1,
                (14.5/11.0) -1,
            }, 1),
            new Trade(new double[]{
                (11.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (10.5/12) -1,
                (12*0.7/12) -1,
            }, 3),
            new Trade(new double[]{
                (10.5/10) - 1,
                (14.5/10) - 1,
            }, 5),
            new Trade(new double[]{
                (10.5/11) -1,
                (11*0.7/11) -1,

            }, 7),
            new Trade(new double[]{
                (7.5/8) -1,
                (0.7*8/8) -1,
            }, 9),
            new Trade(new double[]{
                (7*1.3/7) -1,
            }, 11),
        };

    }
}
