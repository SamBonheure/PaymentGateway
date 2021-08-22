using FluentAssertions;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;
using System;
using Xunit;

namespace PaymentGateway.Domain.UnitTests
{
    public class PaymentTest
    {
        [Fact]
        public void Create_Should_Return_Payment_When_Valid()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);
            var money = Money.Create("USD", 50);
            var merchantId = Guid.Parse("6e075f51-6b78-430b-8e77-8fe3d72d9af0");
            var paymentId = Guid.Parse("1f325b1f-b57c-4b8b-82c0-003dd8107dda");
            var description = "Amazon Payment";

            // Act
            var payment = Payment.Create(paymentId, merchantId, card, money, description);

            // Assert
            payment.Id.Should().Be(paymentId);
            payment.MerchantId.Should().Be(merchantId);
            payment.Status.Should().Be(PaymentStatus.Pending);
            payment.Card.Should().Be(card);
            payment.Amount.Should().Be(money);
            payment.Description.Should().Be(description);
        }

        [Fact]
        public void Create_Should_Return_Error_When_MerchantId_Is_Empty()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);
            var money = Money.Create("USD", 50);
            var merchantId = Guid.Empty;
            var paymentId = Guid.Parse("1f325b1f-b57c-4b8b-82c0-003dd8107dda");

            // Act
            Action action = () => Payment.Create(paymentId, merchantId, card, money, "Amazon Payment");

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*merchantId*");
        }

        [Fact]
        public void Create_Should_Return_Error_When_Id_Is_Empty()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);
            var money = Money.Create("USD", 50);
            var merchantId = Guid.Parse("6e075f51-6b78-430b-8e77-8fe3d72d9af0");
            var paymentId = Guid.Empty;

            // Act
            Action action = () => Payment.Create(paymentId, merchantId, card, money, "Amazon Payment");

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("*id*");
        }

        [Fact]
        public void Approve_Should_Set_Payment_As_Approved()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);
            var money = Money.Create("USD", 50);
            var merchantId = Guid.Parse("6e075f51-6b78-430b-8e77-8fe3d72d9af0");
            var paymentId = Guid.Parse("1f325b1f-b57c-4b8b-82c0-003dd8107dda");
            var description = "Amazon Payment";

            var payment = Payment.Create(paymentId, merchantId, card, money, description);

            // Act
            payment.Approve();

            // Assert
            payment.Status.Should().Be(PaymentStatus.Approved);
        }

        [Fact]
        public void Decline_Should_Set_Payment_As_Declined()
        {
            // Arrange
            var cardNumber = CardNumber.Create("3792120488774391");
            var cvv = CVV.Create("665");
            var expiryDate = ExpiryDate.Create(29, 10);
            var ownerName = "Tom Cruise";
            var card = Card.Create(cardNumber, cvv, expiryDate, ownerName);
            var money = Money.Create("USD", 50);
            var merchantId = Guid.Parse("6e075f51-6b78-430b-8e77-8fe3d72d9af0");
            var paymentId = Guid.Parse("1f325b1f-b57c-4b8b-82c0-003dd8107dda");
            var description = "Amazon Payment";
            var reason = PaymentDeclinedReasonCode.InsufficientFunds;

            var payment = Payment.Create(paymentId, merchantId, card, money, description);

            // Act
            payment.Decline(reason);

            // Assert
            payment.Status.Should().Be(PaymentStatus.Declined);
            payment.Reason.Should().Be(reason);
        }
    }
}
