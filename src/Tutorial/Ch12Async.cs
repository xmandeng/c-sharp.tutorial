using System.Diagnostics;

namespace Tutorial;

// Chapter 12: async and await.
public static class Ch12Async
{
    // async method returning Task<string> (Python: async def fetch(...) -> str)
    public static async Task<string> FetchAsync(string name, int delayMs)
    {
        await Task.Delay(delayMs); // Python: await asyncio.sleep(...)
        return $"{name} finished after {delayMs} ms";
    }

    public static async Task RunAsync()
    {
        var timer = Stopwatch.StartNew();

        // One after the other: about 600 ms in total
        string first = await FetchAsync("first", 300);
        string second = await FetchAsync("second", 300);
        long ms = timer.ElapsedMilliseconds / 100 * 100; // round down to 100s
        Console.WriteLine($"sequential took ~{ms} ms");

        timer.Restart();

        // Both at once: about 300 ms in total.
        // Calling an async method STARTS it.
        // (A Python coroutine does nothing until awaited.)
        Task<string> a = FetchAsync("A", 300);
        Task<string> b = FetchAsync("B", 300);
        string[] results = await Task.WhenAll(a, b); // Python: asyncio.gather
        ms = timer.ElapsedMilliseconds / 100 * 100;
        Console.WriteLine($"concurrent took ~{ms} ms");

        foreach (var line in new[] { first, second }.Concat(results))
        {
            Console.WriteLine(line);
        }
    }
}
