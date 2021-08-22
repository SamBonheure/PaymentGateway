using System;

namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidCVVException : Exception
    {
        public InvalidCVVException()
        {
        }

        public InvalidCVVException(string message)
            : base(message)
        {
        }

        public InvalidCVVException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}