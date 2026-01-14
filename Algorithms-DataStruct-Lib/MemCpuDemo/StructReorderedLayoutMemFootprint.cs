using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{
    // 12-byte struct on typical .NET runtimes due to padding:
    // byte + (3 padding) + int + byte + (3 tail padding) = 12
    public struct Unordered
    {
        public byte A;
        public int B;
        public byte C;
    }

    // 8-byte struct:
    // int + byte + byte + (2 tail padding) = 8
    public struct Ordered
    {
        public int B;
        public byte A;
        public byte C;
    }

    public class StructLayoutFootprintBench
    {
        // Keep N big enough so cache behavior matters.
        // If your machine runs out of RAM or starts swapping, reduce it (e.g., 5_000_000).
        [Params(10_000_000)]
        public int N;

        private Unordered[] _unordered;
        private Ordered[] _ordered;

        [GlobalSetup]
        public void Setup()
        {
            _unordered = new Unordered[N];
            _ordered = new Ordered[N];

            // Fill arrays so the JIT can't treat them as "all zeros" and do anything clever.
            // Also ensures memory pages are committed.
            for (int i = 0; i < N; i++)
            {
                _unordered[i] = new Unordered { A = (byte)(i & 0xFF), B = i, C = (byte)((i >> 8) & 0xFF) };
                _ordered[i] = new Ordered { B = i, A = (byte)(i & 0xFF), C = (byte)((i >> 8) & 0xFF) };
            }
        }

        [Benchmark]
        public long SumOnUnordered()
        {
            long sum = 0;
            var arr = _unordered;

            for (int i = 0; i < arr.Length; i++)
                sum += arr[i].B;

            return sum;
        }

        [Benchmark]
        public long SumOnOrdered()
        {
            long sum = 0;
            var arr = _ordered;

            for (int i = 0; i < arr.Length; i++)
                sum += arr[i].B;

            return sum;
        }

        public static void Run()
        {
            BenchmarkRunner.Run<StructLayoutFootprintBench>();
        }
    }
}