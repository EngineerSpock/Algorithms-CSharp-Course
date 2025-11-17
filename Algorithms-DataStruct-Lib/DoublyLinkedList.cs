using System;
using System.Collections;
using System.Collections.Generic;

namespace Algorithms_DataStruct_Lib
{
    public class DoublyLinkedList<T> : IEnumerable<T>
    {
        public DoublyLinkedNode<T> Head { get; private set; }
        public DoublyLinkedNode<T> Tail { get; private set; }

        public void AddFirst(T value)
        {
            AddFirst(new DoublyLinkedNode<T>(value));
        }

        private void AddFirst(DoublyLinkedNode<T> node)
        {
            //save off the Head
            DoublyLinkedNode<T> temp = Head;
            //point Head to node
            Head = node;

            //insert the rest of the list after the head
            Head.Next = temp;

            if (IsEmpty)
            {
                Tail = Head;
            }
            else
            {
                //before: 1(head) <-------> 5 <-> 7 -> null
                //after:  3(head) <-------> 1 <-> 5 <-> 7 -> null

                //update "previous" ref of the former head
                temp.Previous = Head;
            }

            Count++;
        }

        public void AddLast(T value)
        {
            AddLast(new DoublyLinkedNode<T>(value));
        }

        private void AddLast(DoublyLinkedNode<T> node)
        {
            if (IsEmpty)
                Head = node;
            else
            {
                Tail.Next = node;
                node.Previous = Tail;
            }
            Tail = node;
            Count++;
        }

        public void RemoveFirst()
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            //shift head
            Head = Head.Next;

            Count--;

            if (IsEmpty)
                Tail = null;
            else
                Head.Previous = null;
        }

        public void RemoveLast()
        {
            if (IsEmpty)
                throw new InvalidOperationException();

            if (Count == 1)
            {
                Head = null;
                Tail = null;
            }
            else
            {
                Tail.Previous.Next = null; //null the last node
                Tail = Tail.Previous; //shift the Tail (now it is the former penultimate node)
            }

            Count--;
        }

        public IEnumerator<T> GetEnumerator()
        {
            DoublyLinkedNode<T> current = Head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get; private set; }
        public bool IsEmpty => Count == 0;
    }
}