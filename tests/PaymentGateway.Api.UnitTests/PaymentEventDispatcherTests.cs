using PaymentGateway.Api.Dispatcher;
using PaymentGateway.Domain.Interfaces;
using System;
using Xunit;
using Fluentassertion;

namespace PaymentGateway.Api.UnitTests
{
    public class PaymentEventDispatcherTests
    {
        [Fact]
        public void Publish_Should_Throw_NotSupport_When_Event_Type_Invalid()
        {
            // Arrange
            PaymentEventDispatcher dispatcher = new PaymentEventDispatcher();

            // Act
            Action action = () => dispatcher.PublishAsync(new IEvent());

            // Assert
            action.Should().Throw<ArgumentException>().WithMessage("jobId cannot be empty.");
        }
    }
}
