using System.Collections.Generic;
using Algorithms_DataStruct_Lib.SymbolTables;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using NUnit.Framework.Internal;

namespace Algorithms.DataStruct.Lib.Tests.SymbolTables
{
    [TestFixture]
    public class BinarySearchStTests
    {
        private BinarySearchSt<string, int> _bst;

        [SetUp]
        public void Init()
        {
            _bst = new BinarySearchSt<string, int>(16);
        }

        [Test]
        public void ContainsShouldReturnFalseIfItemWasNotFoundAndTrueIfFound()
        {
            _bst.Add("a", 1);
            
            ClassicAssert.IsTrue(_bst.Contains("a"));
            ClassicAssert.IsFalse(_bst.Contains("b"));

            _bst.Add("b", 1);
            _bst.Add("c", 1);

            ClassicAssert.IsTrue(_bst.Contains("b"));
            ClassicAssert.IsTrue(_bst.Contains("c"));
        }

        [Test]
        public void GetShouldReturnCorrectValue()
        {
            ClassicAssert.AreEqual(default(int), _bst.GetValueOrDefault("a"));

            _bst.Add("a", 1);
            _bst.Add("c", 3);
            _bst.Add("b", 2);

            ClassicAssert.AreEqual(1, _bst.GetValueOrDefault("a"));
            ClassicAssert.AreEqual(2, _bst.GetValueOrDefault("b"));
            ClassicAssert.AreEqual(3, _bst.GetValueOrDefault("c"));

            ClassicAssert.AreEqual(default(int), _bst.GetValueOrDefault("d"));
        }

        [Test]
        public void PutUpdatingValue_ShouldUpdateCorrectly()
        {
            _bst.Add("a", 1);
            ClassicAssert.AreEqual(1, _bst.GetValueOrDefault("a"));

            _bst.Add("a", 2);
            ClassicAssert.AreEqual(2, _bst.GetValueOrDefault("a"));
        }

        [Test]
        public void IterateOver_SeveralItems_ReturnsCorrectSortedSequence()
        {
            _bst = new BinarySearchSt<string, int>(3);
            _bst.Add("a", 1);
            _bst.Add("c", 3);
            _bst.Add("b", 2);

            var expected = new List<string> {"a", "b", "c"};

            CollectionAssert.AreEqual(expected, _bst.Keys());
        }


        [Test]
        public void MinMax_SeveralItems_ReturnsMinAndMax()
        {
            _bst.Add("a", 1);
            _bst.Add("c", 3);
            _bst.Add("b", 2);

            ClassicAssert.AreEqual("a", _bst.Min());
            ClassicAssert.AreEqual("c", _bst.Max());
        }

        [Test]
        public void RankReturnsCorrectIndex()
        {
            _bst.Add("a", 1);
            _bst.Add("c", 3);
            _bst.Add("b", 2);
            _bst.Add("d", 4);

            ClassicAssert.AreEqual(0, _bst.Rank("a"));
            ClassicAssert.AreEqual(3, _bst.Rank("d"));
        }

        [Test]
        public void CeilingAndFloor()
        {
            _bst = new BinarySearchSt<string, int>(4);

            _bst.Add("b", 1);
            _bst.Add("c", 2);
            _bst.Add("d", 3);
            _bst.Add("e", 4);

            ClassicAssert.AreEqual("b", _bst.Ceiling("b"));
            ClassicAssert.AreEqual("b", _bst.Ceiling("a"));
            ClassicAssert.AreEqual(null, _bst.Ceiling("f"));

            ClassicAssert.AreEqual("e", _bst.Floor("e"));

            ClassicAssert.AreEqual(null, _bst.Floor("a"));

            ClassicAssert.AreEqual("e", _bst.Floor("f"));
        }

        [Test]
        public void Add_TooManyItems_CapacityGrows()
        {
            _bst = new BinarySearchSt<string, int>(4);

            _bst.Add("b", 1);
            _bst.Add("c", 2);
            _bst.Add("d", 3);
            _bst.Add("e", 4);
            _bst.Add("f", 5);

            ClassicAssert.AreEqual(8, _bst.Capacity);
        }

        [Test]
        public void Remove_TooManyItems_CapacityGrows()
        {
            _bst = new BinarySearchSt<string, int>(4);

            _bst.Add("b", 1);
            _bst.Add("c", 2);
            _bst.Add("d", 3);
            _bst.Add("e", 4);
            _bst.Add("f", 5);

            _bst.Remove("d");
        }

        [Test]
        public void Range()
        {
            _bst.Add("b", 1);
            _bst.Add("c", 2);
            _bst.Add("d", 3);
            _bst.Add("e", 4);
            _bst.Add("f", 5);

            var range = _bst.Range("a", "g");

            CollectionAssert.AreEqual(new List<string>()
            {
                "b", "c", "d", "e", "f"
            }, range);
        }
    }
}
