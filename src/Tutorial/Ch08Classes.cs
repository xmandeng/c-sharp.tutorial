namespace Tutorial;

// Chapter 8: classes, properties, records, structs.

// A class bundles data with the methods that act on it.
public class BankAccount
{
    // Static field: shared by all BankAccounts (Python: class attribute)
    private static int _nextId = 1;

    // Instance field: private storage, one per object
    private decimal _balance;

    // Read-only auto-properties: set in the constructor, never again
    public string Id { get; }
    public string Owner { get; }

    // Computed property (Python: @property)
    public decimal Balance => _balance;

    // Constructor (Python: __init__). No "self": fields are in scope directly.
    public BankAccount(string owner, decimal openingBalance = 0m)
    {
        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new ArgumentException("Owner is required.", nameof(owner));
        }
        Owner = owner;
        Id = $"ACC-{_nextId++:D4}";
        _balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount), "Deposit must be positive.");
        }
        _balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount > _balance)
        {
            throw new InvalidOperationException(
                $"Insufficient funds: balance {_balance}, requested {amount}.");
        }
        _balance -= amount;
    }

    // Python: __str__
    public override string ToString() => $"{Id} {Owner}: {Balance:0.00}";
}

// record: a data class with value equality (Python: @dataclass(frozen=True))
public record Point(int X, int Y);

// struct: a small value type, copied on assignment
public struct Temperature
{
    public double Celsius { get; set; }
    public double Fahrenheit => Celsius * 9 / 5 + 32;
}

public static class Ch08Classes
{
    public static void Run()
    {
        var account = new BankAccount("Ada", 100m);   // "new" creates an object
        account.Deposit(50m);
        account.Withdraw(30m);
        Console.WriteLine(account);                   // calls ToString()
        // account.Balance = 1_000_000m;              // compile error: no setter

        try
        {
            account.Withdraw(500m);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Refused: {ex.Message}");
        }

        var p1 = new Point(1, 2);
        var p2 = new Point(1, 2);
        Console.WriteLine(p1 == p2);          // True: records compare by value
        Console.WriteLine(p1);                // Point { X = 1, Y = 2 }
        var moved = p1 with { X = 10 };       // copy with one field changed
        Console.WriteLine(moved);

        var g1 = new BankAccount("Grace");
        var g2 = new BankAccount("Grace");
        Console.WriteLine(g1 == g2);          // False: classes compare by identity

        var t1 = new Temperature { Celsius = 20 };
        var t2 = t1;                          // struct: a full copy
        t2.Celsius = 100;
        Console.WriteLine($"t1 {t1.Celsius}C = {t1.Fahrenheit}F; t2 {t2.Celsius}C");
    }
}
