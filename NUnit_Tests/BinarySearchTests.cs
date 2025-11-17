using Algorithms_DataStruct_Lib;
using NUnit.Framework;

namespace Algorithms.DataStruct.Lib.Tests
{
    [TestFixture]
    public class BinarySearchTests
    {
        [Test]
        public void BinarySearch_SortedInput_ReturnsCorrectIndex()
        {
            int[] input = {0, 3, 4, 7, 8, 12, 15, 22};

            const int notFound = -1;
            Assert.That(Searching.BinarySearch(input, 10), Is.EqualTo(notFound));
            Assert.That(Searching.BinarySearch(input, 4), Is.EqualTo(2));
            Assert.That(Searching.BinarySearch(input, 8), Is.EqualTo(4));
            Assert.That(Searching.BinarySearch(input, 15), Is.EqualTo(6));
            Assert.That(Searching.BinarySearch(input, 22), Is.EqualTo(7));

            Assert.That(Searching.RecursiveBinarySearch(input, 10), Is.EqualTo(notFound));
            Assert.That(Searching.RecursiveBinarySearch(input, 4), Is.EqualTo(2));
            Assert.That(Searching.RecursiveBinarySearch(input, 8), Is.EqualTo(4));
            Assert.That(Searching.RecursiveBinarySearch(input, 15), Is.EqualTo(6));
            Assert.That(Searching.RecursiveBinarySearch(input, 22), Is.EqualTo(7));
        }
    }
}