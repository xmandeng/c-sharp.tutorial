namespace Tutorial.Orders;

// Discount rules sit behind an interface, so OrderService works with any of them.
public interface IDiscountPolicy
{
    decimal DiscountFor(Order order);
}

public class NoDiscount : IDiscountPolicy
{
    public decimal DiscountFor(Order order) => 0m;
}

// A percentage off any order whose subtotal reaches a threshold.
public class BulkDiscount : IDiscountPolicy
{
    public decimal Threshold { get; }
    public decimal Rate { get; }

    public BulkDiscount(decimal threshold = 100m, decimal rate = 0.10m)
    {
        Threshold = threshold;
        Rate = rate;
    }

    public decimal DiscountFor(Order order)
    {
        if (order.Subtotal < Threshold)
        {
            return 0m;
        }
        return Math.Round(order.Subtotal * Rate, 2);
    }
}
