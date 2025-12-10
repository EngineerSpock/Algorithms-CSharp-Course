using System;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{    
    [ThreadingDiagnoser]
    public class FalseSharingBench
    {
        // How many threads participate in the test
        [Params(4, 8)]
        public int ThreadCount { get; set; }

        // How many increments each thread performs
        private const int OperationsPerThread = 10_000_000;

        // Padding so that counters for different threads are far apart in memory
        private const int Padding = 16; // 16 * 8 bytes = 128 bytes per thread

        private long[] _compactCounters; // one long per thread (false sharing)
        private long[] _paddedCounters;  // one long per thread, but spaced apart

        [GlobalSetup]
        public void Setup()
        {
            int maxThreads = ThreadCount;

            // Compact: counters[0], counters[1], ... sit next to each other in memory
            _compactCounters = new long[maxThreads];

            // Padded: counter for thread t is at index t * Padding
            _paddedCounters = new long[maxThreads * Padding];
        }

        [Benchmark]
        public long CompactCounters_FalseSharing()
        {
            var counters = _compactCounters;
            int threads = ThreadCount;

            Parallel.For(0, threads, t =>
            {
                for (int i = 0; i < OperationsPerThread; i++)
                {
                    counters[t]++;  // many threads write to adjacent longs → false sharing
                }
            });

            // Aggregate the result to prevent JIT from optimizing the loop away
            long total = 0;
            for (int i = 0; i < threads; i++)
                total += counters[i];

            return total;
        }

        [Benchmark]
        public long PaddedCounters_NoFalseSharing()
        {
            var counters = _paddedCounters;
            int threads = ThreadCount;

            Parallel.For(0, threads, t =>
            {
                int index = t * Padding; // each thread gets its own "slot"
                for (int i = 0; i < OperationsPerThread; i++)
                {
                    counters[index]++;  // each thread touches its own cache line
                }
            });

            long total = 0;
            for (int t = 0; t < threads; t++)
                total += counters[t * Padding];

            return total;
        }

        public static void Run()
        {
            BenchmarkRunner.Run<FalseSharingBench>();
        }
    }
}
