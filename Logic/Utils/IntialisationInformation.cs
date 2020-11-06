using System.Collections.Generic;
using System.Linq;

namespace Logic.Utils
{
    public class IntialisationInformation
    {
        public static string CurrentStage { get; set; } = "HI";
        public static int StageCount { get;  set; }
        public static int TotalCount { get;  set; }

        private static List<System.Action> _subscribers { get; set; } = new List<System.Action>();

        public static void Subscribe(System.Action a) => _subscribers.Add(a);

        public static void IncreaseCount()
        {
            StageCount++;
            _subscribers.ForEach(x => x.Invoke());
        }

        public static void ChangeTotalCount(int count)
        {
            TotalCount = count;
            _subscribers.ForEach(x => x.Invoke());
        }

        public static void ChangeStage(string info)
        {
            CurrentStage = info;
            StageCount = 0;
            _subscribers.ForEach(x => x.Invoke());
        }
    }
}
