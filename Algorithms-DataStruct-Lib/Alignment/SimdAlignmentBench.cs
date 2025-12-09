using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.Alignment
{
    [MemoryDiagnoser]
    public unsafe class SimdAlignmentScenariosBench
    {
        private const int CacheLineBytes  = 64;
        private const int VectorWidthSse  = 4;  // 4 floats = 16 bytes
        private const int VectorWidthAvx  = 8;  // 8 floats = 32 bytes

        // Размер массива (в float); достаточно большой для нерегулярных паттернов,
        // но ещё не "гигантский". Для сценария "очень большой массив" потом можно
        // отдельно сделать параметр или другой класс.
        private const int TotalFloats = 1_000_000 + 64;

        // Число операций для нерегулярного доступа
        private const int Operations = 1_000_000;

        private void*  _raw;           // исходный указатель из NativeMemory.Alloc
        private float* _baseAligned;   // выровненный по 64 байтам
        private float* _baseMisaligned; // намеренно смещённый на 1 float (4 байта)

        private int[] _indices;        // нерегулярные индексы (общие для SSE/AVX)

        [GlobalSetup]
        public void Setup()
        {
            if (!Sse.IsSupported)
                throw new PlatformNotSupportedException("SSE is not supported on this CPU.");

            if (!Avx.IsSupported)
                throw new PlatformNotSupportedException("AVX is not supported on this CPU.");

            // Немного переразмерим память, чтобы гарантированно хватило для
            // выравнивания и всех чтений.
            int extraFloats   = 32;
            nuint totalFloats = (nuint)(TotalFloats + extraFloats);
            nuint totalBytes  = totalFloats * (nuint)sizeof(float);

            _raw = NativeMemory.Alloc(totalBytes);
            if (_raw == null)
                throw new OutOfMemoryException("NativeMemory.Alloc returned null");

            // Выровняем базу по 64 байтам (размер кеш-линии).
            nint addr        = (nint)_raw;
            nint alignedAddr = (addr + (CacheLineBytes - 1)) & ~(CacheLineBytes - 1);
            _baseAligned     = (float*)alignedAddr;

            // Намеренно невыравненный базовый указатель: смещение на 1 float (4 байта).
            _baseMisaligned = _baseAligned + 1;

            // Инициализация данных.
            for (int i = 0; i < TotalFloats; i++)
                _baseAligned[i] = i * 0.001f;

            // Готовим нерегулярные индексы.
            // Для AVX нам нужно, чтобы при смещении idx мы могли прочитать 8 float подряд.
            _indices = new int[Operations];
            int maxStart = TotalFloats - VectorWidthAvx;

            int current    = 0;
            const int step = 37; // псевдослучайный шаг для "разброса" по массиву

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
                _baseAligned = null;
                _baseMisaligned = null;
            }
        }

        // =====================================================================
        // 1. REGULAR ACCESS + SSE (aligned / misaligned)
        // =====================================================================

        [Benchmark]
        public float RegularAlignedSse()
        {
            Vector128<float> acc = Vector128<float>.Zero;
            float* ptr = _baseAligned;

            // Последовательный проход по массиву блоками по 4 float.
            int limit = TotalFloats - VectorWidthSse;
            for (int i = 0; i < limit; i += VectorWidthSse)
            {
                Vector128<float> v = Sse.LoadVector128(ptr + i);
                acc = Sse.Add(acc, v);
            }

            return HorizontalAdd128(acc);
        }

        [Benchmark]
        public float RegularMisalignedSse()
        {
            Vector128<float> acc = Vector128<float>.Zero;
            float* ptr = _baseMisaligned;

            int limit = TotalFloats - VectorWidthSse;
            for (int i = 0; i < limit; i += VectorWidthSse)
            {
                Vector128<float> v = Sse.LoadVector128(ptr + i);
                acc = Sse.Add(acc, v);
            }

            return HorizontalAdd128(acc);
        }

        // =====================================================================
        // 2. IRREGULAR ACCESS + SSE (aligned / misaligned)
        // =====================================================================

        [Benchmark]
        public float IrregularAlignedSse()
        {
            Vector128<float> acc = Vector128<float>.Zero;
            float* ptr = _baseAligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                int idx = indices[i];
                Vector128<float> v = Sse.LoadVector128(ptr + idx);
                acc = Sse.Add(acc, v);
            }

            return HorizontalAdd128(acc);
        }

        [Benchmark]
        public float IrregularMisalignedSse()
        {
            Vector128<float> acc = Vector128<float>.Zero;
            float* ptr = _baseMisaligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                int idx = indices[i];
                Vector128<float> v = Sse.LoadVector128(ptr + idx);
                acc = Sse.Add(acc, v);
            }

            return HorizontalAdd128(acc);
        }

        // =====================================================================
        // 3. REGULAR ACCESS + AVX (aligned / misaligned)
        // =====================================================================

        [Benchmark]
        public float RegularAlignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _baseAligned;

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
            float* ptr = _baseMisaligned;

            int limit = TotalFloats - VectorWidthAvx;
            for (int i = 0; i < limit; i += VectorWidthAvx)
            {
                Vector256<float> v = Avx.LoadVector256(ptr + i);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }

        // =====================================================================
        // 4. IRREGULAR ACCESS + AVX (aligned / misaligned)
        // =====================================================================

        [Benchmark]
        public float IrregularAlignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _baseAligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                int idx = indices[i];
                Vector256<float> v = Avx.LoadVector256(ptr + idx);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }

        [Benchmark]
        public float IrregularMisalignedAvx()
        {
            Vector256<float> acc = Vector256<float>.Zero;
            float* ptr = _baseMisaligned;
            int[] indices = _indices;

            for (int i = 0; i < indices.Length; i++)
            {
                int idx = indices[i];
                Vector256<float> v = Avx.LoadVector256(ptr + idx);
                acc = Avx.Add(acc, v);
            }

            return HorizontalAdd256(acc);
        }

        // =====================================================================
        // Вспомогательные методы
        // =====================================================================

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float HorizontalAdd128(Vector128<float> v)
        {
            float* tmp = stackalloc float[4];
            Sse.Store(tmp, v);
            return tmp[0] + tmp[1] + tmp[2] + tmp[3];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float HorizontalAdd256(Vector256<float> v)
        {
            float* tmp = stackalloc float[8];
            Avx.Store(tmp, v);
            return tmp[0] + tmp[1] + tmp[2] + tmp[3]
                 + tmp[4] + tmp[5] + tmp[6] + tmp[7];
        }

        // Удобный статический запуск для Program.Main
        public static void RunAll()
        {
            BenchmarkRunner.Run<SimdAlignmentScenariosBench>();
        }
    }
}
