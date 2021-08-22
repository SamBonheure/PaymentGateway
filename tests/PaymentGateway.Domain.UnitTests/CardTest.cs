using FluentAssertions;
using PaymentGateway.Domain.Entities;
using Xunit;


namespace PaymentGateway.Domain.UnitTests
{
    public class CardTest
    {
        [Fact]
        public void Create_Should_Return_Card_When_Valid()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";

            // Act
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);

            // Assert
            card.Number.Should().Be(cardNumber);
            card.Cvv.Should().Be(cvv);
            card.ExpiryDate.Should().Be(expiryDate);
            card.OwnerName.Should().Be(ownerName);
        }
    }
}
