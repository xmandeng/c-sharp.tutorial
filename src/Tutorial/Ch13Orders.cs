using Tutorial.Orders;

namespace Tutorial;

// Chapter 13: a small program that uses everything so far.
public static class Ch13Orders
{
    public static void Run()
    {
        var keyboard = new Product("KB-01", "Keyboard", 45.00m);
        var mouse = new Product("MS-02", "Mouse", 25.00m);
        var monitor = new Product("MN-03", "Monitor", 180.00m);

        var inventory = new Inventory();
        inventory.Restock(keyboard.Sku, 10);
        inventory.Restock(mouse.Sku, 10);
        inventory.Restock(monitor.Sku, 1);

        var service = new OrderService(inventory, new BulkDiscount());

        var small = new Order("ORD-1", "Ada");
        small.Add(mouse, 2);
        PrintReceipt(service.Checkout(small));

        var large = new Order("ORD-2", "Grace");
        large.Add(keyboard, 1);
        large.Add(monitor, 1);
        PrintReceipt(service.Checkout(large));

        var tooMany = new Order("ORD-3", "Linus");
        tooMany.Add(monitor, 1);          // the only monitor was sold in ORD-2
        try
        {
            service.Checkout(tooMany);
        }
        catch (OrderException ex)
        {
            Console.WriteLine($"{ex.OrderId} rejected: {ex.Message}");
        }

        Console.WriteLine($"Mice left: {inventory.StockOf(mouse.Sku)}");
    }

    private static void PrintReceipt(Receipt r)
    {
        Console.WriteLine($"{r.OrderId}: subtotal {r.Subtotal:0.00} - discount {r.Discount:0.00} " +
                          $"+ tax {r.Tax:0.00} = {r.Total:0.00}");
    }
}
