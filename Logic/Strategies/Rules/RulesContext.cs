namespace Logic.Strategies.Rules
{
    public class RulesContext
    {
        //public static double MaxSpread = 3;
        //public static DateTime ExitActionition => new DateTime(1,1,1,5,0,0);

        //private static List<Tuple<DateTime, DateTime>> okToTrade { get; set; } = new List<Tuple<DateTime, DateTime>>()
        //{
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 0, 0, 0), new DateTime(01, 01, 01, 0, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 0, 30, 0), new DateTime(01, 01, 01, 1, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 1, 0, 0), new DateTime(01, 01, 01, 1, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 1, 30, 0), new DateTime(01, 01, 01, 2, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 2, 0, 0), new DateTime(01, 01, 01, 2, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 2, 30, 0), new DateTime(01, 01, 01, 3, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 3, 0, 0), new DateTime(01, 01, 01, 3, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 3, 30, 0), new DateTime(01, 01, 01, 4, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 4, 0, 0), new DateTime(01, 01, 01, 4, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 4, 30, 0), new DateTime(01, 01, 01, 5, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 5, 0, 0), new DateTime(01, 01, 01, 5, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 5, 30, 0), new DateTime(01, 01, 01, 6, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 6, 0, 0), new DateTime(01, 01, 01, 6, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 6, 30, 0), new DateTime(01, 01, 01, 7, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 7, 0, 0), new DateTime(01, 01, 01, 7, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 7, 30, 0), new DateTime(01, 01, 01, 8, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 8, 0, 0), new DateTime(01, 01, 01, 8, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 8, 30, 0), new DateTime(01, 01, 01, 9, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 9, 0, 0), new DateTime(01, 01, 01, 9, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 9, 30, 0), new DateTime(01, 01, 01, 10, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 10, 0, 0), new DateTime(01, 01, 01, 10, 20, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 10, 20, 0), new DateTime(01, 01, 01, 10, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 10, 30, 0), new DateTime(01, 01, 01, 11, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 11, 0, 0), new DateTime(01, 01, 01, 11, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 11, 30, 0), new DateTime(01, 01, 01, 12, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 12, 0, 0), new DateTime(01, 01, 01, 12, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 12, 30, 0), new DateTime(01, 01, 01, 13, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 13, 0, 0), new DateTime(01, 01, 01, 13, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 13, 30, 0), new DateTime(01, 01, 01, 14, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 14, 0, 0), new DateTime(01, 01, 01, 14, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 14, 30, 0), new DateTime(01, 01, 01, 15, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 15, 0, 0), new DateTime(01, 01, 01, 15, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 15, 30, 0), new DateTime(01, 01, 01, 15, 40, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 15, 40, 0), new DateTime(01, 01, 01, 16, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 16, 0, 0), new DateTime(01, 01, 01, 16, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 16, 30, 0), new DateTime(01, 01, 01, 17, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 17, 0, 0), new DateTime(01, 01, 01, 17, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 17, 30, 0), new DateTime(01, 01, 01, 18, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 18, 0, 0), new DateTime(01, 01, 01, 18, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 18, 30, 0), new DateTime(01, 01, 01, 19, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 19, 0, 0), new DateTime(01, 01, 01, 19, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 19, 30, 0), new DateTime(01, 01, 01, 20, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 20, 0, 0), new DateTime(01, 01, 01, 20, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 20, 30, 0), new DateTime(01, 01, 01, 21, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 21, 0, 0), new DateTime(01, 01, 01, 21, 30, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 21, 30, 0), new DateTime(01, 01, 01, 22, 0, 0)),
        //    new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 22, 0, 0), new DateTime(01, 01, 01, 22, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 22, 30, 0), new DateTime(01, 01, 01, 23, 0, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 23, 0, 0), new DateTime(01, 01, 01, 23, 30, 0)),
        //    //new Tuple<DateTime, DateTime>(new DateTime(01, 01, 01, 23, 30, 0), new DateTime(01, 01, 01, 0, 0, 0)),
        //};

        //private static List<Session> _broaderMarket { get; set; }
        //public static void InitBroaderMarketContext(List<Session> input)
        //{
        //    _broaderMarket = SessionCollate.CollateToDaily(input);

        //}

        //public static bool IsValid(MarketData entryPoint)
        //{
        //    var timeCheck = okToTrade.Any(x => entryPoint.Time.TimeOfDay > x.Item1.TimeOfDay && entryPoint.Time.TimeOfDay < x.Item2.TimeOfDay);
        //    var maxSpread = (entryPoint.Close_Ask - entryPoint.Close_Bid) <= MaxSpread;

        //    return  maxSpread ;
        //}

        //public static bool CloseActionitions(MarketData exitPoint) => (exitPoint.Time.DayOfWeek == DayOfWeek.Saturday && exitPoint.Time.Hour > ExitActionition.Hour);
        //public static bool CloseActionitions(MarketData exitPoint) => false;
    }
}
