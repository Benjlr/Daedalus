using Logic.Rules;

namespace Logic
{
    public class Strategy
    {
        public bool[] Entries { get; }
        public bool[] Exits { get; }

        public int[] Durations { get; }

        public IRuleSet[] Rules { get; }

        public Strategy(IRuleSet[] rules, bool[] entries, bool[] exits, int[] durations)
        {
            Entries = entries;
            Exits = exits;
            Rules = rules;
            Durations = durations;
        }

    }
}
