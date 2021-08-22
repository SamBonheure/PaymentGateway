using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using System;
using Xunit;

namespace PaymentGateway.Domain.UnitTests
{
    public class MoneyTest
    {
        [Theory]
        [InlineData("USD", 50)]
        [InlineData("EUR", 10.50)]
        public void Create_Should_Return_Money_When_Amount_And_Currency_Valid(string currency, decimal amount)
        {
            // Act
            var money = Money.Create(currency, amount);

            // Assert
            money.Amount.Should().Be(amount);
            money.Currency.Should().Be(currency);
        }

        [Fact]
        public void Create_Should_Return_Error_When_Currency_Is_Null()
        {
            // Act
            Action action = () => Money.Create(null, 100);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage("*currency*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("US")]
        [InlineData("ABCD")]
        public void Create_Should_Return_Error_When_Currency_Is_Not_3_Characters(string currency)
        {
            // Act
            Action action = () => Money.Create(currency, 100);

            // Assert
            action.Should().Throw<InvalidCurrencyException>().WithMessage($"{currency} is not a valid currency");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Create_Should_Return_Error_When_Amount_Is_Lower_Or_Equal_To_Zero(decimal amount)
        {
            // Act
            Action action = () => Money.Create("USD", amount);

            // Assert
            action.Should().Throw<InvalidAmountException>().WithMessage($"{amount} is not a valid amount");
        }

        [Theory]
        [InlineData(0.001)]
        [InlineData(0.000001)]
        [InlineData(0.00000101)]
        public void Create_Should_Return_Error_When_Amount_Is_More_Than_3_Decimal_Places(int amount)
        {
            // Act
            Action action = () => Money.Create("USD", amount);

            // Assert
            action.Should().Throw<InvalidAmountException>().WithMessage($"{amount} is not a valid amount");
        }
    }
}
