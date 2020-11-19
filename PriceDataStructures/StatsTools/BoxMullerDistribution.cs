using System;

namespace DataStructures.StatsTools
{
    public class BoxMullerDistribution
    {
        private static readonly Random _rand = new Random();


        public static double Generate(double mean, double standardDeviation) {
            double uOne = 1.0 - _rand.NextDouble();
            double uTwo = 1.0 - _rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(uOne)) * Math.Sin(2.0 * Math.PI * uTwo);
            return mean + standardDeviation * randStdNormal;
        }


    }
}
