namespace Tutorial;

// Chapter 6: arrays, List, Dictionary, HashSet, tuples.
public static class Ch06Collections
{
    public static void Run()
    {
        // Array: fixed length, one element type
        int[] scores = { 90, 72, 85 };
        Console.WriteLine($"first {scores[0]}, length {scores.Length}");
        Console.WriteLine($"last {scores[^1]}");        // Python: scores[-1]
        int[] firstTwo = scores[0..2];                  // Python: scores[0:2]
        Console.WriteLine(string.Join(", ", firstTwo));

        // List<T>: growable, like a Python list, but one element type
        var names = new List<string> { "Ada", "Grace" };
        names.Add("Linus");                             // Python: append
        names.Remove("Grace");
        Console.WriteLine($"{names.Count} names: {string.Join(", ", names)}");
        // names.Add(42);  // compile error: an int is not a string

        // Dictionary<TKey, TValue>: like a Python dict
        var ages = new Dictionary<string, int>
        {
            ["Ada"] = 36,
            ["Grace"] = 85,
        };
        ages["Linus"] = 54;                             // add or overwrite

        // ages["Nobody"] would throw KeyNotFoundException (Python: KeyError)
        if (ages.TryGetValue("Ada", out int adaAge))    // Python: ages.get("Ada")
        {
            Console.WriteLine($"Ada was {adaAge}");
        }
        foreach (var (person, age) in ages)             // Python: ages.items()
        {
            Console.WriteLine($"{person}: {age}");
        }

        // HashSet<T>: like a Python set
        var tags = new HashSet<string> { "c#", "dotnet" };
        bool added = tags.Add("c#");                    // false: already there
        Console.WriteLine($"added again? {added}; has dotnet? {tags.Contains("dotnet")}");

        // Tuple with named parts
        (string City, int Population) place = ("Oslo", 709_000);
        Console.WriteLine($"{place.City} has {place.Population} people");
    }
}
