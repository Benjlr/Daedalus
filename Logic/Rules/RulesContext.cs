using System;

namespace Logic.Rules
{
    public class RulesContext
    {
        public static double MaxSpread = 3;
        public static DateTime ExitPosition => new DateTime(1,1,1,3,0,0);

        public static bool IsValid(MarketData entryPoint)
        {
            return (entryPoint.Close_Ask - entryPoint.Close_Bid) <= MaxSpread &&
                   !(entryPoint.Time.DayOfWeek == DayOfWeek.Saturday && entryPoint.Time.Hour > ExitPosition.Hour);
        }

        //public static bool ClosePositions(MarketData exitPoint) => (exitPoint.Time.DayOfWeek == DayOfWeek.Saturday && exitPoint.Time.Hour > ExitPosition.Hour);
        public static bool ClosePositions(MarketData exitPoint) =>false;
    }
}
