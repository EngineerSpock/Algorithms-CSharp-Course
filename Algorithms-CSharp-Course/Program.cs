using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Algorithms_DataStruct_Lib;
using Algorithms_DataStruct_Lib.Alignment;
using Algorithms_DataStruct_Lib.BranchPrediction;
using Algorithms_DataStruct_Lib.Stacks;
using Algorithms_DataStruct_Lib.SymbolTables;
using Algorithms_DataStruct_Lib.Trees;

namespace Algorithms_CSharp_Course
{    
    class Program
    {
        static void Main(string[] args)
        {
            BranchPredictionBench.Run();
            // SimdAlignmentScenariosBench.RunAll();

            Console.Read(); 
        }

        private static void HashSetTestRun()
        {
            var c1 = new Person
            {
                Age = 18,
                Ssn = 1000
            };

            var c2 = new Person
            {
                Age = 18,
                Ssn = 1000
            };

            Console.WriteLine(c1.GetHashCode() == c2.GetHashCode());

            HashSet<Person> hs = [c1, c2];
            Console.WriteLine(hs.Count);
            Console.Read();
        }

        private static void BstTestRun()
        {
            var sst = new BinarySearchSt<string, int>(1000000);

            //const int minLength = 10;
            const int minLength = 8;
            //string[] words = File.ReadAllText("Data\\leipzig1M.txt")
            string[] separators = { "\r\n", "", " " };
            string[] words = File.ReadAllText("Data\\tale.txt")
                        .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                        //.Select(str => Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled))
                        .Where(str => str.Length >= minLength)
                        .ToArray();

            var find = words.ToList().Where(str => str.Contains("busin")).ToList();

            foreach (var word in words)
            {
                if (!sst.Contains(word))
                {
                    sst.Add(word, 1);
                }
                else
                {
                    sst.Add(word, sst.GetValueOrDefault(word) + 1);
                }
            }

            string max = "";
            sst.Add(max, 0);

            foreach (var word in sst.Keys())
            {
                if (sst.GetValueOrDefault(word) > sst.GetValueOrDefault(max))
                {
                    max = word;
                }
            }

            Console.WriteLine($"The max counter = {sst.GetValueOrDefault(max)} and it is for {max}");
            Console.Read();
        }

        private static void LinearSearchDemo()
        {
            var customersList = new List<Person>()
            {
                new Person {Age = 3, Name = "Ann"},
                new Person {Age = 16, Name = "Bill"},
                new Person {Age = 20, Name = "Rose"},
                new Person {Age = 14, Name = "Rob"},
                new Person {Age = 28, Name = "Bill"},
                new Person {Age = 14, Name = "John"},
            };
            var intList = new List<int>() {1, 4, 2, 7, 5, 9, 12, 3, 2, 1};
            
            bool contains = intList.Contains(3);
            bool contains2 = customersList.Contains(new Person {Age = 14, Name = "Rob"}, new CustomersComparer());

            bool exists = customersList.Exists(customer => customer.Age == 28);

            int min = intList.Min();
            int max = intList.Max();

            int youngestCustomerAge = customersList.Min(customer => customer.Age);

            Person bill = customersList.Find(customer => customer.Name == "Bill");
            Person lastBill = customersList.FindLast(customer => customer.Name == "Bill");
            Person lastBill2 = customersList.Last(customer => customer.Name == "Bill");

            List<Person> customers = customersList.FindAll(customer => customer.Age > 18);
            IEnumerable<Person> whereAge = customersList.Where(customer => customer.Age > 18);

            int index1 = customersList.FindIndex(customer => customer.Age == 14);
            int lastIndex = customersList.FindLastIndex(customer => customer.Age > 18);

            int indexOf = intList.IndexOf(2);
            int lastIndexOf = intList.LastIndexOf(2);

            //from list
            bool isTrueForAll = customersList.TrueForAll(customer => customer.Age > 10);

            //from linq
            bool all = customersList.All(customer => customer.Age > 18);

            bool areThereAny = customersList.Any(customer => customer.Age == 3);

            int count = customersList.Count(customer => customer.Age > 18);

            Person firstBill = customersList.First(customer => customer.Name == "Bill");

            Person singleAnn = customersList.Single(customer => customer.Name == "Ann");
        }

