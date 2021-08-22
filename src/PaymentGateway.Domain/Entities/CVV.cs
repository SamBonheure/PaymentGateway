using PaymentGateway.Domain.Exceptions;
using System;

namespace PaymentGateway.Domain.Entities
{
    public class CVV
    {
        public string Code { get; private set; }

        private CVV(string code)
        {
            Code = code;
        }

        public static CVV Create(string code)
        {
            if (code is null)
            {
                throw new ArgumentNullException(nameof(code));
            }
            if (code.Length != 3 && code.Length != 4)
            {
                throw new InvalidCVVException($"{code} is not a valid CVV code");
            }

            return new CVV(code);
        }
    }
}
