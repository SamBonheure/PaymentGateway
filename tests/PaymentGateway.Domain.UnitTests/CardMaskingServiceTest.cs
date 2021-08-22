using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Services;
using Xunit;


namespace PaymentGateway.Domain.UnitTests
{
    public class CardMaskingServiceTest
    {
        [Theory]
        [InlineData("3792120488774391", "************4391")]
        [InlineData("6334 1389 7772 5595", "***************5595")]
        public void Mask_Should_Return_Masked_CardNumber(string unmaskedCardNumber, string expectedMaskedCardNumber)
        {
            // Arrange
            var cardNumber = CardNumber.Create(unmaskedCardNumber);

            // Act
            string maskedCardNumber = CardMaskingService.Mask(cardNumber);

            // Assert
            maskedCardNumber.Should().Be(expectedMaskedCardNumber);
        }
    }
}
