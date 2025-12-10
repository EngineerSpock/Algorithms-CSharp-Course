using System;
using System.Diagnostics;
using System.Numerics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{
    public class SimdOneDimArrayBench
    {
        private const int N = 1_000_000;

        private float[] _a;
        private float[] _b;
        private float[] _result;

        [GlobalSetup]
        public void Setup()
        {
            _a = new float[N];
            _b = new float[N];
            _result = new float[N];

            for (int i = 0; i < N; i++)
            {
                _a[i] = 1.0f;
                _b[i] = 2.0f;
            }
        }

        [Benchmark]
        public void NormalSum()
        {
            var a = _a;
            var b = _b;
            var result = _result;

            for (int i = 0; i < N; i++)
            {
                result[i] = a[i] + b[i];
            }
        }

        [Benchmark]
        public void SimdSum()
        {
            var a = _a;
            var b = _b;
            var result = _result;

            int vectorSize = Vector<float>.Count;
            int i = 0;

            for (; i <= N - vectorSize; i += vectorSize)
            {
                var va = new Vector<float>(a, i);
                var vb = new Vector<float>(b, i);
                var vr = va + vb;
                vr.CopyTo(result, i);
            }

            // processing the Tail
            for (; i < N; i++)
            {
                result[i] = a[i] + b[i];
            }
        }

        public static void Run()
        {
            BenchmarkRunner.Run<SimdOneDimArrayBench>();
        }
    }
}
