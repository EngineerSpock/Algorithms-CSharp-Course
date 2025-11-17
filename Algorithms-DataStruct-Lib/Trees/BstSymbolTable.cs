using System;
using System.Collections.Generic;

namespace Algorithms_DataStruct_Lib.Trees
{
    public class BstSymbolTable<TKey, TValue> where TKey : IComparable<TKey>
    {
        private TreeNode _root; 

        public bool IsEmpty => Count == 0;

        /**
         * Returns the number of key-value pairs in this symbol table.
         * @return the number of key-value pairs in this symbol table
         */
        public int Count => GetCount(_root);

        // return number of key-value pairs in BST rooted at x
        private int GetCount(TreeNode x)
        {
            if (x == null) return 0;
            return x.Count;
        }

        /**
         * Does this symbol table contain the given key?
         *
         * @param  key the key
         * @return {@code true} if this symbol table contains {@code key} and
         *         {@code false} otherwise
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public bool contains(TKey key)
        {
            if (key == null) throw new ArgumentException("argument to contains() is null");
            return get(key) != null;
        }

        /**
         * Returns the value associated with the given key.
         *
         * @param  key the key
         * @return the value associated with the given key if the key is in the symbol table
         *         and {@code null} if the key is not in the symbol table
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public TValue get(TKey key)
        {
            return get(_root, key);
        }

        private TValue get(TreeNode x, TKey key)
        {
            if (key == null) throw new ArgumentException("calls get() with a null key");
            if (x == null) return default(TValue);
            var cmp = key.CompareTo(x.Key);
            if (cmp < 0) return get(x.Left, key);
            if (cmp > 0) return get(x.Right, key);
            return x.Value;
        }

        /**
         * Inserts the specified key-value pair into the symbol table, overwriting the old 
         * value with the new value if the symbol table already contains the specified key.
         * Deletes the specified key (and its associated value) from this symbol table
         * if the specified value is {@code null}.
         *
         * @param  key the key
         * @param  val the value
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public void put(TKey key, TValue val)
        {
            if (key == null) throw new ArgumentException("calls put() with a null key");
            if (val == null)
            {
                delete(key);
                return;
            }

            _root = put(_root, key, val);
        }

        private TreeNode put(TreeNode x, TKey key, TValue val)
        {
            if (x == null) return new TreeNode(key, val, 1);
            var cmp = key.CompareTo(x.Key);
            if (cmp < 0) x.Left = put(x.Left, key, val);
            else if (cmp > 0) x.Right = put(x.Right, key, val);
            else x.Value = val;
            x.Count = 1 + GetCount(x.Left) + GetCount(x.Right);
            return x;
        }


        /**
         * Removes the smallest key and associated value from the symbol table.
         *
         * @throws InvalidOperationException if the symbol table is empty
         */
        public void deleteMin()
        {
            if (IsEmpty) throw new InvalidOperationException("Symbol table underflow");
            _root = deleteMin(_root);
        }

        private TreeNode deleteMin(TreeNode x)
        {
            if (x.Left == null) return x.Right;
            x.Left = deleteMin(x.Left);
            x.Count = GetCount(x.Left) + GetCount(x.Right) + 1;
            return x;
        }

        /**
         * Removes the largest key and associated value from the symbol table.
         *
         * @throws InvalidOperationException if the symbol table is empty
         */
        public void deleteMax()
        {
            if (IsEmpty) throw new InvalidOperationException("Symbol table underflow");
            _root = deleteMax(_root);
        }

        private TreeNode deleteMax(TreeNode x)
        {
            if (x.Right == null) return x.Left;
            x.Right = deleteMax(x.Right);
            x.Count = GetCount(x.Left) + GetCount(x.Right) + 1;
            return x;
        }

