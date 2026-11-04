using Tutorial.Orders;

namespace Tutorial;

// Chapter 11: try/catch/finally, custom exceptions, using.
public static class Ch11Exceptions
{
    public static int ParseQuantity(string text)
    {
        if (!int.TryParse(text, out int qty))
        {
            // throw = Python's raise
            throw new FormatException($"'{text}' is not a whole number.");
        }
        if (qty < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(text),
                "Quantity cannot be negative."
            );
        }
        return qty;
    }

    public static void Run()
    {
        foreach (var input in new[] { "5", "abc", "-2" })
        {
            try
            {
                Console.WriteLine($"parsed {ParseQuantity(input)}");
            }
            catch (FormatException ex) // Python: except ValueError as ex
            {
                Console.WriteLine($"bad format: {ex.Message}");
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"out of range: parameter '{ex.ParamName}'");
            }
            finally // always runs
            {
                Console.WriteLine($"  (checked '{input}')");
            }
        }

        // A custom exception type with an extra property
        try
        {
            throw new OrderException("ORD-7", "Out of stock.");
        }
        catch (OrderException ex)
        {
            Console.WriteLine($"order {ex.OrderId} failed: {ex.Message}");
        }

        // using = Python's with. Dispose() runs when the block ends, even on error.
        using (var log = new TempLog("demo"))
        {
            log.Write("inside the using block");
        }
    }
}

// IDisposable is the interface "using" relies on (Python: __enter__/__exit__)
public sealed class TempLog : IDisposable
{
    private readonly string _name;

    public TempLog(string name)
    {
        _name = name;
        Console.WriteLine($"open {_name}");
    }

    public void Write(string message) =>
        Console.WriteLine($"[{_name}] {message}");

    public void Dispose() => Console.WriteLine($"close {_name}");
}
