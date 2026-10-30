using System;
using System.Collections.Generic;

public class TaskExecutor
{
    private readonly Queue<string> taskQueue = new();

    // LLM Improvement:
    // Added configurable retry count for failed tasks.
    private const int MaxRetries = 2;

    public void AddTask(string task)
    {
        // LLM Improvement:
        // Prevent invalid tasks from entering the queue.
        if (string.IsNullOrWhiteSpace(task))
        {
            LogError("Attempted to add a null or empty task.");
            return;
        }

        taskQueue.Enqueue(task);
    }

    public void ProcessTasks()
    {
        while (taskQueue.Count > 0)
        {
            string task = taskQueue.Dequeue();

            Console.WriteLine($"Processing task: {task}");

            int attempts = 0;
            bool completed = false;

            while (!completed && attempts <= MaxRetries)
            {
                try
                {
                    attempts++;

                    ExecuteTask(task);

                    completed = true;

                    Console.WriteLine(
                        $"Task '{task}' completed successfully.");
                }
                catch (Exception ex)
                {
                    LogError(
                        $"Task '{task}' failed on attempt {attempts}. Error: {ex.Message}");

                    if (attempts > MaxRetries)
                    {
                        LogError(
                            $"Task '{task}' exceeded maximum retry attempts.");
                    }
                }
            }
        }
    }

    private void ExecuteTask(string task)
    {
        // LLM Improvement:
        // Added validation to prevent null reference issues.
        if (string.IsNullOrWhiteSpace(task))
        {
            throw new ArgumentException("Task cannot be null or empty.");
        }

        // Simulated failure scenario.
        if (task.Contains("Fail", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Task execution failed.");
        }
    }

    // LLM Improvement:
    // Centralized logging method.
    private void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(
            $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ERROR: {message}");
        Console.ResetColor();
    }
}

// class Program
// {
//     static void Main()
//     {
//         TaskExecutor executor = new();

//         executor.AddTask("Task 1");
//         executor.AddTask(null);          // Rejected safely
//         executor.AddTask("Fail Task");   // Retried before failing
//         executor.AddTask("Task 2");

//         executor.ProcessTasks();
//     }
// }