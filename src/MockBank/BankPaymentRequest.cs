namespace MockBank
{
    public class BankPaymentRequest
    {
        public string CardNumber { get; set; }
        public string CVV { get; set; }
        public int ExpiryYear { get; set; }
        public int ExpiryMonth { get; set; }
        public string OwnerName { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}
