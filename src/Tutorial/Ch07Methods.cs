namespace Tutorial;

// Chapter 7: methods (C#'s name for functions), parameters, lambdas.
public static class Ch07Methods
{
    // access  static  return-type  Name(type param, type param)
    public static int Add(int a, int b)
    {
        return a + b;
    }

    // Overload: same name, different parameter types
    public static double Add(double a, double b) => a + b;

    // Expression-bodied method: one expression, no braces, no return
    public static int Square(int x) => x * x;

    // Optional parameter with a default, as in Python
    public static string Greet(string name, string greeting = "Hello") =>
        $"{greeting}, {name}!";

    // out parameter: hand back a second value
    public static bool TryDivide(int a, int b, out int result)
    {
        if (b == 0)
        {
            result = 0;
            return false;
        }
        result = a / b;
        return true;
    }

    // Or return a tuple, as Python would
    public static (int Min, int Max) MinMax(List<int> values) =>
        (values.Min(), values.Max());

    // params: any number of arguments (Python: *args)
    public static int Sum(params int[] numbers)
    {
        int total = 0;
        foreach (int n in numbers)
        {
            total += n;
        }
        return total;
    }

    // ref: the method can change the caller's variable
    public static void DoubleInPlace(ref int x) => x *= 2;

    public static void Run()
    {
        Console.WriteLine(Add(2, 3)); // int version
        Console.WriteLine(Add(2.5, 0.25)); // double version
        Console.WriteLine(Square(4));
        Console.WriteLine(Greet("Ada"));
        Console.WriteLine(Greet("Ada", greeting: "Welcome")); // named argument

        if (TryDivide(10, 3, out int quotient))
        {
            Console.WriteLine($"10 / 3 = {quotient}");
        }

        var (lo, hi) = MinMax(new List<int> { 4, 1, 9 }); // unpack a tuple
        Console.WriteLine($"min {lo}, max {hi}");
        Console.WriteLine(Sum(1, 2, 3, 4));

        int value = 5;
        DoubleInPlace(ref value);
        Console.WriteLine($"value is now {value}");

        // Lambdas. Func<int, int>: takes an int, returns an int.
        // Action<string>: takes a string, returns nothing.
        Func<int, int> triple = x => x * 3; // Python: lambda x: x * 3
        Action<string> shout = s => Console.WriteLine(s.ToUpperInvariant());
        Console.WriteLine(triple(5));
        shout("done");
    }
}
