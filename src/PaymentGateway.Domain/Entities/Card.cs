namespace PaymentGateway.Domain.Entities
{
    public class Card
    {
        public CardNumber Number { get; private set; }
        public CVV Cvv { get; private set; }
        public ExpiryDate ExpiryDate { get; private set; }
        public string OwnerName { get; set; }

        private Card(CardNumber number, CVV cvv, ExpiryDate expiryDate, string ownerName)
        {
            Number = number;
            Cvv = cvv;
            ExpiryDate = expiryDate;
            OwnerName = ownerName;
        }

        public static Card Create(CardNumber number, CVV cvv, ExpiryDate expiryDate, string ownerName)
        {
            return new Card(number, cvv, expiryDate, ownerName);
        }
    }
}
