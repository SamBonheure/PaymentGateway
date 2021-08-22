using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using System;
using Xunit;

namespace PaymentGateway.Domain.UnitTests
{
    public class CardNumberTest
    {
        [Theory]
        [InlineData("3792120488774391")]
        [InlineData("5465984640800225")]
        [InlineData("6011 7854 6419 8118")]
        public void Create_Should_Return_CardNumber_When_Number_Is_Valid(string number)
        {
            // Act
            var cardnumber = CardNumber.Create(number);

            // Assert
            cardnumber.Number.Should().Be(number);
        }

        [Fact]
        public void Create_Should_Return_Error_When_Number_Is_Null()
        {
            // Act
            Action action = () => CardNumber.Create(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage("*number*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("1234")]
        [InlineData("37921204887743")]
        public void Create_Should_Return_Error_When_Number_Is_Invalid(string number)
        {
            // Act
            Action action = () => CardNumber.Create(number);

            // Assert
            action.Should().Throw<InvalidCardNumberException>().WithMessage($"{number} is not a valid card number");
        }
    }
}
