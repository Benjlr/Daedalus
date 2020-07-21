using System.Collections.Generic;
using System.Linq;
using LinqStatistics;
using Logic.Rules;

namespace Logic
{
    public class Strategy
    {
        public bool[] Entries { get; }
        public bool[] Exits { get; }

        public int[] Durations { get; }

        public List<Bin> LongTimeDistribution => Durations.Where(x=>x!=0).Histogram(40).ToList();

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
