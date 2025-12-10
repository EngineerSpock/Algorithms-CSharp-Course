using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{
    public unsafe class SimdAlignmentScenariosBench
    {
        private const int CacheLineBytes = 64;
        private const int VectorWidthAvx = 8;        // 8 floats per AVX register
        private const int TotalFloats = 1_000_000;
        private const int Operations = 1_000_000;

        private void* _raw;          // raw allocated pointer
        private float* _aligned;      // 64-byte aligned pointer
        private float* _misaligned;   // shifted by 1 float (4 bytes)
        private int[] _indices;      // irregular access indices

        [GlobalSetup]
        public void Setup()
        {
            if (!Avx.IsSupported)
                throw new PlatformNotSupportedException("AVX is not supported on this CPU.");

            nuint totalFloats = (nuint)(TotalFloats + VectorWidthAvx);
            nuint totalBytes = totalFloats * (nuint)sizeof(float);

            _raw = NativeMemory.Alloc(totalBytes);
            if (_raw == null)
                throw new OutOfMemoryException();

            // Compute a 64-byte aligned address
            nint addr = (nint)_raw;
            nint alignedAddr = (addr + (CacheLineBytes - 1)) & ~(CacheLineBytes - 1);
            _aligned = (float*)alignedAddr;

            // Deliberately misaligned by 4 bytes
            _misaligned = _aligned + 1;

            // Initialize data
            for (int i = 0; i < TotalFloats; i++)
                _aligned[i] = i * 0.001f;

            // Prepare indices for irregular access
            _indices = new int[Operations];
            int maxStart = TotalFloats - VectorWidthAvx;

            int current = 0;
            const int step = 37;

            for (int i = 0; i < Operations; i++)
            {
                current += step;
                if (current >= maxStart)
                    current -= maxStart;

                _indices[i] = current;
            }
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            if (_raw != null)
            {
                NativeMemory.Free(_raw);
                _raw = null;
                _aligned = null;
                _misaligned = null;
            }
        }

        [Benchmark]
        public float RegularAlignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _aligned;

            int limit = TotalFloats - VectorWidthAvx;
            for (int i = 0; i < limit; i += VectorWidthAvx)
            {
                Vector256<float> v = Avx.LoadVector256(ptr + i);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }

        [Benchmark]
        public float RegularMisalignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _misaligned;

            int limit = TotalFloats - VectorWidthAvx;
            for (int i = 0; i < limit; i += VectorWidthAvx)
            {
                Vector256<float> v = Avx.LoadVector256(ptr + i);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }
       
        [Benchmark]
        public float IrregularAlignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _aligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                Vector256<float> v = Avx.LoadVector256(ptr + indices[i]);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }
        
        [Benchmark]
        public float IrregularMisalignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _misaligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                Vector256<float> v = Avx.LoadVector256(ptr + indices[i]);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float HorizontalAdd256(Vector256<float> v)
        {
            float* tmp = stackalloc float[VectorWidthAvx];
            Avx.Store(tmp, v);

            float sum = 0f;
            for (int i = 0; i < VectorWidthAvx; i++)
                sum += tmp[i];

            return sum;
        }

        public static void RunAll()
        {
            BenchmarkRunner.Run<SimdAlignmentScenariosBench>();
        }
    }
}
