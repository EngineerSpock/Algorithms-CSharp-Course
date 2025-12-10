using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace Algorithms_DataStruct_Lib.MemCpuDemo
{
    public class SpatialLocalityBench
    {
        private const int Size = 1000;
        private int[,] _matrix;

        [GlobalSetup]
        public void Setup()
        {
            _matrix = new int[Size, Size];

            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    _matrix[i, j] = i + j;
        }

        [Benchmark]
        public long SumRowWise()
        {
            long sum = 0;
            var matrix = _matrix;

            for (int i = 0; i < Size; i++)          
                for (int j = 0; j < Size; j++)    
                    sum += matrix[i, j];

            return sum;
        }

        [Benchmark]
        public long SumColumnWise()
        {
            long sum = 0;
            var matrix = _matrix;

            for (int j = 0; j < Size; j++)          
                for (int i = 0; i < Size; i++)      
                    sum += matrix[i, j];

            return sum;
        }

        public static void Run()
        {
            BenchmarkRunner.Run<SpatialLocalityBench>();
        }
    }
}
