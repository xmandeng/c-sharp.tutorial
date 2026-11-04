namespace Tutorial;

// Chapter 10: LINQ, C#'s answer to list comprehensions and itertools.

public record Employee(string Name, string Dept, decimal Salary, int Years);

// Extension method: a static method you call as if it belonged to the type.
// The "this" before the first parameter is what makes it one.
// LINQ's Where, Select, OrderBy are all extension methods on IEnumerable<T>.
public static class EmployeeExtensions
{
    public static decimal TotalSalary(this IEnumerable<Employee> staff)
        => staff.Sum(e => e.Salary);
}

public static class Ch10Linq
{
    public static readonly List<Employee> Staff = new()
    {
        new("Ada", "Engineering", 120_000m, 5),
        new("Grace", "Engineering", 135_000m, 9),
        new("Linus", "Engineering", 98_000m, 2),
        new("Margaret", "Sales", 87_000m, 4),
        new("Alan", "Sales", 91_000m, 0),
        new("Barbara", "Finance", 105_000m, 7),
    };

    public static void Run()
    {
        // Python: [e.name for e in staff if e.salary > 100_000]
        List<string> highEarners = Staff
            .Where(e => e.Salary > 100_000m)
            .Select(e => e.Name)
            .ToList();
        Console.WriteLine($"High earners: {string.Join(", ", highEarners)}");

        // Python: sorted(staff, key=lambda e: (-e.years, e.name))
        var bySeniority = Staff
            .OrderByDescending(e => e.Years)
            .ThenBy(e => e.Name);
        Console.WriteLine($"Most senior: {bySeniority.First().Name}");

        // Aggregates
        decimal payroll = Staff.Sum(e => e.Salary);
        double avgYears = Staff.Average(e => e.Years);
        bool anyNewHires = Staff.Any(e => e.Years == 0);
        Employee? firstInHr = Staff.FirstOrDefault(e => e.Dept == "HR");  // null if none
        Console.WriteLine($"Payroll {payroll}, avg years {avgYears:F1}, " +
                          $"new hires? {anyNewHires}, HR? {firstInHr?.Name ?? "none"}");

        // GroupBy, then shape each group into a new anonymous object
        var byDept = Staff
            .GroupBy(e => e.Dept)
            .Select(g => new
            {
                Dept = g.Key,
                Count = g.Count(),
                Avg = g.Average(e => e.Salary),
            })
            .OrderBy(x => x.Dept);
        foreach (var d in byDept)
        {
            Console.WriteLine($"{d.Dept,-12} {d.Count} people, avg {d.Avg:0}");
        }

        // Deferred execution: a query runs when you loop over it, and again
        // on every later loop. (Lazy like a Python generator, but reusable.)
        var numbers = new List<int> { 1, 2, 3 };
        var evens = numbers.Where(n => n % 2 == 0);  // nothing runs yet
        numbers.Add(4);
        Console.WriteLine($"evens: {string.Join(",", evens)}");  // 2,4

        // Query syntax: the same thing written SQL-style
        var engineers = from e in Staff
                        where e.Dept == "Engineering"
                        orderby e.Salary descending
                        select e.Name;
        Console.WriteLine($"Engineers by pay: {string.Join(", ", engineers)}");

        // Calling the extension method defined above
        decimal engPayroll = Staff.Where(e => e.Dept == "Engineering").TotalSalary();
        Console.WriteLine($"Engineering payroll: {engPayroll}");
    }
}
