namespace Tutorial;

// Chapter 5: if, switch, and loops.
public static class Ch05ControlFlow
{
    public static void Run()
    {
        int score = 83;

        if (score >= 90)
        {
            Console.WriteLine("A");
        }
        else if (score >= 80) // Python: elif
        {
            Console.WriteLine("B");
        }
        else
        {
            Console.WriteLine("C or below");
        }

        // && || !  instead of  and or not
        bool passed = score >= 60 && score <= 100;

        // Ternary: condition ? a : b      (Python: a if condition else b)
        string label = passed ? "pass" : "fail";
        Console.WriteLine(label);

        // switch expression: picks a value by pattern (like Python's match)
        string grade = score switch
        {
            >= 90 => "A",
            >= 80 => "B",
            >= 70 => "C",
            _ => "F", // _ = anything else
        };
        Console.WriteLine($"grade {grade}");

        // Classic for loop: start; keep going while; step
        // Python: for i in range(3):
        for (int i = 0; i < 3; i++)
        {
            Console.Write($"{i} ");
        }
        Console.WriteLine();

        // foreach is Python's for ... in ...
        string[] fruits = { "apple", "banana", "cherry" };
        foreach (string fruit in fruits)
        {
            if (fruit == "banana")
            {
                continue;
            }
            Console.WriteLine(fruit);
        }

        int n = 0;
        while (n < 3)
        {
            n++; // same as n += 1
        }
        Console.WriteLine($"n = {n}");
    }
}
