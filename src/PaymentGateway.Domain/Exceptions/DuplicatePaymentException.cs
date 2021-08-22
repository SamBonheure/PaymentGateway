using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentGateway.Domain.Exceptions
{
    public class DuplicatePaymentException : Exception
    {
        public DuplicatePaymentException()
        {
        }

        public DuplicatePaymentException(string message)
            : base(message)
        {
        }

        public DuplicatePaymentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
