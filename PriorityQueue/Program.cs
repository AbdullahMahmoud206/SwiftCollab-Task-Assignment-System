// class Program
// {
//     static void Main()
//     {
//         var queue = new ApiRequestPriorityQueue();

//         queue.EnqueueBatch(new[]
//         {
//             new ApiRequest("/auth", 1),
//             new ApiRequest("/healthcheck", 2),
//             new ApiRequest("/data", 3),
//             new ApiRequest("/analytics", 4)
//         });

//         while (queue.Count > 0)
//         {
//             var request = queue.Dequeue();

//             Console.WriteLine(
//                 $"Processing: {request?.Endpoint} (Priority: {request?.Priority})");
//         }
//     }
// }