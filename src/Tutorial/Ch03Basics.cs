namespace Tutorial;

// Chapter 3: the shape of a C# file.
public static class Ch03Basics
{
    public static void Run()
    {
        Console.WriteLine("Hello, C#!");

        string name = "Ada";
        int year = 1843;

        // $"..." is an interpolated string, like Python's f"..."
        Console.WriteLine($"{name} wrote the first program in {year}.");

        // Format specifiers go after a colon. F2 = two decimal places.
        // Python: f"{pi:.2f}"
        double pi = Math.PI;
        Console.WriteLine($"pi to 2 places: {pi:F2}");

        // Statements end with ';'. Blocks use { } instead of indentation.
        if (year < 1900)
        {
            Console.WriteLine("That was a long time ago.");
        }
    }
}
