using PaymentGateway.Domain.Enums;
using System;

namespace MockBank
{
    public class BankPaymentResponse
    {
        public BankPaymentStatus Status { get; private set; }
        public PaymentDeclinedReasonCode? Reason { get; private set; }
        public Guid PaymentId { get; private set; }

        public BankPaymentResponse(BankPaymentStatus status, Guid paymentId, PaymentDeclinedReasonCode? reason = null)
        {
            Status = status;
            Reason = reason;
            PaymentId = paymentId;
        }
    }
}
