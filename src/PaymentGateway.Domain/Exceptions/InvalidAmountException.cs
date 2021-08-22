using System;

namespace PaymentGateway.Domain.Exceptions
{
    public class InvalidAmountException : Exception
    {
        public InvalidAmountException()
        {
        }

        public InvalidAmountException(string message)
            : base(message)
        {
        }

        public InvalidAmountException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
