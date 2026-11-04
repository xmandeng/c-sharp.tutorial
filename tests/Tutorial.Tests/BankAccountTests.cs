using Tutorial;

namespace Tutorial.Tests;

// xUnit: [Fact] marks a test (pytest: a function named test_*).
// Assert.Equal(expected, actual) is pytest's: assert actual == expected
public class BankAccountTests
{
    [Fact]
    public void Deposit_IncreasesBalance()
    {
        // Arrange
        var account = new BankAccount("Ada", 100m);

        // Act
        account.Deposit(25m);

        // Assert
        Assert.Equal(125m, account.Balance);
    }

    [Fact]
    public void Withdraw_MoreThanBalance_Throws()
    {
        var account = new BankAccount("Ada", 10m);

        // pytest: with pytest.raises(InvalidOperationException): ...
        Assert.Throws<InvalidOperationException>(() => account.Withdraw(50m));
        Assert.Equal(10m, account.Balance);
    }

    // [Theory] + [InlineData] = pytest's @pytest.mark.parametrize
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Deposit_NonPositive_Throws(int amount)
    {
        var account = new BankAccount("Ada");
        Assert.Throws<ArgumentOutOfRangeException>(() => account.Deposit(amount));
    }
}