        /**
         * Removes the specified key and its associated value from this symbol table     
         * (if the key is in this symbol table).    
         *
         * @param  key the key
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public void delete(TKey key)
        {
            if (key == null) throw new ArgumentException("calls delete() with a null key");
            _root = delete(_root, key);
        }

        private TreeNode delete(TreeNode x, TKey key)
        {
            if (x == null) return null;

            var cmp = key.CompareTo(x.Key);
            if (cmp < 0)
            {
                x.Left = delete(x.Left, key);
            }
            else if (cmp > 0)
            {
                x.Right = delete(x.Right, key);
            }
            else
            {
                if (x.Right == null) return x.Left;
                if (x.Left == null) return x.Right;
                var t = x;
                x = min(t.Right);
                x.Right = deleteMin(t.Right);
                x.Left = t.Left;
            }

            x.Count = GetCount(x.Left) + GetCount(x.Right) + 1;
            return x;
        }


        /**
         * Returns the smallest key in the symbol table.
         *
         * @return the smallest key in the symbol table
         * @throws InvalidOperationException if the symbol table is empty
         */
        public TKey min()
        {
            if (IsEmpty) throw new InvalidOperationException("calls min() with empty symbol table");
            return min(_root).Key;
        }

        private TreeNode min(TreeNode x)
        {
            if (x.Left == null) return x;
            return min(x.Left);
        }

        /**
         * Returns the largest key in the symbol table.
         *
         * @return the largest key in the symbol table
         * @throws InvalidOperationException if the symbol table is empty
         */
        public TKey max()
        {
            if (IsEmpty) throw new InvalidOperationException("calls max() with empty symbol table");
            return max(_root).Key;
        }

        private TreeNode max(TreeNode x)
        {
            if (x.Right == null) return x;
            return max(x.Right);
        }

        /**
         * Returns the largest key in the symbol table less than or equal to {@code key}.
         *
         * @param  key the key
         * @return the largest key in the symbol table less than or equal to {@code key}
         * @throws InvalidOperationException if there is no such key
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public TKey floor(TKey key)
        {
            if (key == null) throw new ArgumentException("argument to floor() is null");
            if (IsEmpty) throw new InvalidOperationException("calls floor() with empty symbol table");
            var x = floor(_root, key);
            if (x == null) return default(TKey);
            return x.Key;
        }

        private TreeNode floor(TreeNode x, TKey key)
        {
            if (x == null) return null;
            var cmp = key.CompareTo(x.Key);
            if (cmp == 0) return x;
            if (cmp < 0) return floor(x.Left, key);
            var t = floor(x.Right, key);
            if (t != null) return t;
            return x;
        }

        public TKey floor2(TKey key)
        {
            return floor2(_root, key, default(TKey));
        }

        private TKey floor2(TreeNode x, TKey key, TKey best)
        {
            if (x == null) return best;
            var cmp = key.CompareTo(x.Key);
            if (cmp < 0) return floor2(x.Left, key, best);
            if (cmp > 0) return floor2(x.Right, key, x.Key);
            return x.Key;
        }

        /**
         * Returns the smallest key in the symbol table greater than or equal to {@code key}.
         *
         * @param  key the key
         * @return the smallest key in the symbol table greater than or equal to {@code key}
         * @throws InvalidOperationException if there is no such key
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public TKey ceiling(TKey key)
        {
            if (key == null) throw new ArgumentException("argument to ceiling() is null");
            if (IsEmpty) throw new InvalidOperationException("calls ceiling() with empty symbol table");
            var x = ceiling(_root, key);
            if (x == null) return default(TKey);
            return x.Key;
        }

        private TreeNode ceiling(TreeNode x, TKey key)
        {
            if (x == null) return null;
            var cmp = key.CompareTo(x.Key);
            if (cmp == 0) return x;
            if (cmp < 0)
            {
                var t = ceiling(x.Left, key);
                if (t != null) return t;
                return x;
            }

            return ceiling(x.Right, key);
        }

        /**
         * Return the key in the symbol table whose rank is {@code k}.
         * This is the (k+1)st smallest key in the symbol table.
         *
         * @param  k the order statistic
         * @return the key in the symbol table of rank {@code k}
         * @throws ArgumentException unless {@code k} is between 0 and
         *        <em>n</em>–1
         */
        public TKey select(int k)
        {
            if (k < 0 || k >= Count) throw new ArgumentException("argument to select() is invalid: " + k);

            var x = select(_root, k);
            return x.Key;
        }

        // Return key of rank k. 
        private TreeNode select(TreeNode x, int k)
        {
            if (x == null) return null;
            var t = GetCount(x.Left);
            if (t > k) return select(x.Left, k);
            if (t < k) return select(x.Right, k - t - 1);
            return x;
        }

