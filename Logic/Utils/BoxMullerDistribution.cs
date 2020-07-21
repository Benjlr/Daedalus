using System;

namespace Logic.Utils
{
    public class BoxMullerDistribution
    {
        private static Random _rand;


        public static double Generate(double mean, double standardDeviation)
        {
            if(_rand == null) _rand = new Random();
            double uOne = 1.0 - _rand.NextDouble();
            double uTwo = 1.0 - _rand.NextDouble();

            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(uOne)) * Math.Sin(2.0 * Math.PI * uTwo);
            return mean + standardDeviation * randStdNormal;
        }


    }
}
