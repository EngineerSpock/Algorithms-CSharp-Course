using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithms_DataStruct_Lib
{
    public class DoublyLinkedNode<T>
    {
        public DoublyLinkedNode<T> Next { get; internal set; }
        public DoublyLinkedNode<T> Previous { get; internal set; }

        public T Value { get; set; }

        public DoublyLinkedNode(T value)
        {
            Value = value;
        }
    }
}
