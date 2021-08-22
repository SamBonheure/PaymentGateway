using System;

namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCardNumberException : Exception
    {
        public InvalidCardNumberException()
        {
        }

        public InvalidCardNumberException(string message)
            : base(message)
        {
        }

        public InvalidCardNumberException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
