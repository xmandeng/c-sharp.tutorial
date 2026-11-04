using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Tutorial.Api;
using Tutorial.Orders;

namespace Tutorial.Tests;

// Integration tests: WebApplicationFactory starts the whole web API in memory,
// with no network port, and returns an HttpClient connected to it.
// Python equivalent: FastAPI's TestClient(app).
public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    // xUnit creates one factory for the class and passes it to each test object.
    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    // xUnit v3 supplies a token that cancels the call if the run is aborted.
    private static CancellationToken Token =>
        TestContext.Current.CancellationToken;

    [Fact]
    public async Task GetProducts_ReturnsTheCatalog()
    {
        var products = await _client.GetFromJsonAsync<List<Product>>(
            "/products",
            Token
        );

        Assert.NotNull(products);
        Assert.Equal(3, products.Count);
    }

    [Fact]
    public async Task PostOrder_ValidOrder_ReturnsReceipt()
    {
        var request = new OrderRequest("Ada", [new LineRequest("MN-03", 1)]);

        var response = await _client.PostAsJsonAsync("/orders", request, Token);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var receipt = await response.Content.ReadFromJsonAsync<Receipt>(Token);
        Assert.Equal(174.96m, receipt!.Total); // (180 - 18) * 1.08
    }

    [Fact]
    public async Task PostOrder_UnknownSku_Returns404()
    {
        var request = new OrderRequest("Ada", [new LineRequest("XX-99", 1)]);

        var response = await _client.PostAsJsonAsync("/orders", request, Token);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
