using System;
using DataStructures;

namespace TestUtils
{
    class FBETestBars
    {
        public static BidAskData[] DataLong = new BidAskData[]
        {
            new BidAskData(new DateTime(1,1,1),10,10,11,11,9,9,11,11,620 ), //  s           0           s   
            new BidAskData(new DateTime(1,1,2),9,9,11,11,8,8,11,11,400 ), //    b9          0               b9
            new BidAskData(new DateTime(1,1,3),12,12,14,14,10,10,11,11,48 ),//  s 12b9      12/9        s   12b9
            new BidAskData(new DateTime(1,1,4),13,13,14,14,13,13,11,11,678 ), //13b9, b13   13/9            13b9    b13
            new BidAskData(new DateTime(1,1,5),15,15,18,18,9,9,12,12,87 ), //   s 15b13     15/13       s   15b9    15b13
            new BidAskData(new DateTime(1,1,6),7,7,9,9,6,6,9,9,98 ), // 7b13 b7             7/13            7b9     7b13    b7
            new BidAskData(new DateTime(1,1,7),6,6,7,7,4,4,4,4,1234 ), //       s 6b7       6/7         s   6b13    6b7
            new BidAskData(new DateTime(1,1,8),9,9,10,10,8,8,10,10,625430 ), // 9b7 b9      9/7             9b13    9b7     b9
            new BidAskData(new DateTime(1,1,9),13,13,14,14,9,9,11,11,6260 ),//  s 13b9      13/9        s   13b7    13b9
            new BidAskData(new DateTime(1,1,10),14,14,16,16,13,13,15,15,1 ),// 14b9 b14     14/9            14b7    14b9    b14
        };

    }
}
