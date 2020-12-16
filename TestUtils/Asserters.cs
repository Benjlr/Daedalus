using DataStructures;
using System.Collections.Generic;
using Xunit;

namespace TestUtils
{
    public class Asserters
    {
        public static void ArrayDoublesEqual(double[] expected, double[] actual, int comparer = 6) {
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
                Assert.Equal(expected[i], actual[i], comparer);
        }

        public static void ArrayDoublesEqual(DatedResult[] expected, DatedResult[] actual, int comparer = 6) {
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++) {
                Assert.Equal(expected[i].Return, actual[i].Return, comparer);
                Assert.Equal(expected[i].Drawdown, actual[i].Drawdown, comparer);
                Assert.Equal(expected[i].Date, actual[i].Date);
            }
        }

        public static void ListDoublesEqual(List<double> expected, List<double> actual, int comparer = 6) {
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
                Assert.Equal(expected[i], actual[i], comparer);
        }


        public static void ListListDoubleEquals(List<List<double>> expected, List<List<double>> actual) {
            Assert.Equal(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++) 
                ListDoublesEqual(expected[i], actual[i]);
        }
    }
}
