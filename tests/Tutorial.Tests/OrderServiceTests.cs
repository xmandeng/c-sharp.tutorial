using Tutorial.Orders;

namespace Tutorial.Tests;

public class OrderServiceTests
{
    private static readonly Product Mouse = new("MS-02", "Mouse", 25.00m);
    private static readonly Product Monitor = new("MN-03", "Monitor", 180.00m);

    // Helper: a service with 5 of each product and the given discount rule
    private static (OrderService Service, Inventory Stock) Build(
        IDiscountPolicy discounts)
    {
        var stock = new Inventory();
        stock.Restock(Mouse.Sku, 5);
        stock.Restock(Monitor.Sku, 5);
        return (new OrderService(stock, discounts), stock);
    }

    [Fact]
    public void Checkout_SmallOrder_AddsTaxOnly()
    {
        var (service, _) = Build(new BulkDiscount());
        var order = new Order("T-1", "Ada");
        order.Add(Mouse, 2);                    // 50.00

        Receipt receipt = service.Checkout(order);

        Assert.Equal(0m, receipt.Discount);
        Assert.Equal(4.00m, receipt.Tax);       // 8% of 50
        Assert.Equal(54.00m, receipt.Total);
    }

    [Fact]
    public void Checkout_LargeOrder_GetsBulkDiscount()
    {
        var (service, _) = Build(new BulkDiscount(threshold: 100m, rate: 0.10m));
        var order = new Order("T-2", "Grace");
        order.Add(Monitor, 1);                  // 180.00

        Receipt receipt = service.Checkout(order);

        Assert.Equal(18.00m, receipt.Discount);
        Assert.Equal(174.96m, receipt.Total);   // (180 - 18) * 1.08
    }

    [Fact]
    public void Checkout_NotEnoughStock_ThrowsAndLeavesStockAlone()
    {
        var (service, stock) = Build(new NoDiscount());
        var order = new Order("T-3", "Linus");
        order.Add(Mouse, 1);
        order.Add(Monitor, 99);

        var ex = Assert.Throws<OrderException>(() => service.Checkout(order));

        Assert.Equal("T-3", ex.OrderId);
        Assert.Equal(5, stock.StockOf(Mouse.Sku));   // the mouse was not removed
    }

    [Fact]
    public void Checkout_EmptyOrder_Throws()
    {
        var (service, _) = Build(new NoDiscount());
        Assert.Throws<OrderException>(() => service.Checkout(new Order("T-4", "Alan")));
    }
}
