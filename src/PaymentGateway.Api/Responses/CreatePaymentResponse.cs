using System;

namespace PaymentGateway.Api.Responses
{
    public class CreatePaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}
