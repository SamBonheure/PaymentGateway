using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Domain.Services
{
    public static class CardMaskingService
    {
        private const int UNMASKED_DIGITS = 4;

        /// <summary>
        /// Mask a card number
        /// </summary>
        /// <param name="cardNumber">The card number to mask</param>
        /// <returns>A masked card number</returns>
        public static string Mask(CardNumber cardNumber)
        {
            var visiblePart = cardNumber.Number.Substring(0 + cardNumber.Number.Length - UNMASKED_DIGITS);
            var maskedPart = new string('*', cardNumber.Number.Length - UNMASKED_DIGITS);

            return maskedPart + visiblePart;
        }
    }
}
