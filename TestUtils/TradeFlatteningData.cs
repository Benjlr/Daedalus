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

        public static SessionData[] longMarket = new SessionData[]
        {
            new SessionData(StartDate, 200, 10, 10, 10, 10), //0
            new SessionData(IncStart(), 200, 11, 11, 11, 11),
            new SessionData(IncStart(), 200, 12, 12, 12, 12),
            new SessionData(IncStart(), 200, 13, 13, 13, 13),
            new SessionData(IncStart(), 200, 14, 14, 14, 14),
            new SessionData(IncStart(), 200, 15, 15, 15, 15),//5
            new SessionData(IncStart(), 200, 16, 16, 16, 16),
            new SessionData(IncStart(), 200, 17, 17, 17, 17),
            new SessionData(IncStart(), 200, 18, 18, 18, 18),
            new SessionData(IncStart(), 200, 19, 19, 19, 19),
            new SessionData(IncStart(), 200, 20, 20, 20, 20),//10
            new SessionData(IncStart(), 200, 21, 21, 21, 21),
            new SessionData(IncStart(), 200, 22, 22, 22, 22),
            new SessionData(IncStart(), 200, 23, 23, 23, 23),
            new SessionData(IncStart(), 200, 24, 24, 24, 24),
            new SessionData(IncStart(), 200, 25, 25, 25, 25),//15
            new SessionData(IncStart(), 200, 26, 26, 26, 26),
            new SessionData(IncStart(), 200, 27, 27, 27, 27),
            new SessionData(IncStart(), 200, 28, 28, 28, 28),
            new SessionData(IncStart(), 200, 29, 29, 29, 29),
            new SessionData(IncStart(), 200, 30, 30, 30, 30),//20
            new SessionData(IncStart(), 200, 31, 31, 31, 31),
            new SessionData(IncStart(), 200, 32, 32, 32, 32),
            new SessionData(IncStart(), 200, 33, 33, 33, 33),
            new SessionData(IncStart(), 200, 34, 34, 34, 34),
            new SessionData(IncStart(), 200, 35, 35, 35, 35),//25
            new SessionData(IncStart(), 200, 36, 36, 36, 36),
            new SessionData(IncStart(), 200, 37, 37, 37, 37),
            new SessionData(IncStart(), 200, 38, 38, 38, 38),
            new SessionData(IncStart(), 200, 39, 39, 39, 39),
            new SessionData(IncStart(), 200, 40, 40, 40, 40),//30
            new SessionData(IncStart(), 200, 41, 41, 41, 41),
            new SessionData(IncStart(), 200, 42, 42, 42, 42),
            new SessionData(IncStart(), 200, 43, 43, 43, 43),
            new SessionData(IncStart(), 200, 44, 44, 44, 44),
            new SessionData(IncStart(), 200, 45, 45, 45, 45),//35
            new SessionData(IncStart(), 200, 46, 46, 46, 46),
            new SessionData(IncStart(), 200, 47, 47, 47, 47),
            new SessionData(IncStart(), 200, 48, 48, 48, 48),
            new SessionData(IncStart(), 200, 49, 49, 49, 49),
            new SessionData(IncStart(), 200, 50, 50, 50, 50) //40
        };

        public static List<Trade> 

        public static SessionData[] shorterMarket = new SessionData[]
        {
            new SessionData(StartDateShorter, 200, 10, 10, 10, 10),//0
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

        
    }
}