        /**
         * Return the number of keys in the symbol table strictly less than {@code key}.
         *
         * @param  key the key
         * @return the number of keys in the symbol table strictly less than {@code key}
         * @throws ArgumentException if {@code key} is {@code null}
         */
        public int rank(TKey key)
        {
            if (key == null) throw new ArgumentException("argument to rank() is null");
            return rank(key, _root);
        }

        // Number of keys in the subtree less than key.
        private int rank(TKey key, TreeNode x)
        {
            if (x == null) return 0;
            var cmp = key.CompareTo(x.Key);
            if (cmp < 0) return rank(key, x.Left);
            if (cmp > 0) return 1 + GetCount(x.Left) + rank(key, x.Right);
            return GetCount(x.Left);
        }

        /**
         * Returns all keys in the symbol table as an {@code IEnumerable}.
         * To iterate over all of the keys in the symbol table named {@code st},
         * use the foreach notation: {@code for (Key key : st.keys())}.
         *
         * @return all keys in the symbol table
         */
        public IEnumerable<TKey> keys()
        {
            if (IsEmpty) return new Queue<TKey>();
            return keys(min(), max());
        }

        /**
         * Returns all keys in the symbol table in the given range,
         * as an {@code IEnumerable}.
         *
         * @param  lo minimum endpoint
         * @param  hi maximum endpoint
         * @return all keys in the symbol table between {@code lo} 
         *         (inclusive) and {@code hi} (inclusive)
         * @throws ArgumentException if either {@code lo} or {@code hi}
         *         is {@code null}
         */
        public IEnumerable<TKey> keys(TKey lo, TKey hi)
        {
            if (lo == null) throw new ArgumentException("first argument to keys() is null");
            if (hi == null) throw new ArgumentException("second argument to keys() is null");

            var queue = new Queue<TKey>();
            keys(_root, queue, lo, hi);
            return queue;
        }

        private void keys(TreeNode x, Queue<TKey> queue, TKey lo, TKey hi)
        {
            if (x == null) return;
            var cmplo = lo.CompareTo(x.Key);
            var cmphi = hi.CompareTo(x.Key);
            if (cmplo < 0) keys(x.Left, queue, lo, hi);
            if (cmplo <= 0 && cmphi >= 0) queue.Enqueue(x.Key);
            if (cmphi > 0) keys(x.Right, queue, lo, hi);
        }

        /**
         * Returns the number of keys in the symbol table in the given range.
         *
         * @param  lo minimum endpoint
         * @param  hi maximum endpoint
         * @return the number of keys in the symbol table between {@code lo} 
         *         (inclusive) and {@code hi} (inclusive)
         * @throws ArgumentException if either {@code lo} or {@code hi}
         *         is {@code null}
         */
        public int GetCount(TKey lo, TKey hi)
        {
            if (lo == null) throw new ArgumentException("first argument to size() is null");
            if (hi == null) throw new ArgumentException("second argument to size() is null");

            if (lo.CompareTo(hi) > 0) return 0;
            if (contains(hi)) return rank(hi) - rank(lo) + 1;
            return rank(hi) - rank(lo);
        }

        /**
         * Returns the height of the BST (for debugging).
         *
         * @return the height of the BST (a 1-node tree has height 0)
         */
        public int height()
        {
            return height(_root);
        }

        private int height(TreeNode x)
        {
            if (x == null) return -1;
            return 1 + Math.Max(height(x.Left), height(x.Right));
        }

        /**
         * Returns the keys in the BST in level order (for debugging).
         *
         * @return the keys in the BST in level order traversal
         */
        public IEnumerable<TKey> levelOrder()
        {
            var keys = new Queue<TKey>();
            var queue = new Queue<TreeNode>();
            queue.Enqueue(_root);
            while (queue.Count != 0)
            {
                var x = queue.Dequeue();
                if (x == null) continue;
                keys.Enqueue(x.Key);
                queue.Enqueue(x.Left);
                queue.Enqueue(x.Right);
            }

            return keys;
        }

        private class TreeNode
        {
            public TKey Key { get; } 
            public TreeNode Left     {get; set;}
            public TreeNode Right    {get; set;}
            public int Count         {get; set;}
            public TValue Value      {get; set;}

            public TreeNode(TKey key, TValue value, int count)
            {
                this.Key = key;
                this.Value = value;
                this.Count = count;
            }
        }
    }
}