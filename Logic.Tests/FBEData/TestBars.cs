using System;
using System.Collections.Generic;
using System.Text;
using PriceSeriesCore;
using Xunit.Sdk;

namespace Logic.Tests.FBEData
{
    class TestBars
    {
        public static MarketData[] DataLong = new MarketData[]
        {
            new MarketData(new DateTime(1,1,1),10,10,11,11,9,9,11,11,620 ), //
            new MarketData(new DateTime(1,1,2),9,9,11,11,8,8,11,11,400 ), 
            new MarketData(new DateTime(1,1,3),12,12,14,14,10,10,11,11,48 ),// 
            new MarketData(new DateTime(1,1,4),13,13,14,14,13,13,11,11,678 ), 
            new MarketData(new DateTime(1,1,5),15,15,18,18,9,9,12,12,87 ), //
            new MarketData(new DateTime(1,1,6),7,7,9,9,6,6,9,9,98 ), 
            new MarketData(new DateTime(1,1,7),6,6,7,7,4,4,4,4,1234 ), //
            new MarketData(new DateTime(1,1,8),9,9,10,10,8,8,10,10,625430 ), 
            new MarketData(new DateTime(1,1,9),13,13,14,14,9,9,11,11,6260 ),// 
            new MarketData(new DateTime(1,1,10),14,14,16,16,13,13,15,15,1 ),
        };

    }
}
