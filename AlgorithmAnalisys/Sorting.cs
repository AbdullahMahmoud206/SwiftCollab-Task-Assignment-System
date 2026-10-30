using System;
using System.Threading.Tasks;

public class Sorting
{
    // LLM Improvement:
    // Replaced Bubble Sort (O(n²)) with Quick Sort (O(n log n) average).
    public static void QuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            QuickSort(arr, low, pivotIndex - 1);
            QuickSort(arr, pivotIndex + 1, high);
        }
    }

    // Partition method used by Quick Sort.
    private static int Partition(int[] arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] <= pivot)
            {
                i++;

                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }

        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);

        return i + 1;
    }

    // LLM Improvement:
    // Parallel Quick Sort for very large datasets.
    public static void ParallelQuickSort(int[] arr, int low, int high)
    {
        if (low < high)
        {
            int pivotIndex = Partition(arr, low, high);

            Parallel.Invoke(
                () => ParallelQuickSort(arr, low, pivotIndex - 1),
                () => ParallelQuickSort(arr, pivotIndex + 1, high)
            );
        }
    }

    public static void PrintArray(int[] arr)
    {
        foreach (var item in arr)
        {
            Console.Write(item + " ");
        }

        Console.WriteLine();
    }

    // public static void Main()
    // {
    //     int[] dataset = { 50, 20, 40, 10, 30 };

    //     Console.WriteLine("Before Sorting:");
    //     PrintArray(dataset);

    //     QuickSort(dataset, 0, dataset.Length - 1);

    //     Console.WriteLine("After Sorting:");
    //     PrintArray(dataset);
    // }
}