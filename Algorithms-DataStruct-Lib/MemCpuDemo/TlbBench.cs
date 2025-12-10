using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.TlbDemo
{
    [MemoryDiagnoser]
    public class TlbRandomPagesBench
    {
        private const int PageSize    = 4096;
        private const int Operations  = 50_000_000; // more ops → меньше шума

        private byte[] _buffer = null!;
        private int[] _randomPages = null!; // precomputed random page indices in [0, PagesToTouch)

        // Сколько разных страниц входит в рабочий набор.
        // Ожидаем строгий рост времени:
        // 1 < 64 < 256 < 1024 < 4096 < 16384
        [Params(1, 64, 256, 1024, 4096, 16384)]
        public int PagesToTouch { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            // Рабочий набор страниц = PagesToTouch.
            // Размер буфера — ровно столько страниц.
            _buffer = new byte[PagesToTouch * PageSize];

            // Инициализируем буфер детерминированными данными
            for (int i = 0; i < _buffer.Length; i++)
                _buffer[i] = (byte)(i * 37 + 11);

            // Предварительно считаем последовательность случайных страниц
            // в диапазоне [0, PagesToTouch).
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

            // Выполняем одинаковое количество обращений во всех сценариях.
            //
            // Для каждого обращения:
            //   - берём заранее сгенерированный случайный номер страницы,
            //   - читаем один байт в начале этой страницы.
            //
            // По мере роста PagesToTouch:
            //   - рабочий набор охватывает всё больше страниц,
            //   - TLB должен держать всё больше трансляций,
            //   - растёт число промахов → растёт число page walks → растёт средняя латентность.
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
