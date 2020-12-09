using DataStructures;
using System;
using System.Collections.Generic;

namespace TestUtils
{
    public class TradeFlatteningData
    {
        private static DateTime StartDate = new DateTime(2020, 01, 01);
        private static DateTime StartDateShorter = new DateTime(2020, 01, 10);
        private static DateTime StartDateShortest = new DateTime(2020, 01, 15);

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

        public static SessionData[] longMarket = new SessionData[]
        {
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

        public static SessionData[] mediumMarket = new SessionData[]
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

        public static SessionData[] shortMarket = new SessionData[]
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


        public static List<Trade> longMarketTrades = new List<Trade>()
        {
            new Trade(new double[] {0.046, 0.01 , 0.015 , -0.01,  -0.2,  0.15}, 1),
            new Trade(new double[] {0.014, 0.03 , -0.039, 0.004,  0.01,  0.03}, 6),
            new Trade(new double[] {-0.029, 0.05 , 0.008, 0.04,  0.022,  0.034}, 8),
            new Trade(new double[] {0.003, 0.2  , 0.03  ,  -0.1,  0.01,  0.05}, 11),
            new Trade(new double[] {0.008, 0.025, -0.038, -0.02,  0.03, -0.02}, 16),
            new Trade(new double[] {0.025, 0.027, 0.007, -0.038, -0.043, -0.015}, 18),
            new Trade(new double[] {-0.040, 0.01 , 0.04  , 0.005, -0.06, -0.065}, 21),
            new Trade(new double[] {-0.034, 0.036 , -0.019  , -0.039, 0.031, -0.008}, 23),
            new Trade(new double[] {-0.046, 0.012, -0.01 ,  0.01,  0.04,  0.05}, 26),
            new Trade(new double[] {0.007,-0.011,0.017,0.028,0.029,-0.016}, 30),
            new Trade(new double[] {-0.046,0.004,-0.041,-0.043,-0.025,0.044}, 32),
            new Trade(new double[] {-0.012,-0.03,0.002,0.022,-0.014,-0.048}, 35),
            new Trade(new double[] {-0.027,0.048,-0.002}, 38),
        };

        public static double[] longExpectedResults = new[]
        {
            0, 0.046, 0.01, 0.015, -0.01, -0.2, 0.164, 0.03, -0.068, 0.054, 0.018, 0.073, 0.222, 0.064, -0.1, 0.01, 0.058, 0.025, -0.013, 0.007, 0.037, -0.098, -0.033, -0.009, 0.041, -0.079, -0.15, 0.043, -0.018, 0.01,
            0.047, 0.039, -0.029, 0.032, -0.012, -0.071, -0.055, 0.046, -0.005, 0.034, -0.05
        };

        public static List<Trade> mediumMarketTrades = new List<Trade>()
        {
            new Trade(new double[] {-0.004, 0.026, 0.039, 0.044, 0.01, -0.03}, 1),
            new Trade(new double[] {0.034, -0.027, 0.018, 0.029, -0.012, 0.028}, 3),
            new Trade(new double[] {-0.012, 0.016, 0.016, -0.032, -0.046, 0.048}, 6),
            new Trade(new double[] {-0.014, -0.042, -0.012, -0.006, -0.019, 0.006}, 9),
            new Trade(new double[] {0.023, 0.03, 0.016, -0.007, 0.031, -0.042}, 12),
            new Trade(new double[] {-0.049, 0.01, 0.02, -0.021, -0.028}, 15),
            new Trade(new double[] {-0.033, -0.04}, 18)
        };


        public static double[] mediumExpectedResults = new[]
        {
            0, -0.004, 0.026, 0.073, 0.017, 0.028, -0.013, 0.004, 0.044, -0.046, -0.088, 0.036, 0.017, 0.011, 0.022, -0.056, 0.041, -0.022, -0.054, -0.068
        };

        public static List<Trade> shortMarketTrades = new List<Trade>()
        {
            new Trade(new double[] {0.048,0.037,-0.03,0.022,-0.027,0.034}, 0),
            new Trade(new double[] {0.014,0.016,0.03,-0.034,-0.014,0.012}, 1),
            new Trade(new double[] {-0.028,0.046,-0.025,-0.038,-0.032,0.003}, 3),
            new Trade(new double[] {-0.02,-0.034,0.002,0.039,0.042}, 5),
            new Trade(new double[] {-0.032,0.041,0.01,0.033}, 6),
            new Trade(new double[] {0.024,-0.009}, 8),
            new Trade(new double[] {-0.001}, 9),
        };

        public static double[] shortExpectedResults = new[]
        {
            0.048, 0.051, -0.014, 0.024, -0.015, -0.025, -0.092, 0.011, 0.076, 0.065
        };

        public static double[] LongAndShortResults = new[]
        {
            0,0.046,0.01,0.015,-0.01,-0.2,0.164,0.03,-0.068,0.054,0.018,0.073,0.222,0.064,-0.052,0.061,0.044,0.049,-0.028,-0.018,-0.055,-0.087,0.043,0.056,0.041,-0.079,-0.15,0.043,-0.018,0.01,0.047,0.039,-0.029,0.032,-0.012,-0.071,-0.055,0.046,-0.005,0.034,-0.05
        };

        public static double[] LongAndMediumResults = new[]
        {
            0,0.046,0.01,0.015,-0.01,-0.2,0.164,0.03,-0.068,0.054,0.014,0.099,0.295,0.081,-0.072,-0.003,0.062,0.069,-0.059,-0.081,0.073,-0.081,-0.022,0.013,-0.015,-0.038,-0.172,-0.011,-0.086,0.01,0.047,0.039,-0.029,0.032,-0.012,-0.071,-0.055,0.046,-0.005,0.034,-0.05
        };

        public static double[] LongShortAndMediumResults = new[]
        {
            0,0.046,0.01,0.015,-0.01,-0.2,0.164,0.03,-0.068,0.054,0.014,0.099,0.295,0.081,-0.024,0.048,0.048,0.093,-0.074,-0.106,-0.019,-0.07,0.054,0.078,-0.015,-0.038,-0.172,-0.011,-0.086,0.01,0.047,0.039,-0.029,0.032,-0.012,-0.071,-0.055,0.046,-0.005,0.034,-0.05
        };
        public static double[] ShortAndMediumResults = new[]
        {
            0,-0.004,0.026,0.073,0.017,0.076,0.038,-0.01,0.068,-0.061,-0.113,-0.056,0.028,0.087,0.087,-0.056,0.041,-0.022,-0.054,-0.068
        };

    }
}

