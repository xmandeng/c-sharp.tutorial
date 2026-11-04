namespace Tutorial.Orders;

// The data types for the mini project.

// One item for sale. A record: immutable, compared by value.
public record Product(string Sku, string Name, decimal Price);

// One row of an order: which product, how many.
public record OrderLine(Product Product, int Quantity)
{
    public decimal Subtotal => Product.Price * Quantity;
}

// An order is mutable (lines get added), so it is a class, not a record.
public class Order
{
    public string Id { get; }
    public string Customer { get; }
    public List<OrderLine> Lines { get; } = new();

    public Order(string id, string customer)
    {
        Id = id;
        Customer = customer;
    }

    public decimal Subtotal => Lines.Sum(line => line.Subtotal);

    public void Add(Product product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(quantity), "Quantity must be positive.");
        }
        Lines.Add(new OrderLine(product, quantity));
    }
}

// What checkout returns.
public record Receipt(string OrderId, decimal Subtotal,
                      decimal Discount, decimal Tax, decimal Total);

// Raised when an order cannot be completed.
public class OrderException : Exception
{
    public string OrderId { get; }

    public OrderException(string orderId, string message) : base(message)
    {
        OrderId = orderId;
    }
}
