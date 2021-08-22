using PaymentGateway.Domain.Enums;
using System;

namespace PaymentGateway.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; private set; }
        public Guid MerchantId { get; private set; }
        public Card Card { get; private set; }
        public Money Amount { get; private set; }
        public string Description { get; private set; }
        public PaymentStatus Status { get; private set; }
        public PaymentDeclinedReasonCode? Reason { get; private set; }
        public bool? IsRisk { get; private set; }
        public DateTime RequestedOn { get; private set; }

        private Payment(Guid id, Guid merchantId, Card card, Money amount, string description)
        {
            Id = id;
            MerchantId = merchantId;
            Card = card;
            Amount = amount;
            Description = description;
            RequestedOn = DateTime.Now;
            Status = PaymentStatus.Pending;
        }

        public void Approve()
        {
            Status = PaymentStatus.Approved;
        }

        public void Decline(PaymentDeclinedReasonCode reason)
        {
            Status = PaymentStatus.Declined;
            Reason = reason;
        }

        public static Payment Create(Guid id, Guid merchantId, Card card, Money amount, string description)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException(nameof(id));
            }
            if (merchantId == Guid.Empty)
            {
                throw new ArgumentException(nameof(merchantId));
            }

            return new Payment(id, merchantId, card, amount, description);
        }
    }
}
