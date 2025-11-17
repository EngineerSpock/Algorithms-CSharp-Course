using System;
using System.Collections;
using System.Collections.Generic;

namespace Algorithms_DataStruct_Lib.Queues
{
    public class CircularQueue<T> : IEnumerable<T>
    {
        private T[] _queue;

        public CircularQueue()
        {
            const int defaultCapacity = 4;
            _queue = new T[defaultCapacity];
        }
        public CircularQueue(int capacity)
        {
            _queue = new T[capacity];
        }

        public void Enqueue(T item)
        {
            if (Count == _queue.Length - 1)
            {
                int countPriorResize = Count;
                T[] newArray = new T[2*_queue.Length];

                Array.Copy(_queue, _head, newArray, 0, _queue.Length - _head);
                Array.Copy(_queue, 0, newArray, _queue.Length - _head, _tail);

                _queue = newArray;

                _head = 0;
                _tail = countPriorResize;
            }

            _queue[_tail] = item;
            if (_tail < _queue.Length - 1)
            {
                _tail++;
            }
            else
            {
                _tail = 0;
            }
        }

        public void Dequeue()
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            _queue[_head++] = default(T);

            if (IsEmpty)
                _head = _tail = 0;
            else if(_head == _queue.Length)
            {
                _head = 0;
            }
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException();
            return _queue[_head];
        }

        public bool IsEmpty => Count == 0;

        private int _head;
        private int _tail;
        public int Count => _head <= _tail 
            ? _tail - _head 
            : _tail - _head + _queue.Length;

        public int Capacity => _queue.Length;

        public IEnumerator<T> GetEnumerator()
        {
            if (_head <= _tail)
            {
                for (int i = _head; i < _tail; i++)
                {
                    yield return _queue[i];
                }
            }
            else
            {
                for (int i = _head; i < _queue.Length; i++)
                {
                    yield return _queue[i];
                }

                for (int i = 0; i < _tail; i++)
                {
                    yield return _queue[i];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}