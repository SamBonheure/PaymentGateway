using System;

namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCurrencyException : Exception
    {
        public InvalidCurrencyException()
        {
        }

        public InvalidCurrencyException(string message)
            : base(message)
        {
        }

        public InvalidCurrencyException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
