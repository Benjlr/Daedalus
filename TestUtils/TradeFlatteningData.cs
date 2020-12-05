using DataStructures;
using System;
using System.Collections.Generic;

namespace TestUtils
{
    public class TradeFlatteningData
    {
        private static DateTime StartDate = new DateTime(2020,01,01);
        private static DateTime StartDateShorter = new DateTime(2020,01,10);
        private static DateTime StartDateShortest = new DateTime(2020,01,15);

        private static DateTime IncStart() {
            StartDate = StartDate.AddDays(1);
            return StartDate;
        }

        private static DateTime IncStartDateShorter() {
            StartDateShorter = StartDateShorter.AddDays(1);
            return StartDateShorter;
        }

        private static DateTime IncStartDateShortest() {
            StartDateShortest = StartDateShortest.AddDays(1);
            return StartDateShortest;
        }

        public static List<Trade> longMarketTradesFiveInterval = new List<Trade>()
        {
            new Trade(new double[] {0, 0.1, 0.2, 0.3, 0.4}, 0),
            new Trade(new double[] {0, (16 / 15.0) - 1, (17 / 15.0) - 1, (18 / 15.0) - 1, (19 / 15.0) - 1}, 5),
            new Trade(new double[] {0, (21 / 20.0) - 1, (22 / 20.0) - 1, (23 / 20.0) - 1, (24 / 20.0) - 1}, 10),
            new Trade(new double[] {0, (26 / 25.0) - 1, (27 / 25.0) - 1, (28 / 25.0) - 1, (29 / 25.0) - 1}, 15),
            new Trade(new double[] {0, (31 / 30.0) - 1, (32 / 30.0) - 1, (33 / 30.0) - 1, (34 / 30.0) - 1}, 20),
            new Trade(new double[] {0, (36 / 35.0) - 1, (37 / 35.0) - 1, (38 / 35.0) - 1, (39 / 35.0) - 1}, 25),
        };

        public static List<Trade> longMarketTradesTenInterval = new List<Trade>()
        {
            new Trade(new double[] {0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9}, 0),
            new Trade(
                new double[]
                {
                    0, (21 / 20.0) - 1, (22 / 20.0) - 1, (23 / 20.0) - 1, (24 / 20.0) - 1, (25 / 20.0) - 1,
                    (26 / 20.0) - 1, (27 / 20.0) - 1, (28 / 20.0) - 1, (29 / 20.0) - 1
                }, 10),
            new Trade(
                new double[]
                {
                    0, (31 / 30.0) - 1, (32 / 30.0) - 1, (33 / 30.0) - 1, (34 / 30.0) - 1, (35 / 30.0) - 1,
                    (36 / 30.0) - 1, (37 / 30.0) - 1, (38 / 30.0) - 1, (39 / 30.0) - 1
                }, 20),
            new Trade(
                new double[]
                {
                    0, (41 / 40.0) - 1, (42 / 40.0) - 1, (43 / 40.0) - 1, (44 / 40.0) - 1, (45 / 40.0) - 1,
                    (46 / 40.0) - 1, (47 / 40.0) - 1, (48 / 40.0) - 1, (49 / 40.0) - 1
                }, 30)
        };
        public static SessionData[] longMarket = new SessionData[]
        {
            new SessionData(),
            new SessionData(StartDate, 200, 10, 10, 10, 10), //0    0                   0
            new SessionData(IncStart(), 200, 11, 11, 11, 11),//     0.1                 0.1
            new SessionData(IncStart(), 200, 12, 12, 12, 12),//     0.2                 0.2
            new SessionData(IncStart(), 200, 13, 13, 13, 13),//     0.3                 0.3
            new SessionData(IncStart(), 200, 14, 14, 14, 14),//     0.4                 0.4
            new SessionData(IncStart(), 200, 15, 15, 15, 15),//5        0               0.5
            new SessionData(IncStart(), 200, 16, 16, 16, 16),//         (16/15)-1       0.6
            new SessionData(IncStart(), 200, 17, 17, 17, 17),//         17/15 -1        0.7
            new SessionData(IncStart(), 200, 18, 18, 18, 18),//         18/15 -1        0.8
            new SessionData(IncStart(), 200, 19, 19, 19, 19),//         19/15 -1        0.9
            new SessionData(IncStart(), 200, 20, 20, 20, 20),//10   0                   0
            new SessionData(IncStart(), 200, 21, 21, 21, 21),//     21/20-1             21/20-1
            new SessionData(IncStart(), 200, 22, 22, 22, 22),//     22/20-1             22/20-1
            new SessionData(IncStart(), 200, 23, 23, 23, 23),//     23/20-1             23/20-1
            new SessionData(IncStart(), 200, 24, 24, 24, 24),//     24/20-1             24/20-1
            new SessionData(IncStart(), 200, 25, 25, 25, 25),//15       0               25/20-1
            new SessionData(IncStart(), 200, 26, 26, 26, 26),//         26/25-1         26/20-1
            new SessionData(IncStart(), 200, 27, 27, 27, 27),//         27/25-1         27/20-1
            new SessionData(IncStart(), 200, 28, 28, 28, 28),//         28/25-1         28/20-1
            new SessionData(IncStart(), 200, 29, 29, 29, 29),//         29/25-1         29/20-1
            new SessionData(IncStart(), 200, 30, 30, 30, 30),//20   0                   0
            new SessionData(IncStart(), 200, 31, 31, 31, 31),//     31/30-1             31/30-1
            new SessionData(IncStart(), 200, 32, 32, 32, 32),//     32/30-1             32/30-1
            new SessionData(IncStart(), 200, 33, 33, 33, 33),//     33/30-1             33/30-1
            new SessionData(IncStart(), 200, 34, 34, 34, 34),//     34/30-1             34/30-1
            new SessionData(IncStart(), 200, 35, 35, 35, 35),//25       0               35/30-1
            new SessionData(IncStart(), 200, 36, 36, 36, 36),//         36/35-1         36/30-1
            new SessionData(IncStart(), 200, 37, 37, 37, 37),//         37/35-1         37/30-1
            new SessionData(IncStart(), 200, 38, 38, 38, 38),//         38/35-1         38/30-1
            new SessionData(IncStart(), 200, 39, 39, 39, 39),//         39/35-1         39/30-1
            new SessionData(IncStart(), 200, 40, 40, 40, 40),//30   0                   0
            new SessionData(IncStart(), 200, 41, 41, 41, 41),//     41/40-1             41/40-1
            new SessionData(IncStart(), 200, 42, 42, 42, 42),//     42/40-1             42/40-1
            new SessionData(IncStart(), 200, 43, 43, 43, 43),//     43/40-1             43/40-1
            new SessionData(IncStart(), 200, 44, 44, 44, 44),//     44/40-1             44/40-1
            new SessionData(IncStart(), 200, 45, 45, 45, 45),//35       0               45/40-1
            new SessionData(IncStart(), 200, 46, 46, 46, 46),//         46/45-1         46/40-1
            new SessionData(IncStart(), 200, 47, 47, 47, 47),//         47/45-1         47/40-1
            new SessionData(IncStart(), 200, 48, 48, 48, 48),//         48/45-1         48/40-1
            new SessionData(IncStart(), 200, 49, 49, 49, 49),//         49/45-1         49/40-1
            new SessionData(IncStart(), 200, 50, 50, 50, 50) //40   0                   0
        };

        

