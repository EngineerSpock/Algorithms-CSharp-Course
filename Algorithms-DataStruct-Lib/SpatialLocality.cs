using System;

public class SpatialLocality
{
    // Example matrix (1000x1000 for demonstration)
    private const int Size = 1000;
    private static int[,] matrix = new int[Size, Size];

    static void RunDemo()
    {
        // Initialize matrix with sample values
        for (int i = 0; i < Size; i++)
            for (int j = 0; j < Size; j++)
                matrix[i, j] = i + j;

        // Measure row-wise summation
        long rowSum = SumRowWise();
        Console.WriteLine($"Row-wise sum: {rowSum}");

        // Measure column-wise summation  
        long colSum = SumColumnWise();
        Console.WriteLine($"Column-wise sum: {colSum}");
    }

    // Efficient: Leverages spatial locality (row-major order)
    static long SumRowWise()
    {
        long sum = 0;
        for (int i = 0; i < Size; i++)      // Outer loop: rows
            for (int j = 0; j < Size; j++)  // Inner loop: columns
                sum += matrix[i, j];
        return sum;
    }

    // Inefficient: Poor cache utilization (jumps across memory)
    static long SumColumnWise()
    {
        long sum = 0;
        for (int j = 0; j < Size; j++)      // Outer loop: columns  
            for (int i = 0; i < Size; i++)  // Inner loop: rows
                sum += matrix[i, j];
        return sum;
    }
}
