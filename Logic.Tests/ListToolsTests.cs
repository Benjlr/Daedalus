using Logic.Utils;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
