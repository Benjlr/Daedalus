using System;
using Microsoft.VisualBasic;

namespace DataStructures
{
    public struct BidAskData
    {
        public BidAsk Open { get; set; }
        public BidAsk High { get; set; }
        public BidAsk Low { get; set; }
        public BidAsk Close { get; set; }
        public double Volume { get; set; }

        public BidAskData(
            BidAsk open,
            BidAsk high,
            BidAsk low,
            BidAsk close,
            double volume) {

            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public BidAskData(DateTime time, double o_a, double o_b, double h_a, double h_b, double l_a, double l_b, double c_a, double c_b, double vol)  {
            Open = new BidAsk(o_b,o_a, time.Ticks);
            High = new BidAsk(h_b, h_a, time.Ticks);
            Low = new BidAsk(l_b, l_a, time.Ticks);
            Close = new BidAsk(c_b, c_a, time.Ticks);
            Volume = vol;
        }

        public BidAskData(DateTime time, int vol, double open, double high, double low, double close ) {
            Open = new BidAsk(open, open, time.Ticks);
            High = new BidAsk(high, high, time.Ticks);
            Low = new BidAsk(low, low, time.Ticks);
            Close = new BidAsk(close, close, time.Ticks);
            Volume = vol;
        }
    }

    public struct BidAsk
    {
        public double Bid { get; set; }
        public double Ask { get; set; }
        public long Ticks { get; set; }

        public double Mid => ((Ask - Bid) / 2.0) + Bid;
        public DateTime TicksToTime => new DateTime(Ticks);

        public BidAsk(double bid, double ask, long date) {
            Bid = bid;
            Ask = ask;
            Ticks = date;
        }
    }


}