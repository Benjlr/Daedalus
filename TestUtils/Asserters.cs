using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
