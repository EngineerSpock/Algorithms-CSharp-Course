using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{
    /// <summary>
    /// Benchmark to demonstrate the impact of branch predictability.
    /// We compare the same operation on:
    /// - Predictable data (all small values first, then all large values)
    /// - Randomly shuffled data with the same value distribution
    /// </summary>
    public class BranchPredictionBench
    {
        private const int N = 1_000_000;

        private byte[] _predictable; // sorted: first half < 128, second half >= 128
        private byte[] _random;      // same values, but shuffled randomly

        [GlobalSetup]
        public void Setup()
        {
            var rnd = new Random(42);
            var values = new byte[N];

            // Fill with 50% values < 128 and 50% values >= 128
            for (int i = 0; i < N / 2; i++)
            {
                values[i] = (byte)rnd.Next(0, 128);      // "small" values
            }

            for (int i = N / 2; i < N; i++)
            {
                values[i] = (byte)rnd.Next(128, 256);    // "large" values
            }

            // Predictable version: just copy as-is (all small, then all large)
            _predictable = new byte[N];
            Array.Copy(values, _predictable, N);

            // Random version: same multiset of values, but shuffled
            _random = new byte[N];
            Array.Copy(values, _random, N);
            Shuffle(_random, rnd);
        }

        private static void Shuffle(byte[] array, Random rnd)
        {
            // Fisher–Yates shuffle
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rnd.Next(i + 1);
                byte tmp = array[i];
                array[i] = array[j];
                array[j] = tmp;
            }
        }

        [Benchmark]
        public long PredictableBranch()
        {
            long sum = 0;
            var data = _predictable;

            for (int i = 0; i < data.Length; i++)
            {
                byte v = data[i];

                // For predictable data, branch predictor quickly learns:
                // first many iterations are true, then many are false.
                if (v < 128)
                {
                    sum += v;
                }
            }

            return sum;
        }

        [Benchmark]
        public long RandomBranch()
        {
            long sum = 0;
            var data = _random;

            for (int i = 0; i < data.Length; i++)
            {
                byte v = data[i];

                // Same condition, same overall distribution of values,
                // but now outcomes are effectively random from iteration to iteration.
                if (v < 128)
                {
                    sum += v;
                }
            }

            return sum;
        }

        /// <summary>
        /// Helper to run this benchmark from a console app.
        /// </summary>
        public static void Run()
        {
            BenchmarkRunner.Run<BranchPredictionBench>();
        }
    }
}
