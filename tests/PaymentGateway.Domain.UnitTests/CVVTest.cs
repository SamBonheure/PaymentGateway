using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using System;
using Xunit;

namespace PaymentGateway.Domain.UnitTests
{
    public class CVVTest
    {
        [Theory]
        [InlineData("665")]
        [InlineData("000")]
        [InlineData("1234")]
        public void Create_Should_Return_CVV_When_Code_Is_Valid(string code)
        {
            // Act
            var cvv = CVV.Create(code);

            // Assert
            cvv.Code.Should().Be(code);
        }

        [Fact]
        public void Create_Should_Return_Error_When_Code_Is_Null()
        {
            // Act
            Action action = () => CVV.Create(null);

            // Assert
            action.Should().Throw<ArgumentNullException>().WithMessage("*code*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("12")]
        [InlineData("12345")]
        public void Create_Should_Return_Error_When_Code_Is_Not_Three_Or_Four_Characters(string code)
        {
            // Act
            Action action = () => CVV.Create(code);

            // Assert
            action.Should().Throw<InvalidCVVException>().WithMessage($"{code} is not a valid CVV code");
        }
    }
}