        private static void BitArrayDemo()
        {
            // Creates and initializes several BitArrays.
            BitArray myBA1 = new BitArray(5);

            BitArray myBA2 = new BitArray(5, false);

            byte[] myBytes = new byte[5] { 1, 2, 3, 4, 5 };
            BitArray myBA3 = new BitArray(myBytes);

            bool[] myBools = new bool[5] { true, false, true, true, false };
            BitArray myBA4 = new BitArray(myBools);

            int[] myInts = new int[5] { 6, 7, 8, 9, 10 };
            BitArray myBA5 = new BitArray(myInts);

            // Displays the properties and values of the BitArrays.
            Console.WriteLine("myBA1");
            Console.WriteLine("   Count:    {0}", myBA1.Count);
            Console.WriteLine("   Length:   {0}", myBA1.Length);
            Console.WriteLine("   Values:");
            PrintValues(myBA1, 8);

            Console.WriteLine("myBA2");
            Console.WriteLine("   Count:    {0}", myBA2.Count);
            Console.WriteLine("   Length:   {0}", myBA2.Length);
            Console.WriteLine("   Values:");
            PrintValues(myBA2, 8);

            Console.WriteLine("myBA3");
            Console.WriteLine("   Count:    {0}", myBA3.Count);
            Console.WriteLine("   Length:   {0}", myBA3.Length);
            Console.WriteLine("   Values:");
            PrintValues(myBA3, 8);

            Console.WriteLine("myBA4");
            Console.WriteLine("   Count:    {0}", myBA4.Count);
            Console.WriteLine("   Length:   {0}", myBA4.Length);
            Console.WriteLine("   Values:");
            PrintValues(myBA4, 8);

            Console.WriteLine("myBA5");
            Console.WriteLine("   Count:    {0}", myBA5.Count);
            Console.WriteLine("   Length:   {0}", myBA5.Length);
            Console.WriteLine("   Values:");
            PrintValues(myBA5, 8);
        }

        private static void BitVector32()
        {
            /*
            // Creates and initializes a BitVector32.
            BitVector32 myBV = new BitVector32(0);

            // Creates four sections in the BitVector32 with maximum values 6, 3, 1, and 15.
            // mySect3, which uses exactly one bit, can also be used as a bit flag.
            BitVector32.Section mySect1 = BitVector32.CreateSection(6);
            BitVector32.Section mySect2 = BitVector32.CreateSection(3, mySect1);
            BitVector32.Section mySect3 = BitVector32.CreateSection(1, mySect2);
            BitVector32.Section mySect4 = BitVector32.CreateSection(15, mySect3);

            // Displays the values of the sections.
            Console.WriteLine("Initial values:");
            Console.WriteLine("\tmySect1: {0}", myBV[mySect1]);
            Console.WriteLine("\tmySect2: {0}", myBV[mySect2]);
            Console.WriteLine("\tmySect3: {0}", myBV[mySect3]);
            Console.WriteLine("\tmySect4: {0}", myBV[mySect4]);

            // Sets each section to a new value and displays the value of the BitVector32 at each step.
            Console.WriteLine("Changing the values of each section:");
            Console.WriteLine("\tInitial:    \t{0}", myBV.ToString());
            myBV[mySect1] = 5;
            Console.WriteLine("\tmySect1 = 5:\t{0}", myBV.ToString());
            myBV[mySect2] = 3;
            Console.WriteLine("\tmySect2 = 3:\t{0}", myBV.ToString());
            myBV[mySect3] = 1;
            Console.WriteLine("\tmySect3 = 1:\t{0}", myBV.ToString());
            myBV[mySect4] = 9;
            Console.WriteLine("\tmySect4 = 9:\t{0}", myBV.ToString());

            // Displays the values of the sections.
            Console.WriteLine("New values:");
            Console.WriteLine("\tmySect1: {0}", myBV[mySect1]);
            Console.WriteLine("\tmySect2: {0}", myBV[mySect2]);
            Console.WriteLine("\tmySect3: {0}", myBV[mySect3]);
            Console.WriteLine("\tmySect4: {0}", myBV[mySect4]);
            */
        }

