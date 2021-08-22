using System;

namespace PaymentGateway.Api.Responses
{
    public class GetPaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
        public bool IsApproved { get; set; }
        public CardModel Card { get; set; }
        public MoneyModel Amount { get; set; }
        public string Description { get; set; }
    }
    public class CardModel
    {
        public string CardNumber { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
    public class MoneyModel
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}
