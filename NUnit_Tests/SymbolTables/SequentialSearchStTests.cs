using System;
using System.Collections.Generic;
using Algorithms_DataStruct_Lib.SymbolTables;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Algorithms.DataStruct.Lib.Tests.SymbolTables
{
    [TestFixture]
    public class SequentialSearchStTests
    {
        private SequentialSearchSt<string, int> _st;

        [SetUp]
        public void Init()
        {
            _st = new SequentialSearchSt<string, int>();
        }

        [Test]
        public void Count_PutSingleItem_ReturnsOne()
        {
            _st.Add("a", 1);

            ClassicAssert.AreEqual(1, _st.Count);            
        }

        [Test]
        public void Add_PassNullAsKey_Throws()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => _st.Add(null, 0));            
        }

        [Test]
        public void Remove_PassNullAsKey_Throws()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => _st.Remove(null));
        }

        [Test]
        public void Ctor_PassNullAsComparer_Throws()
        {
            ClassicAssert.Throws<ArgumentNullException>(() =>
            {
                new SequentialSearchSt<string, int>(null);
            });
        }

        [Test]
        public void Contains_PutSingeItem_ContainsThatItem()
        {
            _st.Add("a", 1);

            ClassicAssert.IsTrue(_st.Contains("a"));
            ClassicAssert.IsFalse(_st.Contains("b"));
        }


        [Test]
        public void Get_PutSingleItem_ReturnsTrueAndValue()
        {
            _st.Add("a", 1);

            bool exists = _st.TryGet("a", out int result);

            ClassicAssert.AreEqual(1, result);
            ClassicAssert.IsTrue(exists);
        }

        [Test]
        public void GetByInexistentKey_PutSingleItem_ReturnsFalseAndDefaultValue()
        {
            _st.Add("a", 1);

            bool exists = _st.TryGet("b", out int result);

            ClassicAssert.AreEqual(default(int), result);
            ClassicAssert.IsFalse(exists);
        }

        [Test]
        public void UpdateValueByKey_ValueGetsRewritten()
        {
            _st.Add("a", 1);
            _st.Add("a", 2);

            _st.TryGet("a", out int result);

            ClassicAssert.AreEqual(2, result);
        }

        [Test]
        public void Remove_AddOneOrSeveralItems_CorrectState()
        {
            _st.Add("a", 1);
            ClassicAssert.IsFalse(_st.Remove("b"));

            _st.Remove("a");
            ClassicAssert.AreEqual(0, _st.Count);

            _st.Add("a", 1);
            _st.Add("b", 2);

            _st.Remove("a");
            ClassicAssert.AreEqual(1, _st.Count);

            _st.TryGet("b", out int result);
            ClassicAssert.AreEqual(2, result);

            _st.Add("a", 1);
            _st.Remove("a");

            ClassicAssert.AreEqual(1, _st.Count);

            _st.TryGet("b", out result);
            ClassicAssert.AreEqual(2, result);
        }

        [Test]
        public void Keys_SeveralKeys_CorrectSequence()
        {
            _st.Add("a", 1);
            _st.Add("b", 2);
            _st.Add("c", 3);

            var expected = new List<string>{"c", "b", "a"};
            ClassicAssert.AreEqual(expected, _st.Keys());
        }
    }
}