using System;
using System.Collections.Generic;
using Algorithms_DataStruct_Lib;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Algorithms.DataStruct.Lib.Tests
{
    [TestFixture]
    public class DoublyLinkedListTests
    {
        private DoublyLinkedList<int> _list;

        [SetUp]
        public void Init()
        {
            _list = new DoublyLinkedList<int>();
        }

        [Test]
        public void CreateEmptyList_CorrectState()
        {
            ClassicAssert.IsTrue(_list.IsEmpty);
            ClassicAssert.IsNull(_list.Head);
            ClassicAssert.IsNull(_list.Tail);
        }

        [Test]
        public void AddFirst_and_AddLast_OneItem_CorrectState()
        {
            _list.AddFirst(1);

            CheckStateWithSingleNode(_list);

            _list.RemoveFirst();
            _list.AddLast(1);

            CheckStateWithSingleNode(_list);
        }

        [Test]
        public void AddRemoveToGetToStateWithSingleNode_CorrectState()
        {
            _list.AddFirst(1);
            _list.AddFirst(2);
            _list.RemoveFirst();

            CheckStateWithSingleNode(_list);

            _list.AddFirst(2);
            _list.RemoveLast();

            CheckStateWithSingleNode(_list);
        }


        private void CheckStateWithSingleNode(DoublyLinkedList<int> list)
        {
            ClassicAssert.AreEqual(1, list.Count);
            ClassicAssert.IsFalse(list.IsEmpty);
            ClassicAssert.AreSame(list.Head, list.Tail);
        }

        [Test]
        public void AddFirst_and_AddLast_AddItemsInCorrectOrder()
        {
            _list.AddFirst(1);
            _list.AddFirst(2);

            ClassicAssert.AreEqual(2, _list.Head.Value);
            ClassicAssert.AreEqual(1, _list.Tail.Value);

            _list.AddLast(3);

            ClassicAssert.AreEqual(3, _list.Tail.Value);
        }

        [Test]
        public void RemoveFirst_EmptyList_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _list.RemoveFirst());
        }
        [Test]
        public void RemoveLast_EmptyList_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => _list.RemoveLast());
        }

        [Test]
        public void RemoveLast_SingleElement_HeadTailAreNull()
        {
            _list.AddFirst(1);

            _list.RemoveLast();

            ClassicAssert.AreEqual(0, _list.Count);
            ClassicAssert.IsNull(_list.Head);
            ClassicAssert.IsNull(_list.Tail);
        }

        [Test]
        public void IterateOver_SeveralItems_ExpectedSequence()
        {
            _list.AddLast(1);
            _list.AddLast(2);
            _list.AddLast(3);

            var q = new List<int>();

            foreach (var cur in _list)
                q.Add(cur);

            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, q);
        }

        [Test]
        public void RemoveFirst_RemoveLast_CorrectState()
        {
            _list.AddFirst(1);
            _list.AddFirst(2);
            _list.AddFirst(3);
            _list.AddFirst(4);

            _list.RemoveFirst();
            _list.RemoveLast();

            ClassicAssert.AreEqual(3, _list.Head.Value);
            ClassicAssert.AreEqual(2, _list.Tail.Value);
        }
    }
}