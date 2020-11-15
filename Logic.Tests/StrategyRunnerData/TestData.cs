using RuleSets;
using System;
using PriceSeriesCore;

namespace Logic.Tests.StrategyRunnerData
{
    public class TestData
    {
        public MarketData data = new MarketData(time: new DateTime(),
            o_a: 1000,
            o_b: 1000,
            h_a: 1001,
            h_b: 1001,
            l_a: 999,
            l_b: 999,
            c_a: 1000,
            c_b: 1000,
            vol: 45);

        public MarketData data2 = new MarketData(time: new DateTime(),
            o_a: 1002,
            o_b: 1002,
            h_a: 1004,
            h_b: 1004,
            l_a: 999,
            l_b: 999,
            c_a: 1002,
            c_b: 1002,
            vol: 29);

        public MarketData data3 = new MarketData(time: new DateTime(),
            o_a: 1004,
            o_b: 1004,
            h_a: 1010,
            h_b: 1010,
            l_a: 1004,
            l_b: 1004,
            c_a: 1006,
            c_b: 1006,
            vol: 68);

        public MarketData data4 = new MarketData(time: new DateTime(),
            o_a: 998,
            o_b: 998,
            h_a: 1001,
            h_b: 1001,
            l_a: 994,
            l_b: 994,
            c_a: 998,
            c_b: 998,
            vol: 635);

        public MarketData data5 = new MarketData(time: new DateTime(),
            o_a: 995,
            o_b: 995,
            h_a: 996,
            h_b: 996,
            l_a: 990,
            l_b: 990,
            c_a: 995,
            c_b: 995,
            vol: 29);

        public MarketData data6 = new MarketData(time: new DateTime(),
            o_a: 1010,
            o_b: 1010,
            h_a: 1012,
            h_b: 1012,
            l_a: 1009,
            l_b: 1009,
            c_a: 1010,
            c_b: 1010,
            vol: 29);

        public MarketData data7 = new MarketData(time: new DateTime(),
            o_a: 980,
            o_b: 980,
            h_a: 982,
            h_b: 982,
            l_a: 978,
            l_b: 978,
            c_a: 980,
            c_b: 980,
            vol: 29);


        public MarketData data8 = new MarketData(time: new DateTime(),
            o_a: 1004,
            o_b: 1004,
            h_a: 1006,
            h_b: 1006,
            l_a: 1000,
            l_b: 1000,
            c_a: 1002,
            c_b: 1002,
            vol: 56);
    }

}