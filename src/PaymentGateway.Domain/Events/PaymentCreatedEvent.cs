using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Domain.Events
{
    public class PaymentCreatedEvent : IEvent
    {
        public Payment Payment { get; private set; }

        public PaymentCreatedEvent(Payment payment)
        {
            Payment = payment;
        }
    }
}
