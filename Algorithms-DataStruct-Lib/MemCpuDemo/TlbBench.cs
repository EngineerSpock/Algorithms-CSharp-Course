using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.TlbDemo
{
    [MemoryDiagnoser]
    public class TlbRandomPagesBench
    {
        private const int PageSize    = 4096;
        private const int Operations  = 50_000_000; // more ops → less noise

        private byte[] _buffer = null!;
        private int[] _randomPages = null!; // precomputed random page indices in [0, PagesToTouch)

        // Number of pages in the working set
        // 1 < 64 < 256 < 1024 < 4096 < 16384
        [Params(1, 64, 256, 1024, 4096, 16384)]
        public int PagesToTouch { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            // Buffer size depends on the working set.
            _buffer = new byte[PagesToTouch * PageSize];

            for (int i = 0; i < _buffer.Length; i++)
                _buffer[i] = (byte)(i * 37 + 11);

            // Precomputing random page indices
            // in range [0, PagesToTouch).
            _randomPages = new int[Operations];
            var rnd = new Random(42);
            int pagesToTouch = PagesToTouch;

            for (int i = 0; i < Operations; i++)
            {
                _randomPages[i] = rnd.Next(pagesToTouch);
            }
        }

        [Benchmark]
        public long WalkPages()
        {
            var buffer      = _buffer;
            var randomPages = _randomPages;
            int pageSize    = PageSize;

            long sum = 0;

            // We perform the same number of memory accesses in all scenarios.
            //
            // For each access:
            //   - we take a pre-generated random page index,
            //   - we read one byte at the beginning of that page.
            //
            // As PagesToTouch increases:
            //   - the working set spans more memory pages,
            //   - the TLB has to keep more address translations,
            //   - the number of TLB misses grows → more page walks occur →
            //     the average memory access latency increases.
            for (int i = 0; i < Operations; i++)
            {
                int page = randomPages[i];
                int index = page * pageSize;

                sum += buffer[index];
            }

            return sum;
        }

        public static void Run()
        {
            BenchmarkRunner.Run<TlbRandomPagesBench>();
        }
    }
}
