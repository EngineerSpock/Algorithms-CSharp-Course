using System;
using System.Diagnostics;
using System.Numerics;

class SimdOneDimArray
{
    const int N = 8000000; // Должно быть кратно 4 или 8 для SIMD

    static void NormalSum(float[] a, float[] b, float[] result)
    {
        for (int i = 0; i < N; i++)
        {
            result[i] = a[i] + b[i]; // Обычное поэлементное сложение
        }
    }

    static void SimdSum(float[] a, float[] b, float[] result)
    {
        // Количество элементов в SIMD-регистре (4, 8, 16)
        int vectorSize = Vector<float>.Count; 
        int i = 0;
        for (; i <= N - vectorSize; i += vectorSize)
        {
            var va = new Vector<float>(a, i);
            var vb = new Vector<float>(b, i);
            var vr = va + vb; // SIMD-сложение
            vr.CopyTo(result, i);
        }

        // Оставшиеся элементы (если N не кратно vectorSize)
        for (; i < N; i++)
        {
            result[i] = a[i] + b[i];
        }
    }

    static void RunDemo()
    {
        float[] a = new float[N];
        float[] b = new float[N];
        float[] result1 = new float[N];
        float[] result2 = new float[N];

        for (int i = 0; i < N; i++)
        {
            a[i] = 1.0f;
            b[i] = 2.0f;
        }

        Stopwatch sw = Stopwatch.StartNew();
        NormalSum(a, b, result1);
        sw.Stop();
        Console.WriteLine(
$"Обычное сложение: {sw.Elapsed.TotalSeconds} сек");

        sw.Restart();
        SimdSum(a, b, result2);
        sw.Stop();
        Console.WriteLine(
$"SIMD сложение (Vector<T>): {sw.Elapsed.TotalSeconds} сек");
    }
}

public class SimdTwoDimArray
{
    private const int Size = 8000000;
    private static int[,] matrix = new int[Size, Size];

    static void RunDemo()
    {
        // Инициализация матрицы
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                matrix[i, j] = i + j;

        long rowSum = SumRowWiseSimd();
        Console.WriteLine($"SIMD row-wise sum: {rowSum}");
    }

    static long SumRowWiseSimd()
    {
        long sum = 0;
        Vector<int> vectorSum = Vector<int>.Zero;  // Вектор для накопления суммы

        for (int i = 0; i < Size; i++)
        {
            int[] row = GetRow(matrix, i);  // Получаем строку как массив

            int vectorLength = Vector<int>.Count;  // Сколько int помещается в вектор (обычно 4)
            int j = 0;

            // Обрабатываем блоками по размеру вектора
            for (; j <= row.Length - vectorLength; j += vectorLength)
            {
                Vector<int> v = new Vector<int>(row, j);
                vectorSum += v;
            }

            // Остаток (если длина строки не кратна vectorLength)
            for (; j < row.Length; j++)
            {
                sum += row[j];
            }
        }

        // Суммируем элементы вектора в скалярное значение
        sum += Vector.Dot(vectorSum, Vector<int>.One);

        return sum;
    }

    // Вспомогательный метод: извлекает строку матрицы как массив
    static int[] GetRow(int[,] matrix, int rowIndex)
    {
        int cols = matrix.GetLength(1);
        int[] row = new int[cols];
        for (int j = 0; j < cols; j++)
            row[j] = matrix[rowIndex, j];
        return row;
    }
}
