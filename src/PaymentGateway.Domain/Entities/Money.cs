using PaymentGateway.Domain.Exceptions;
using System;

namespace PaymentGateway.Domain.Entities
{
    public class Money
    {
        public string Currency { get; }
        public decimal Amount { get; }

        private Money(string currency, decimal amount)
        {
            Currency = currency;
            Amount = amount;
        }

        public static Money Create(string currency, decimal amount)
        {
            if (currency is null)
            {
                throw new ArgumentNullException(nameof(currency));
            }
            //TODO: this should check against supported list of Currency codes
            if (currency.Length != 3)
            {
                throw new InvalidCurrencyException($"{currency} is not a valid currency");
            }
            if (amount <= 0 || Decimal.Round(amount, 2) != amount)
            {
                throw new InvalidAmountException($"{amount} is not a valid amount");
            }

            return new Money(currency, amount);
        }
    }
}
