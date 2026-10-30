# SwiftCollab Performance Report

This report consolidates the performance-related findings documented in the
repository README files. The complexity statements below reflect the README
claims and describe the intended optimized designs.

## Executive Summary

The documented optimizations replace an unbalanced binary search tree with an
AVL tree, replace repeated list sorting with a binary min-heap, replace Bubble
Sort with Quick Sort, and improve task execution resilience with validation,
logging, and retries. The primary documented gains are logarithmic tree and
heap operations, average-case `O(n log n)` sorting, and continued processing
after an individual task failure.

## Component Analysis

| Component | Original bottleneck | Documented optimization | Documented result |
|---|---|---|---|
| Task priorities | A BST can become skewed and degrade to `O(n)` | AVL balancing, cached heights, iterative search | Insert/search `O(log n)`; traversal `O(n)` |
| API request queue | Sorting the full list after each enqueue; removing index `0` shifts items | Binary min-heap, batch enqueue, locking, peek | Enqueue/dequeue `O(log n)` |
| Reporting and analytics | Bubble Sort requires quadratic time | In-place Quick Sort | Average-case `O(n log n)` with low extra memory |
| Task execution | Invalid input and unhandled failures can stop processing | Validation, structured exception handling, logging, retries | Queue processing continues after individual failures |

## Annotated Code Excerpts

### Task 1: AVL tree optimization

From `AVLTree/AVLTree.cs`:

```csharp
public Node Root { get; private set; } = null!;
```

`private set` prevents callers from replacing the tree root directly, which
protects the balance invariants maintained by AVL insertion and rotations.

```csharp
public int Height { get; set; }
```

Each node caches its height so balance calculations can read subtree metadata
in `O(1)` time instead of recursively recomputing heights after every update.

```csharp
public bool Search(int value)
{
    Node current = Root;

    while (current != null)
    {
        if (value == current.Value)
            return true;

        current = value < current.Value ? current.Left : current.Right;
    }

    return false;
}
```

The iterative search follows BST ordering, avoids unrelated branches, and
reduces recursion overhead. With AVL balancing, the documented search
complexity is `O(log n)`.

```csharp
else
{
    return node;
}
```

Returning the existing node for a duplicate avoids unnecessary allocation and
prevents duplicate priorities from increasing the tree size.

### Task 2: Priority queue optimization

From `PriorityQueue/PriorityQueue.cs`:

```csharp
private readonly List<ApiRequest> _heap = new();
```

The list is used as the backing array for a binary min-heap, so the smallest
priority remains at index `0` without sorting the entire collection.

```csharp
_heap.Add(request);
HeapifyUp(_heap.Count - 1);
```

Appending and restoring the heap property requires `O(log n)` work, improving
on sorting the complete list after each enqueue.

```csharp
ApiRequest root = _heap[0];
_heap[0] = _heap[^1];
_heap.RemoveAt(_heap.Count - 1);
HeapifyDown(0);
```

Dequeue removes the minimum-priority request and restores heap order in
`O(log n)` time instead of shifting every remaining list element.

```csharp
lock (_lock)
{
    return _heap.Count > 0 ? _heap[0] : null;
}
```

Locking protects concurrent queue access, while `Peek` permits inspection
without removing the next request.

### Task 3: Sorting algorithm optimization

From `AlgorithmAnalisys/Sorting.cs`:

```csharp
public static void QuickSort(int[] arr, int low, int high)
{
    if (low < high)
    {
        int pivotIndex = Partition(arr, low, high);

        QuickSort(arr, low, pivotIndex - 1);
        QuickSort(arr, pivotIndex + 1, high);
    }
}
```

Quick Sort partitions the input in place and recursively sorts the two
subranges. Its documented average-case complexity is `O(n log n)`, compared
with Bubble Sort's `O(n²)`.

```csharp
(arr[i], arr[j]) = (arr[j], arr[i]);
```

In-place swaps avoid a second array and keep additional memory usage low.

```csharp
Parallel.Invoke(
    () => ParallelQuickSort(arr, low, pivotIndex - 1),
    () => ParallelQuickSort(arr, pivotIndex + 1, high)
);
```

The parallel variant can process independent partitions concurrently for very
large datasets, although the README notes that parallel overhead may not
benefit smaller inputs.

### Task 4: Task execution debugging and error handling

From `TaskExecutor/TaskExecutor.cs`:

```csharp
if (string.IsNullOrWhiteSpace(task))
{
    LogError("Attempted to add a null or empty task.");
    return;
}
```

Validation prevents invalid tasks from entering the queue and reports the
rejected input through the centralized logging path.

```csharp
while (!completed && attempts <= MaxRetries)
{
    try
    {
        attempts++;
        ExecuteTask(task);
        completed = true;
    }
    catch (Exception ex)
    {
        LogError(
            $"Task '{task}' failed on attempt {attempts}. Error: {ex.Message}");
    }
}
```

Structured exception handling retries transient failures while keeping the
outer queue-processing loop alive, so one failed task does not stop later
tasks from being attempted.

```csharp
if (task.Contains("Fail", StringComparison.OrdinalIgnoreCase))
{
    throw new InvalidOperationException("Task execution failed.");
}
```

The specific exception type makes the simulated execution failure clearer to
diagnose than a generic exception.

## Complexity Summary

| Operation | Original design | Optimized design |
|---|---:|---:|
| BST insert | `O(n)` worst case | AVL insert `O(log n)` |
| BST search | `O(n)` worst case | AVL search `O(log n)` |
| Tree traversal | `O(n)` | `O(n)` |
| Priority enqueue | `O(n log n)` according to the README | Heap enqueue `O(log n)` |
| Priority dequeue | `O(n)` according to the README | Heap dequeue `O(log n)` |
| Bubble Sort | `O(n²)` | Quick Sort average `O(n log n)` |
| Task execution | `O(1)` per attempt | `O(r)` retry handling, where `r` is retry count |

## Operational Considerations

- AVL and heap performance depends on maintaining their structural invariants.
- Quick Sort has a worst-case `O(n²)` path if pivot selection is poor; the
  README documents its average-case complexity.
- Locking improves safety for concurrent queue access but can reduce
  concurrency under contention.
- Retries improve resilience but should remain bounded and configurable.
- The README files describe intended outcomes; benchmark measurements and
  workload-specific latency data are not included in the repository.

## Source README Files

- `README.md`
- `AVLTree/README.md`
- `PriorityQueue/README.md`
- `AlgorithmAnalisys/README.md`
- `TaskExecutor/README.md`
