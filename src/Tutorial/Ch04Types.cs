namespace Tutorial;

// Chapter 4: every variable has a type, fixed at compile time.
public static class Ch04Types
{
    public static void Run()
    {
        int count = 3; // 32-bit whole number
        long big = 9_000_000_000L; // 64-bit whole number
        double ratio = 0.1 + 0.2; // binary floating point, like Python float
        decimal price = 0.1m + 0.2m; // base-10 number, exact for money
        bool done = false; // lowercase true/false
        char initial = 'A'; // single quotes = one character
        string greeting = "hi"; // double quotes = string

        Console.WriteLine($"double: {ratio}   decimal: {price}");
        Console.WriteLine($"{count} {big} {done} {initial} {greeting}");

        // var asks the compiler to infer the type. The type is still fixed.
        var total = count * 2; // total is an int
        // total = "six";            // compile error: cannot convert string to int
        Console.WriteLine($"total is an {total.GetType().Name}");

        // int / int is integer division (Python: //)
        Console.WriteLine($"7 / 2 = {7 / 2}, 7 / 2.0 = {7 / 2.0}");

        // Converting between types
        int parsed = int.Parse("42"); // throws on bad input
        bool ok = int.TryParse("abc", out int maybe); // returns false instead
        double widened = count; // implicit: no data lost
        int narrowed = (int)3.99; // explicit cast: 3
        Console.WriteLine($"{parsed} {ok} {maybe} {widened} {narrowed}");

        // Value types (int, double, struct) are copied on assignment.
        int a = 1;
        int b = a; // b gets a copy of a's value
        b += 98; // changes only b
        Console.WriteLine($"a = {a}, b = {b}"); // a is still 1

        // Reference types (classes, List, arrays) share one object.
        var listA = new List<int> { 1, 2 };
        var listB = listA;
        listB.Add(3);
        Console.WriteLine($"listA.Count = {listA.Count}"); // 3

        // null and nullable types
        int? missing = null; // int? = int that may be null
        Console.WriteLine(missing ?? -1); // ?? = "if null, use this"
        string? maybeName = null; // string? = may be null
        Console.WriteLine(maybeName?.Length ?? 0); // ?. = stop if null
    }
}