        public static void PrintValues(IEnumerable myList, int myWidth)
        {
            int i = myWidth;
            foreach (Object obj in myList)
            {
                if (i <= 0)
                {
                    i = myWidth;
                    Console.WriteLine();
                }
                i--;
                Console.Write("{0,8}", obj);
            }
            Console.WriteLine();
        }
    

        private static bool Exists(int[] array, int number)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == number)
                    return true;
            }

            return false;
        }

        private static int IndexOf(int[] array, int number)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == number)
                    return i;
            }

            return -1;
        }

        private static void BuiltInQueueDemo()
        {
            Queue<int> q = new Queue<int>(128);
            q.Enqueue(1);
            q.Enqueue(2);
            q.Enqueue(3);

            Console.WriteLine($"Should print out 1:{q.Peek()}");

            q.Dequeue();

            Console.WriteLine($"Should print out 2:{q.Peek()}");

            Console.WriteLine($"Contains 3? Answer:{q.Contains(3)}");
        }

        private static void DemoWithNodes()
        {
            //Node first = new Node() {Value = 5};
            //Node second = new Node() {Value = 1};

            //first.Next = second;

            //Node third = new Node() {Value = 3};
            //second.Next = third;

            //PrintOutLinkedList(first);
        }

        private static void StackDemo()
        {
            var stack = new Stack<int>();
            stack.Push(1);
            stack.Push(2);
            stack.Push(3);
            stack.Push(4);

            Console.WriteLine($"Should print out 4:{stack.Peek()}");

            stack.Pop();

            Console.WriteLine($"Should print out 3:{stack.Peek()}");

            Console.WriteLine("Iterave over the stack.");
            foreach (var cur in stack)
            {
                Console.WriteLine(cur);
            }
        }

        //private static void PrintOutLinkedList(Node node)
        //{
        //    while (node != null)
        //    {
        //        Console.WriteLine(node.Value);
        //        node = node.Next;
        //    }
        //}

        //1! = 1 * 0! = 1
        //2! = 2 * 1 = 2 * 1!
        //3! = 3 * 2 * 1 = 3 * 2!
        //4! = 4 * 3 * 2 * 1 = 4 * 3!
        //n! = n * (n-1)!

        //1*Calculate(1-1) = 1 ->
        //2*Calculate(2-1) = 2 * 1 = 2 ->
        //3*Calculate(3-2)=3*2 = 6

        private static int Calculate(int n)
        {
            if (n == 0)
                return 1;

            return n * Calculate(n - 1);
        }

        private static int IterativeFactorial(int number)
        {
            if (number == 0)
                return 1;

            int factorial = 1;
            for (int i = 1; i <= number; i++)
            {
                factorial *= i;
            }

            return factorial;
        }

        private static void TestThreeSum()
        {
            //300 millisecons for 1k
            //14 second for 4k
            //1 min 36 seconds for 8k
            var ints = In.ReadInts("16kints.txt").ToArray();

            var watch = new Stopwatch();
            watch.Start();

            var triplets = ThreeSum.Count(ints);

            watch.Stop();

            Console.WriteLine($"The number of \"zero-sum\" triplets:{triplets}");
            Console.WriteLine($"Time taken:{watch.Elapsed:g}");
        }

        #region Info

        static void ArrayTimeComplexity(object[] array)
        {
            //access by index O(1)
            Console.WriteLine(array[0]);

            int length = array.Length;
            object elementINeedToFind = new object();

            //searching for an element O(N)
            for (int i = 0; i < length; i++)
            {
                if (array[i] == elementINeedToFind)
                {
                    Console.WriteLine("Exists/Found");
                }
            }

            //add to a full array
            var bigArray = new int[length * 2];
            Array.Copy(array, bigArray, length);
            bigArray[length + 1] = 10;

            //add to the end when there's some space
            //O(1)
            array[length - 1] = 10;

            //O(1)
            array[6] = null;
        }

        private static void RemoveAt(object[] array, int index)
        {
            var newArray = new object[array.Length - 1];
            Array.Copy(array, 0, newArray, 0, index);
            Array.Copy(array, index + 1, newArray, index, array.Length - 1 - index);

        }

        private static unsafe void IterateOver(int[] array)
        {
            fixed (int* b = array)
            {
                int* p = b;
                for (int i = 0; i < array.Length; i++)
                {
                    Console.WriteLine(*p);
                    p++;
                }
            }
        }

        private static void MultiDimArrays()
        {
            int[,] r1 = new int[2, 3] {{1, 2, 3}, {4, 5, 6}};
            int[,] r2 = {{1, 2, 3}, {4, 5, 6}};

            for (int i = 0; i < r2.GetLength(0); i++)
            {
                for (int j = 0; j < r2.GetLength(1); j++)
                {
                    Console.WriteLine($"{r2[i, j]}");
                }
                Console.WriteLine();
            }
        }

        private static void JaggedArraysDemo()
        {
            int[][] jaggedArray = new int[4][];
            jaggedArray[0] = new int[1];
            jaggedArray[1] = new int[3];
            jaggedArray[2] = new int[2];
            jaggedArray[3] = new int[4];

            Console.WriteLine("Enter the numbers for a jagged arary.");

            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    string st = Console.ReadLine();
                    jaggedArray[i][j] = int.Parse(st);
                }
            }

            Console.WriteLine("");
            Console.WriteLine("Printing the Elements:");

            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[i].Length; j++)
                {
                    Console.Write(jaggedArray[i][j]);
                    Console.Write("\0");
                }

                Console.WriteLine("");
            }
        }

        private static void ArraysDemo()
        {
            int[] a1;
            a1 = new int[10];

            int[] a2 = new int[5];

            int[] a3 = new int[5] { 1, 3, -2, 5, 10};

            int[] a4 = {1, 3, 2, 4, 5};

            //System.Array

            //for (int i = 0; i < a3.Length; i++)
            for (int i = 0; i <= a3.Length - 1; i++)
            {
                Console.Write($"{a3[i] }");
            }

            Console.WriteLine();

            foreach (var el in a4)
            {
                Console.Write($"{el} ");
            }

            Console.WriteLine();

            Array myArray = new int[5];

            Array myArray2 = Array.CreateInstance(typeof(int), 5);
            myArray2.SetValue(1, 0);

            Console.Read();
        }

        private static void Test1BasedArray()
        {
            Array myArray = Array.CreateInstance(typeof(int), new[] {4}, new[] {1});

            myArray.SetValue(2019, 1);
            myArray.SetValue(2020, 2);
            myArray.SetValue(2021, 3);
            myArray.SetValue(2022, 4);

            Console.WriteLine($"Starting index:{myArray.GetLowerBound(0)}");
            Console.WriteLine($"Ending index:{myArray.GetUpperBound(0)}");

            //for (int i = myArray.GetLowerBound(0); i <= myArray.GetUpperBound(0); i++)
            for(int i=1; i<5; i++)
            {
                Console.WriteLine($"{myArray.GetValue(i)} at index {i}");
            }
        }
        #endregion
    }

    internal class CustomersComparer : IEqualityComparer<Person>
    {
        public bool Equals(Person x, Person y) => x.Age == y.Age && x.Name == y.Name;

        public int GetHashCode(Person obj) => obj.GetHashCode();
    }
}
