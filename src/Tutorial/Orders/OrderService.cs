namespace Tutorial.Orders;

// Tracks how many of each product are on the shelf.
public class Inventory
{
    private readonly Dictionary<string, int> _stock = new();

    public void Restock(string sku, int quantity)
    {
        _stock[sku] = StockOf(sku) + quantity;
    }

    // GetValueOrDefault returns 0 when the sku is missing
    public int StockOf(string sku) => _stock.GetValueOrDefault(sku);

    public void Remove(string sku, int quantity)
    {
        _stock[sku] = StockOf(sku) - quantity;
    }
}

// Turns an Order into a Receipt.
public class OrderService
{
    public const decimal TaxRate = 0.08m;

    private readonly Inventory _inventory;
    private readonly IDiscountPolicy _discounts;

    // Dependency injection: the service receives its collaborators
    // instead of creating them. Tests can pass in fakes.
    public OrderService(Inventory inventory, IDiscountPolicy discounts)
    {
        _inventory = inventory;
        _discounts = discounts;
    }

    public Receipt Checkout(Order order)
    {
        if (order.Lines.Count == 0)
        {
            throw new OrderException(order.Id, "Order is empty.");
        }

        // Check every line first, so a failure leaves the stock untouched.
        foreach (var line in order.Lines)
        {
            int available = _inventory.StockOf(line.Product.Sku);
            if (available < line.Quantity)
            {
                string name = line.Product.Name;
                string message =
                    $"Only {available} of {name} in stock, "
                    + $"{line.Quantity} requested.";
                throw new OrderException(order.Id, message);
            }
        }

        foreach (var line in order.Lines)
        {
            _inventory.Remove(line.Product.Sku, line.Quantity);
        }

        decimal subtotal = order.Subtotal;
        decimal discount = _discounts.DiscountFor(order);
        decimal tax = Math.Round((subtotal - discount) * TaxRate, 2);
        decimal total = subtotal - discount + tax;
        return new Receipt(order.Id, subtotal, discount, tax, total);
    }
}
