using System;
using System.Collections.Generic;
using System.Threading;
using Algorithms_DataStruct_Lib.Queues;

namespace Algorithms_DataStruct_Lib.SymbolTables
{
    public class BinarySearchSt<TKey, TValue>
    {
        private TKey[] _keys;
        private TValue[] _values;

        public int Count { get; private set; }

        private readonly IComparer<TKey> _comparer;

        public int Capacity => _keys.Length;
        public bool IsEmpty => Count == 0;

        private const int DefaultCapacity = 4;


        public BinarySearchSt(int capacity, IComparer<TKey> comparer)
        {
            _keys = new TKey[capacity];
            _values = new TValue[capacity];
            _comparer = comparer ?? throw new ArgumentNullException("Comparer can't be null.");
        }

        public BinarySearchSt(int capacity):this(capacity, Comparer<TKey>.Default)
        {
            
        }

        public BinarySearchSt():this(DefaultCapacity)
        {
            
        }

        public int Rank(TKey key)
        {
            int low = 0;
            int high = Count - 1;

            while (low <= high)
            {
                int mid = low + (high - low) / 2;

                int cmp = _comparer.Compare(key, _keys[mid]);

                if (cmp < 0)
                    high = mid - 1;
                else if (cmp > 0)
                    low = mid + 1;
                else
                    return mid;
            }
            return low;
        }

        public TValue GetValueOrDefault(TKey key)
        {
            if (IsEmpty)
            {
                return default(TValue);
            }

            int rank = Rank(key);
            if (rank < Count && _comparer.Compare(_keys[rank], key) == 0)
            {
                return _values[rank];
            }

            return default(TValue);
        }

        public void Add(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException("Key can't be null");

            int rank = Rank(key);
            if (rank < Count && _comparer.Compare(_keys[rank], key) == 0)
            {
                _values[rank] = value;
                return;
            }

            if (Count == Capacity)
                Resize(Capacity * 2);

            for (int j = Count; j > rank; j--)
            {
                _keys[j] = _keys[j - 1];
                _values[j] = _values[j - 1];
            }

            _keys[rank] = key;
            _values[rank] = value;

            Count++;
        }

        public void Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("Key can't be null");

            if (IsEmpty)
                return;

            int r = Rank(key);
            if (r == Count || _comparer.Compare(_keys[r], key) != 0)
                return;

            for (int j = r; j < Count - 1; j++)
            {
                _keys[j] = _keys[j + 1];
                _values[j] = _values[j + 1];
            }

            Count--;
            _keys[Count] = default(TKey);
            _values[Count] = default(TValue);

            //resize if 1/4 full
            //if(Count > 0 && Count ==keys.Length / 4) Resize(_keys.Length / 2);
        }

        public bool Contains(TKey key)
        {
            int r = Rank(key);
            if (r < Count && _comparer.Compare(_keys[r], key) == 0)
                return true;
            return false;
        }

        public IEnumerable<TKey> Keys()
        {
            foreach (var key in _keys)
            {
                yield return key;
            }
        }

        private void Resize(int capacity)
        {
            TKey[] keysTmp = new TKey[capacity];
            TValue[] valuesTmp = new TValue[capacity];

            for (int i = 0; i < Count; i++)
            {
                keysTmp[i] = _keys[i];
                valuesTmp[i] = _values[i];
            }

            _values = valuesTmp;
            _keys = keysTmp;
        }

        public TKey Min()
        {
            if(IsEmpty)
                throw new InvalidOperationException("Table is empty.");
            return _keys[0];
        }

        public TKey Max()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Table is empty.");
            return _keys[Count - 1];
        }

        public void RemoveMin()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Table is empty.");

            Remove(Min());
        }

        public void RemoveMax()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Table is empty.");

            Remove(Max());
        }

        public TKey Select(int index)
        {
            if(index<0 || index>=Count)
                throw new ArgumentException("Can't select since index is out of range.");
            return _keys[index];
        }

        public TKey Ceiling(TKey key)
        {
            if(key==null)
                throw new ArgumentNullException("Argument to ceiling() is null.");

            int r = Rank(key);
            if (r == Count) return default(TKey);
            else return _keys[r];
        }

        public TKey Floor(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException("Argument to floor() is null.");

            int r = Rank(key);
            if(r<Count && _comparer.Compare(_keys[r], key)==0) return _keys[r];
            if (r == 0)
                return default(TKey);
            else
                return _keys[r - 1];
        }

        public IEnumerable<TKey> Range(TKey left, TKey right)
        {
            var q = new LinkedQueue<TKey>();

            int low = Rank(left);
            int high = Rank(right);

            for (int i = low; i < high; i++)
            {
                q.Enqueue(_keys[i]);
            }

            if (Contains(right))
                q.Enqueue(_keys[Rank(right)]);

            return q;
        }
    }
}