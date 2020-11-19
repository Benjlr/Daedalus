using System;

namespace DataStructures
{
    public readonly struct SessionData
    {
        public SessionData(
            DateTime od,
            DateTime hd,
            DateTime ld,
            DateTime cd,
            double v,
            double o,
            double h,
            double l,
            double c
        ) {
            OpenDate = od;
            HighDate = hd;
            LowDate = ld;
            CloseDate = cd;
            Volume = v;
            Open = o;
            High = h;
            Low = l;
            Close = c;
        }

        public SessionData(
            DateTime cd,
            double v,
            double o,
            double h,
            double l,
            double c
        ) {
            OpenDate = cd;
            HighDate = cd;
            LowDate = cd;
            CloseDate = cd;
            Volume = v;
            Open = o;
            High = h;
            Low = l;
            Close = c;
        }

        public DateTime OpenDate { get; }
        public DateTime HighDate { get; }
        public DateTime LowDate { get; }
        public DateTime CloseDate { get; }

        public double Volume { get; }
        public double Open { get; }
        public double High { get; }
        public double Low { get; }
        public double Close { get; }

    }
}
