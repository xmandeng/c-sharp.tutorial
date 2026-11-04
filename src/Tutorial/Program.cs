// Entry point. Demo numbers match the textbook's chapter numbers (3 to 13).
// This file uses "top-level statements": you do not write
// a class or a Main method here; the compiler generates them for you.
//
//   dotnet run                run every chapter
//   dotnet run -- 8           run the demo for book chapter 8 only
//   dotnet run -- 4 10        run chapters 4 and 10
using Tutorial;

var titles = new Dictionary<int, string>
{
    [3] = "Hello, C#",
    [4] = "Types and variables",
    [5] = "Control flow",
    [6] = "Collections",
    [7] = "Methods",
    [8] = "Classes, records, structs",
    [9] = "Inheritance and interfaces",
    [10] = "LINQ",
    [11] = "Exceptions and using",
    [12] = "Async and await",
    [13] = "Mini project: orders",
};

// args is the command-line arguments, like Python's sys.argv[1:]
IEnumerable<int> toRun = args.Length > 0 ? args.Select(int.Parse) : titles.Keys;

foreach (int chapter in toRun)
{
    Console.WriteLine($"\n=== Chapter {chapter}: {titles[chapter]} ===");
    switch (chapter)
    {
        case 3: Ch03Basics.Run(); break;
        case 4: Ch04Types.Run(); break;
        case 5: Ch05ControlFlow.Run(); break;
        case 6: Ch06Collections.Run(); break;
        case 7: Ch07Methods.Run(); break;
        case 8: Ch08Classes.Run(); break;
        case 9: Ch09Interfaces.Run(); break;
        case 10: Ch10Linq.Run(); break;
        case 11: Ch11Exceptions.Run(); break;
        case 12: await Ch12Async.RunAsync(); break;
        case 13: Ch13Orders.Run(); break;
    }
}
