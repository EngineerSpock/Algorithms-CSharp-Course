using System;
using Algorithms_DataStruct_Lib.SymbolTables;
using Algorithms_DataStruct_Lib.Trees;

namespace Algorithms_CSharp_Course
{
    internal static class DataStructureRuns
    {
        public static void RunPrimeDemo()
        {
            foreach (var prime in Prime.Sieve(30))
            {
                Console.WriteLine(prime);
            }
        }

        public static void RunBstDemo()
        {
            var bst = new Bst<int>();
            bst.Insert(37);
            bst.Insert(24);
            bst.Insert(17);
            bst.Insert(28);
            bst.Insert(31);
            bst.Insert(29);
            bst.Insert(15);
            bst.Insert(12);
            bst.Insert(20);

            foreach (var value in bst.TraverseInOrder())
            {
                Console.Write($"{value} ");
            }

            Console.WriteLine();
            Console.WriteLine(bst.Min());
            Console.WriteLine(bst.Max());
            Console.WriteLine(bst.Get(20).Value);
            Console.Read();
        }

        public static void RunMaxHeapDemo()
        {
            var heap = new MaxHeap<int>();
            heap.Insert(5);
            heap.Insert(3);
            heap.Insert(8);
            heap.Insert(1);
            heap.Insert(6);

            Console.WriteLine("Heap values:");
            foreach (var value in heap.Values())
            {
                Console.WriteLine(value);
            }

            Console.WriteLine($"Peek: {heap.Peek()}");

            Console.WriteLine("Removing elements:");
            while (!heap.IsEmpty)
            {
                Console.WriteLine(heap.Remove());
            }
        }
    }
}
