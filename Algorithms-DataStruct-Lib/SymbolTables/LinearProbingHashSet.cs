using System;
using System.Collections.Generic;

namespace Algorithms_DataStruct_Lib.SymbolTables
{
    public class LinearProbingHashSet<TKey, TValue>
    {
        private const int DefaultCapacity = 4;

        public int Count { get; private set; }
        public int Capacity { get; private set; }
        private TKey[] _keys;
        private TValue[] _values;

        public LinearProbingHashSet() : this(DefaultCapacity)
        {

        }

        public LinearProbingHashSet(int capacity)
        {
            Capacity = capacity;
            _keys = new TKey[capacity];
            _values = new TValue[capacity];
        }

        private int Hash(TKey key)
        {
            return (key.GetHashCode() & 0x7fffffff) % Capacity;
        }

        public bool Contains(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is not allowed to be null");

            for (int i = Hash(key); _keys[i] != null; i = (i + 1) % Capacity)
            {
                if (_keys[i].Equals(key))
                    return true;
            }

            return false;
        }

        public TValue Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is not allowed to be null");

            for (int i = Hash(key); _keys[i] != null; i = (i + 1) % Capacity)
            {
                if (_keys[i].Equals(key))
                {
                    return _values[i];
                }
            }

            throw new ArgumentException("Key was not found.");
        }

        public bool TryGet(TKey key, out int index)
        {
            if (key == null)
                throw new ArgumentNullException("Key is not allowed to be null");

            for (int i = Hash(key); _keys[i] != null; i = (i + 1) % Capacity)
            {
                if (_keys[i].Equals(key))
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("Key is not allowed to be null");

            if (!TryGet(key, out int index))
                return;

            _keys[index] = default(TKey);
            _values[index] = default(TValue);

            index = (index + 1) % Capacity;

            while (_keys[index] != null)
            {
                TKey keyToRehash = _keys[index];
                TValue valToRehash = _values[index];

                _keys[index] = default(TKey);
                _values[index] = default(TValue);

                Count--;

                Add(keyToRehash, valToRehash);

                index = (index + 1) % Capacity;
            }

            Count--;

            if (Count > 0 && Count <= Capacity / 8)
                Resize(Capacity / 2);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key is not allowed to be null");

            if (value == null)
            {
                Remove(key);
                return;
            }

            if (Count >= Capacity / 2)
                Resize(2 * Capacity);

            int i;
            for (i = Hash(key); _keys[i] != null; i = (i + 1) % Capacity)
            {
                if (_keys[i].Equals(key))
                {
                    _values[i] = value;
                    return;
                }
            }

            _keys[i] = key;
            _values[i] = value;

            Count++;
        }

        private void Resize(int capacity)
        {
            var temp = new LinearProbingHashSet<TKey, TValue>(capacity);

            for (int i = 0; i < Capacity; i++)
            {
                if (_keys[i] != null)
                {
                    temp.Add(_keys[i], _values[i]);
                }
            }

            _keys = temp._keys;
            _values = temp._values;

            Capacity = temp.Capacity;
        }

        public IEnumerable<TKey> Keys()
        {
            var q = new Queue<TKey>();
            for (int i = 0; i < Capacity; i++)
            {
                if(_keys[i]!=null)
                    q.Enqueue(_keys[i]);
            }

            return q;
        }
    }
}