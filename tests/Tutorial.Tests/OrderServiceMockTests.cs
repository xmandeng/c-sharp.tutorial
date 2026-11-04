using Moq;
using Tutorial.Orders;

namespace Tutorial.Tests;

// Moq builds a fake IDiscountPolicy at run time, so this test checks OrderService
// on its own, without depending on how any real discount rule behaves.
// Python equivalent: unittest.mock.Mock(spec=DiscountPolicy).
public class OrderServiceMockTests
{
    [Fact]
    public void Checkout_UsesTheDiscountFromThePolicy()
    {
        // Arrange: a fake policy that returns 10.00 for any order
        // (Python: policy.discount_for.return_value = Decimal("10.00"))
        var policy = new Mock<IDiscountPolicy>();
        policy.Setup(p => p.DiscountFor(It.IsAny<Order>())).Returns(10.00m);

        var stock = new Inventory();
        stock.Restock("MS-02", 5);
        var service = new OrderService(stock, policy.Object);

        var order = new Order("T-5", "Ada");
        order.Add(new Product("MS-02", "Mouse", 25.00m), 2); // 50.00

        // Act
        Receipt receipt = service.Checkout(order);

        // Assert: the fake's discount was applied, and it was asked exactly once
        // (Python: policy.discount_for.assert_called_once_with(order))
        Assert.Equal(10.00m, receipt.Discount);
        Assert.Equal(43.20m, receipt.Total); // (50 - 10) * 1.08
        policy.Verify(p => p.DiscountFor(order), Times.Once());
    }
}
