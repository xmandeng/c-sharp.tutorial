// A minimal ASP.NET Core web API over the Chapter 11 order code.
//   dotnet run --project src/Tutorial.Api
//   curl http://localhost:5239/products
using Tutorial.Api;
using Tutorial.Orders;

var builder = WebApplication.CreateBuilder(args);

// 1. Build the shared objects and register them with the
//    dependency-injection (DI) container.
var catalog = new Dictionary<string, Product>
{
    ["KB-01"] = new Product("KB-01", "Keyboard", 45.00m),
    ["MS-02"] = new Product("MS-02", "Mouse", 25.00m),
    ["MN-03"] = new Product("MN-03", "Monitor", 180.00m),
};
var inventory = new Inventory();
foreach (var sku in catalog.Keys)
{
    inventory.Restock(sku, 5);
}

builder.Services.AddSingleton(catalog);
builder.Services.AddSingleton(inventory);
builder.Services.AddSingleton<IDiscountPolicy>(new BulkDiscount(100m, 0.10m));
builder.Services.AddSingleton<OrderService>(); // DI calls its constructor

var app = builder.Build();

// 2. Map URLs to handler lambdas. ASP.NET fills each parameter from the
//    URL, the JSON request body, or the DI container.
app.MapGet(
    "/products",
    (Dictionary<string, Product> catalog) => catalog.Values
);

app.MapGet(
    "/stock/{sku}",
    (string sku, Inventory inventory) =>
        Results.Ok(new { sku, inStock = inventory.StockOf(sku) })
);

app.MapPost(
    "/orders",
    (
        OrderRequest request,
        Dictionary<string, Product> catalog,
        OrderService service
    ) =>
    {
        var order = new Order(
            $"ORD-{Random.Shared.Next(1000, 9999)}",
            request.Customer
        );
        try
        {
            foreach (var line in request.Lines)
            {
                if (!catalog.TryGetValue(line.Sku, out Product? product))
                {
                    return Results.NotFound(
                        new { error = $"Unknown SKU {line.Sku}" }
                    );
                }
                order.Add(product, line.Quantity);
            }
            return Results.Ok(service.Checkout(order)); // record -> JSON
        }
        catch (Exception ex) when (ex is OrderException or ArgumentException)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
);

await app.RunAsync();
