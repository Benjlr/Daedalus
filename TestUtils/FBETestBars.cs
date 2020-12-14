using System;
using System.Collections.Generic;
using DataStructures;

namespace TestUtils
{
    public class FBETestBars
    {


        public static readonly BidAskData[] DataLong = new BidAskData[]
        {
            new BidAskData(new DateTime(1, 1, 1), 10, 9.5, 11, 10.5, 9, 8.5, 11, 10.5, 620), //  s           0           s   
            new BidAskData(new DateTime(1, 1, 2), 9, 8.5, 11, 10.5, 8, 7.5, 11, 10.5, 400), //    b9          0               b9
            new BidAskData(new DateTime(1, 1, 3), 12, 11.5, 14, 13.5, 10, 9.5, 11, 10.5, 48), //  s 12b9      12/9        s   12b9
            new BidAskData(new DateTime(1, 1, 4), 13, 12.5, 14, 13.5, 11, 10.5, 11, 10.5, 678), //13b9, b13   13/9            13b9    b13
            new BidAskData(new DateTime(1, 1, 5), 15, 14.5, 18, 17.5, 9, 8.5, 12, 11.5, 87), //   s 15b13     15/13       s   15b9    15b13
            new BidAskData(new DateTime(1, 1, 6), 7, 6.5, 9, 8.5, 6, 5.5, 9, 8.5, 98), // 7b13 b7             7/13            7b9     7b13    b7
            new BidAskData(new DateTime(1, 1, 7), 6, 5.5, 7, 6.5, 4, 3.5, 4, 3.5, 1234), //       s 6b7       6/7         s   6b13    6b7
            new BidAskData(new DateTime(1, 1, 8), 9, 8.5, 10, 9.5, 8, 7.5, 10, 9.5, 625430), // 9b7 b9      9/7             9b13    9b7     b9
            new BidAskData(new DateTime(1, 1, 9), 13, 12.5, 14, 13.5, 9, 8.5, 11, 10.5, 6260), //  s 13b9      13/9        s   13b7    13b9
            new BidAskData(new DateTime(1, 1, 10), 14, 13.5, 16, 15.5, 13, 12.5, 15, 14.5, 1), // 14b9 b14     14/9            14b7    14b9    b14
        };