        public static SessionData[] shorterMarket = new SessionData[]
        {
            new SessionData(StartDateShorter, 200, 10, 10, 10, 10),     //0
            new SessionData(IncStartDateShorter(), 200, 11, 11, 11, 11),
            new SessionData(IncStartDateShorter(), 200, 12, 12, 12, 12),
            new SessionData(IncStartDateShorter(), 200, 13, 13, 13, 13),
            new SessionData(IncStartDateShorter(), 200, 14, 14, 14, 14),
            new SessionData(IncStartDateShorter(), 200, 15, 15, 15, 15),//5
            new SessionData(IncStartDateShorter(), 200, 16, 16, 16, 16),
            new SessionData(IncStartDateShorter(), 200, 17, 17, 17, 17),
            new SessionData(IncStartDateShorter(), 200, 18, 18, 18, 18),
            new SessionData(IncStartDateShorter(), 200, 19, 19, 19, 19),
            new SessionData(IncStartDateShorter(), 200, 20, 20, 20, 20),//10
            new SessionData(IncStartDateShorter(), 200, 21, 21, 21, 21),
            new SessionData(IncStartDateShorter(), 200, 22, 22, 22, 22),
            new SessionData(IncStartDateShorter(), 200, 23, 23, 23, 23),
            new SessionData(IncStartDateShorter(), 200, 24, 24, 24, 24),
            new SessionData(IncStartDateShorter(), 200, 25, 25, 25, 25),//15
            new SessionData(IncStartDateShorter(), 200, 26, 26, 26, 26),
            new SessionData(IncStartDateShorter(), 200, 27, 27, 27, 27),
            new SessionData(IncStartDateShorter(), 200, 28, 28, 28, 28),
            new SessionData(IncStartDateShorter(), 200, 29, 29, 29, 29)
        };

        public static SessionData[] shortestMarket = new SessionData[]
        {
            new SessionData(StartDateShortest, 200, 10, 10, 10, 10),//0
            new SessionData(IncStartDateShortest(), 200, 11, 11, 11, 11),
            new SessionData(IncStartDateShortest(), 200, 12, 12, 12, 12),
            new SessionData(IncStartDateShortest(), 200, 13, 13, 13, 13),
            new SessionData(IncStartDateShortest(), 200, 14, 14, 14, 14),
            new SessionData(IncStartDateShortest(), 200, 15, 15, 15, 15),//5
            new SessionData(IncStartDateShortest(), 200, 16, 16, 16, 16),
            new SessionData(IncStartDateShortest(), 200, 17, 17, 17, 17),
            new SessionData(IncStartDateShortest(), 200, 18, 18, 18, 18),
            new SessionData(IncStartDateShortest(), 200, 19, 19, 19, 19),
        };

        public static List<Trade> shortestMarketTradesFiveInterval = new List<Trade>()
        {
            new Trade(new double[] {0, 0.1, 0.2, 0.3, 0.4}, 0),
            new Trade(new double[] {0, (16 / 15.0) - 1, (17 / 15.0) - 1, (18 / 15.0) - 1, (19 / 15.0) - 1}, 5),
        };

        public static List<Trade> shortestMarketTradesTenInterval = new List<Trade>()
        {
            new Trade(new double[] {0, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9}, 0),
        };
    }
}
