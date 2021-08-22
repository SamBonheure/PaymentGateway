using System;

namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidExpiryDateException : Exception
    {
        public InvalidExpiryDateException()
        {
        }

        public InvalidExpiryDateException(string message)
            : base(message)
        {
        }

        public InvalidExpiryDateException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
