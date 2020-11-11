using Logic.Utils;
using System.Collections.Generic;
using Xunit;

namespace Logic.Tests
{
    public class ListToolsTests
    {

        [Fact]
        private void ShouldReturnCorrectlyIndexedList()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7 };
            var expected = new List<double>() { 0.5, 2.3, 2.0, 1.0 };
            var calcResult = ListTools.GetNewListByIndex(list, 3, 6);

            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnCorrectlyIndexedListDespiteIndexOverflow()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7 };
            var expected = new List<double>() { 1.0, 1.6, 1.7 };
            var calcResult = ListTools.GetNewListByIndex(list, 6, 12);

            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnListFromStartIndexAndCountWithCorrectNumberOfElements()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new List<double>() { 1.0, 0.5, 2.3, 2.0 };
            var calcResult = ListTools.GetNewListByStartIndexAndCount(list, 2, 4);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnListFromStartIndexAndCountWithCorrectNumberOfElementsDespiteOverflow()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new List<double>() { 1.0, 1.6, 1.7, 3.6, 9.1 };
            var calcResult = ListTools.GetNewListByStartIndexAndCount(list, 6, 12);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnCorrectlyIndexedArray()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7 };
            var expected = new double[] { 0.5, 2.3, 2.0, 1.0 };
            var calcResult = ListTools.GetNewArrayByIndex(list, 3, 6);
            Assert.Equal(expected, calcResult);

        }

        [Fact]
        private void ShouldReturnCorrectlyIndexedArrayDespiteIndexOverflow()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7 };
            var expected = new double[] { 1.0, 1.6, 1.7 };
            var calcResult = ListTools.GetNewArrayByIndex(list, 6, 12);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnArrayFromStartIndexAndCountWithCorrectNumberOfElements()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new double[] { 1.0, 0.5, 2.3, 2.0 };
            var calcResult = ListTools.GetNewArrayByStartIndexAndCount(list, 2, 4);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnArrayFromStartIndexAndCountWithCorrectNumberOfElementsDespiteOverflow()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new double[] { 1.0, 1.6, 1.7, 3.6, 9.1 };
            var calcResult = ListTools.GetNewArrayByStartIndexAndCount(list, 6, 12);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnArrayFromEndIndexAndCountWithCorrectNumberOfElements()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new double[] { 0.5, 2.3, 2.0, 1.0, 1.6 };
            var calcResult = ListTools.GetNewArrayByEndIndexAndCount(list, 7, 5);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnArrayFromEndIndexAndCountWithCorrectNumberOfElementsDespiteOverflow()
        {
            var list = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new double[] { 0.2, 0.3, 1.0, 0.5, 2.3 };
            var calcResult = ListTools.GetNewArrayByEndIndexAndCount(list, 4, 80);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnListFromEndIndexAndCountWithCorrectNumberOfElements()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new List<double>() { 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6 };
            var calcResult = ListTools.GetNewListByEndIndexAndCount(list, 9, 8);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnListFromEndIndexAndCountWithCorrectNumberOfElementsDespiteOverflow()
        {
            var list = new List<double>() { 0.2, 0.3, 1.0, 0.5, 2.3, 2.0, 1.0, 1.6, 1.7, 3.6, 9.1 };
            var expected = new List<double>() { 0.2, 0.3, 1.0 };
            var calcResult = ListTools.GetNewListByEndIndexAndCount(list, 2, 423);
            Assert.Equal(expected, calcResult);
        }

        [Fact]
        private void ShouldReturnFirstIndexAfterNnonZeroElements()
        {
            var list = new List<double>() { 0, 0, 1.2, 0, 66, 0, 0, 0, 5.3, -0.1, -0.00003, 0, 0, 0.05 };
            var listWo = new List<double>() { 2, 1, 1.2, 0, -56, 0, 0, 0, 5.3, -0.1, -.4, 0 };
            var allZeroes = new List<double>() { 0,0,0,0,0,0,0 };
            Assert.Equal(10, ListTools.GetIndexAtThresholdNonZeroes(5, list));
            Assert.Equal(1, ListTools.GetIndexAtThresholdNonZeroes(2, listWo));
            Assert.Equal(7, ListTools.GetIndexAtThresholdNonZeroes(1, allZeroes));
        }

        [Fact]
        private void ShouldReturnListOfNnonZeroValues()
        {
            var list = new List<double>() { 0, 0, 1.2, 0, 66, 0, 0, 0, 5.3, -0.1, -0.00003, 0, 1, 0, 0.05 };

            Assert.Equal(new List<double>() {66, 5.3,-0.1 } , ListTools.GetLastNnonZeroValues(3, 9, list));
            Assert.Equal(new List<double>() { 1.2,66,5.3,-0.1,-0.00003,1 }, ListTools.GetLastNnonZeroValues(6, 12, list));
            Assert.Null(ListTools.GetLastNnonZeroValues(3, 7, list));
            Assert.Null(ListTools.GetLastNnonZeroValues(3, 2, list));
        }
    }
}
