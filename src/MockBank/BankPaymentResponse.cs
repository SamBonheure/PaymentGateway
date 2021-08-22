namespace MockBank
{
    public class BankPaymentResponse
    {
        public BankPaymentStatus Status { get; private set; }
        public string PaymentId { get; private set; }

        public BankPaymentResponse(BankPaymentStatus status, string paymentId)
        {
            Status = status;
            PaymentId = paymentId;
        }
    }
}
