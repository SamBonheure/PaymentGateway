using MediatR;
using PaymentGateway.Api.Responses;
using System;

namespace PaymentGateway.Api.Commands
{
    public class CreatePaymentCommand : IRequest<CreatePaymentResponse>
    {
        public Guid Id { get; set; }
        internal Guid MerchantId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? Description { get; set; }
        public CardDetails Card { get; set; }

        public class CardDetails
        {
            public string Number { get; set; }
            public string CVV { get; set; }
            public int ExpiryMonth { get; set; }
            public int ExpiryYear { get; set; }
            public string OwnerName { get; set; }
        }
    }
}