        public static readonly List<Trade> longTradesOne = new List<Trade>()
        {
            new Trade(TradeTimeMocker.Mock(new[] {(10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (12.5 / 9.0) - 1},new[] {(7.5 / 9.0) - 1, 0, 0},new DateTime(1, 1, 2)), 1),
            new Trade(TradeTimeMocker.Mock(new[] {(10.5 / 13.0) - 1, (11.5 / 13.0) - 1, (6.5 / 13) - 1}, new[] {(10.5 / 13.0) - 1, (8.5 / 13.0) - 1, (6.5 / 13.0) - 1},new DateTime(1, 1, 4)), 3),
            new Trade(TradeTimeMocker.Mock(new[] {(8.5 / 7.0) - 1, (3.5 / 7) - 1, (8.5 / 7.0) - 1},new[] {(5.5 / 7.0) - 1, (3.5 / 7.0) - 1, 0},new DateTime(1, 1, 6)), 5),
            new Trade(TradeTimeMocker.Mock(new[] {(9.5 / 9.0) - 1, (10.5 / 9.0) - 1, (13.5 / 9.0) - 1},new[] {(7.5 / 9.0) - 1, (8.5 / 9.0) - 1, 0},new DateTime(1, 1, 8)), 7),
            new Trade(TradeTimeMocker.Mock(new[] {(14.5 / 14.0) - 1},new[] {0.0},new DateTime(1, 1, 10)), 9),
        };
        public static readonly List<Trade> longTradesTwo = new List<Trade>()
        {
            new Trade(TradeTimeMocker.Mock(new[] {(10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (10.5 / 9.0) - 1, (11.5 / 9.0) - 1, (6.5 / 9.0) - 1}, new[] {(7.5 / 9.0) - 1, 0, 0,(8.5 / 9.0) - 1,(6.5 / 9.0) - 1},new DateTime(1, 1, 2)), 1),
            new Trade(TradeTimeMocker.Mock(new[] {(10.5 / 13.0) - 1, (11.5 / 13.0) - 1, (8.5 / 13.0) - 1, (3.5 / 13.0) - 1, (8.5 / 13.0) - 1}, new[] {(10.5 / 13.0) - 1, (8.5 / 13.0) - 1, (5.5 / 13.0) - 1, (3.5 / 13.0) - 1, (8.5 / 13.0) - 1},new DateTime(1, 1, 4) ), 3),
            new Trade(TradeTimeMocker.Mock(new[] {(8.5 / 7.0) - 1, (3.5 / 7.0) - 1, (9.5 / 7.0) - 1, (10.5 / 7.0) - 1, (13.5 / 7.0) - 1}, new[] {(5.5 / 7.0) - 1, (3.5 / 7.0) - 1, 0, 0, 0},new DateTime(1, 1, 6) ), 5),
            new Trade(TradeTimeMocker.Mock(new[] {(9.5 / 9.0) - 1, (10.5 / 9.0) - 1, (14.5 / 9) - 1}, new[] {(7.5 / 9.0) - 1, (8.5 / 9.0) - 1, 0},new DateTime(1, 1, 8) ), 7),
            new Trade(TradeTimeMocker.Mock(new[] {(14.5 / 14.0) - 1},new[] {0.0}, new DateTime(1, 1, 10)), 9),
        };
        public static readonly List<Trade> shortTradesOne = new List<Trade>()
        {
            new Trade(TradeTimeMocker.Mock(new[] {1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (13 / 8.5)}, new[] {1 - (11 / 8.5), 1 - (14 / 8.5), 1 - (13 / 8.5)}, new DateTime(1, 1, 2) ), 1),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (11 / 12.5), 1 - (12 / 12.5), 1 - (7.0 / 12.5)}, new[] {1 - (14 / 12.5), 1 - (18 / 12.5), 0}, new DateTime(1, 1, 4) ), 3),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (9 / 6.5), 1 - (4.0 / 6.5), 1 - (9 / 6.5)}, new[] { 1 - (9 / 6.5), 1 - (7 / 6.5), 1 - (9 / 6.5)}, new DateTime(1, 1, 6)   ), 5),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (10 / 8.5), 1 - (11 / 8.5), 1 - (14 / 8.5)}, new[] {1 - (10 / 8.5), 1 - (14 / 8.5), 1 - (14 / 8.5)},  new DateTime(1, 1, 8) ), 7),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (15 / 13.5)},new[] {1 - (15 / 13.5)}, new DateTime(1, 1, 10) ), 9),
        };
        public static readonly List<Trade> shortTradesTwo = new List<Trade>()
        {
            new Trade(TradeTimeMocker.Mock(new[] {1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (11 / 8.5), 1 - (12 / 8.5), 1 - (7 / 8.5)}, new[] {1 - (11 / 8.5), 1 - (14 / 8.5), 1 - (14 / 8.5), 1 - (18 / 8.5), 0},  new DateTime(1, 1, 2) ), 1),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (11 / 12.5), 1 - (12 / 12.5), 1 - (9 / 12.5), 1 - (4 / 12.5), 1 - (9 / 12.5)}, new[] {1 - (14 / 12.5), 1 - (18 / 12.5), 0, 0, 0.0}, new DateTime(1, 1, 4)  ), 3),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (9 / 6.5), 1 - (4 / 6.5), 1 - (10 / 6.5), 1 - (11 / 6.5), 1 - (14 / 6.5)}, new[] {1 - (9 / 6.5), 1 - (7 / 6.5), 1 - (10 / 6.5), 1 - (14 / 6.5), 1 - (14 / 6.5) },  new DateTime(1, 1, 6)), 5),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (10 / 8.5), 1 - (11 / 8.5), 1 - (15.0 / 8.5)}, new[] {1 - (10 / 8.5), 1 - (14 / 8.5), 1 - (15.0 / 8.5)} ,  new DateTime(1, 1, 8)  ), 7),
            new Trade(TradeTimeMocker.Mock(new[] {1 - (15 / 13.5) },new[] {1 - (15 / 13.5) }, new DateTime(1, 1, 10)), 9),
        };
    }
}
