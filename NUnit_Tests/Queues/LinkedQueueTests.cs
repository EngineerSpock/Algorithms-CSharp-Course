using System;
using System.Collections.Generic;
using Algorithms_DataStruct_Lib.Queues;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Algorithms.DataStruct.Lib.Tests.Queues
{
    [TestFixture]
    public class LinkedQueueTests
    {

        [Test]
        public void IsEmpty_EmptyQueue_ReturnsTrue()
        {
            var queue = new LinkedQueue<int>();
            ClassicAssert.IsTrue(queue.IsEmpty);
        }

        [Test]
        public void Peek_EmptyQueue_ThrowsException()
        {
            var queue = new LinkedQueue<int>();

            ClassicAssert.Throws<InvalidOperationException>(() =>
            {
                queue.Peek();
            });
        }

        [Test]
        public void IterateOver_SeveralItems_ExpectedSequence()
        {
            var queue = new LinkedQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var q = new List<int>();

            foreach (var cur in queue)
                q.Add(cur);

            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, q);
        }


        [Test]
        public void Count_EnqueueOneItem_ReturnsOne()
        {
            var queue = new LinkedQueue<int>();
            queue.Enqueue(1);

            ClassicAssert.AreEqual(1, queue.Count);
            ClassicAssert.IsFalse(queue.IsEmpty);
        }

        [Test]
        public void Dequeue_EmptyQueue_ThrowsException()
        {
            var queue = new LinkedQueue<int>();

            ClassicAssert.Throws<InvalidOperationException>(() =>
            {
                queue.Dequeue();
            });
        }

        [Test]
        public void Peek_EnqueueTwoItems_ReturnsHeadItem()
        {
            var queue = new LinkedQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            ClassicAssert.AreEqual(1, queue.Peek());
        }

        [Test]
        public void Peek_EnqueueTwoItemsAndDequeue_ReturnsHeadElement()
        {
            var queue = new LinkedQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Dequeue();

            ClassicAssert.AreEqual(2, queue.Peek());
        }

    }
}