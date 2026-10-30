using System;
using System.Collections.Generic;

public class ApiRequest
{
    public string Endpoint { get; }
    public int Priority { get; }

    public ApiRequest(string endpoint, int priority)
    {
        Endpoint = endpoint;
        Priority = priority;
    }
}

public class ApiRequestPriorityQueue
{
    // LLM Improvement:
    // Replaced List.Sort() implementation with a Binary Min-Heap.
    private readonly List<ApiRequest> _heap = new();

    // LLM Improvement:
    // Added thread safety for concurrent API processing.
    private readonly object _lock = new();

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _heap.Count;
            }
        }
    }

    public void Enqueue(ApiRequest request)
    {
        lock (_lock)
        {
            _heap.Add(request);

            // Restore heap order.
            HeapifyUp(_heap.Count - 1);
        }
    }

    // LLM Improvement:
    // Bulk insert support for large workloads.
    public void EnqueueBatch(IEnumerable<ApiRequest> requests)
    {
        lock (_lock)
        {
            foreach (var request in requests)
            {
                _heap.Add(request);
                HeapifyUp(_heap.Count - 1);
            }
        }
    }

    public ApiRequest? Dequeue()
    {
        lock (_lock)
        {
            if (_heap.Count == 0)
                return null;

            ApiRequest root = _heap[0];

            _heap[0] = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);

            if (_heap.Count > 0)
                HeapifyDown(0);

            return root;
        }
    }

    // LLM Improvement:
    // Allows inspection without removing the request.
    public ApiRequest? Peek()
    {
        lock (_lock)
        {
            return _heap.Count > 0
                ? _heap[0]
                : null;
        }
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;

            if (_heap[parent].Priority <= _heap[index].Priority)
                break;

            (_heap[parent], _heap[index]) =
                (_heap[index], _heap[parent]);

            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            int smallest = index;

            if (left < _heap.Count &&
                _heap[left].Priority < _heap[smallest].Priority)
            {
                smallest = left;
            }

            if (right < _heap.Count &&
                _heap[right].Priority < _heap[smallest].Priority)
            {
                smallest = right;
            }

            if (smallest == index)
                break;

            (_heap[index], _heap[smallest]) =
                (_heap[smallest], _heap[index]);

            index = smallest;
        }
    }
}

