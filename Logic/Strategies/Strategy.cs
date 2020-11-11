
using RuleSets;

namespace Logic
{
    public class Strategy
    {
        public bool[] Entries { get; }
        public bool[] Exits { get; }

        public IRuleSet[] Rules { get; }

        public Strategy(IRuleSet[] rules, bool[] entries, bool[] exits)
        {
            Entries = entries;
            Exits = exits;
            Rules = rules;
        }

    }
}
