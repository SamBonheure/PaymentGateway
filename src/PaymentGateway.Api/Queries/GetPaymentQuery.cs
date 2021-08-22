using MediatR;
using PaymentGateway.Api.Responses;
using System;

namespace PaymentGateway.Api.Queries
{
    public class GetPaymentQuery : IRequest<GetPaymentResponse>
    {
        public Guid Id { get; set; }
        internal Guid MerchantId { get; set; }
    }
}
