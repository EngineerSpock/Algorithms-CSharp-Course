using System;
using System.Collections.Generic;
using Algorithms_DataStruct_Lib.Queues;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Algorithms
{
    [TestFixture]
    public class ArrayQueueTests
    {
        [Test]
        public void Capacity_EnqueueManyItems_DoubledCapacity()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);
            queue.Enqueue(5);

            ClassicAssert.AreEqual(8, queue.Capacity);
        }

        [Test]
        public void IsEmpty_EmptyQueue_ReturnsTrue()
        {
            var queue = new ArrayQueue<int>();
            ClassicAssert.IsTrue(queue.IsEmpty);
        }

        [Test]
        public void Ctor_PassCapacity_CorrectCapacity()
        {
            const int capacity = 8;

            var queue = new ArrayQueue<int>(capacity);
            ClassicAssert.AreEqual(capacity, queue.Capacity);
        }

        [Test]
        public void Count_EnqueueOneItem_ReturnsOne()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);

            ClassicAssert.AreEqual(1, queue.Count);
            ClassicAssert.IsFalse(queue.IsEmpty);
        }

        [Test]
        public void Dequeue_EmptyQueue_ThrowsException()
        {
            var queue = new ArrayQueue<int>();

            ClassicAssert.Throws<InvalidOperationException>(() => { queue.Dequeue(); });
        }

        [Test]
        public void Peek_EmptyQueue_ThrowsException()
        {
            var queue = new ArrayQueue<int>();

            ClassicAssert.Throws<InvalidOperationException>(() => { queue.Peek(); });
        }

        [Test]
        public void Dequeue_SingleElement_BecomesEmpty()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);
            queue.Dequeue();

            ClassicAssert.IsTrue(queue.IsEmpty);
        }

        [Test]
        public void Peek_EnqueueTwoItems_ReturnsHeadItem()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            ClassicAssert.AreEqual(1, queue.Peek());
        }

        [Test]
        public void Peek_EnqueueTwoItemsAndDequeue_ReturnsHeadElement()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);

            queue.Dequeue();

            ClassicAssert.AreEqual(2, queue.Peek());
        }

        [Test]
        public void IterateOver_SeveralItems_ExpectedSequence()
        {
            var queue = new ArrayQueue<int>();
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var q = new List<int>();

            foreach (var cur in queue)
                q.Add(cur);

            CollectionAssert.AreEqual(new List<int> { 1, 2, 3 }, q);
        }

        [Test]
        public void Enqueue_AfterRemovingHead_GrowsWithoutLosingElements()
        {
            var queue = new ArrayQueue<int>();

            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.Enqueue(4);

            queue.Dequeue();
            queue.Dequeue();

            queue.Enqueue(5);

            ClassicAssert.AreEqual(3, queue.Count);
            CollectionAssert.AreEqual(new List<int> { 3, 4, 5 }, new List<int>(queue));
        }
    }
}
