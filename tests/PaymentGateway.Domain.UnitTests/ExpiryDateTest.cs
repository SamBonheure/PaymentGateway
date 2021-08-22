using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using System;
using Xunit;

namespace PaymentGateway.Domain.UnitTests
{
    public class ExpiryDateTest
    {
        [Theory]
        [InlineData(2, 25)]
        [InlineData(5, 23)]
        [InlineData(12, 21)]
        public void Create_Should_Return_ExpiryDate_When_Month_Year_Is_Valid(int month, int year)
        {
            // Act
            var expiryDate = ExpiryDate.Create(year, month);

            // Assert
            expiryDate.Year.Should().Be(year);
            expiryDate.Month.Should().Be(month);
        }

        [Theory]
        [InlineData(0, 25)]
        [InlineData(13, 23)]
        public void Create_Should_Return_Error_When_Month_Is_Invalid(int month, int year)
        {
            // Act
            Action action = () => ExpiryDate.Create(year, month);

            // Assert
            action.Should().Throw<InvalidExpiryDateException>().WithMessage($"{month}/{year} is not a valid expiry date");
        }

        [Theory]
        [InlineData(10, 19)]
        [InlineData(9, 1)]
        public void Create_Should_Return_Error_When_Year_Is_In_Past(int month, int year)
        {
            // Act
            Action action = () => ExpiryDate.Create(year, month);

            // Assert
            action.Should().Throw<InvalidExpiryDateException>().WithMessage("Invalid expiry date, card expired");
        }
    }
}
