using AutoMapper;
using FluentAssertions;
using MockBank;
using Moq;
using PaymentGateway.Api.Dispatcher;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Interfaces;
using System;
using Xunit;

namespace PaymentGateway.Api.UnitTests
{
    public class PaymentEventDispatcherTests
    {
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private readonly Mock<IPaymentRepository> _repositoryMock = new Mock<IPaymentRepository>();
        private readonly Mock<IBankAdaptar> _adaptarMock = new Mock<IBankAdaptar>();

        private readonly IEventDispatcher _sut;

        public PaymentEventDispatcherTests()
        {
            _sut = new PaymentEventDispatcher(_repositoryMock.Object, _mapperMock.Object, _adaptarMock.Object);
        }

        [Fact]
        public void Publish_Should_Dispatch_Event_If_Valid()
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

            var bankRequest = new BankPaymentRequest
            {
                PaymentId = paymentId,
                CardNumber = cardNumber.Number,
                CVV = cvv.Code,
                ExpiryYear = expiryDate.Year,
                ExpiryMonth = expiryDate.Month,
                OwnerName = ownerName,
                Amount = money.Amount,
                Currency = money.Currency
            };

            var bankResponse = new BankPaymentResponse(BankPaymentStatus.Approved, paymentId);

            _adaptarMock.Setup(x => x.ProcessPaymentAsync(bankRequest)).ReturnsAsync(bankResponse);

            // Act
            Action action = () => _sut.PublishAsync(new PaymentCreatedEvent(payment));

            // Assert
            action.Should().NotThrow<Exception>();
        }
    }
}
