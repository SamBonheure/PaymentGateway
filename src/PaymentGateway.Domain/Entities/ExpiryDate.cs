using PaymentGateway.Domain.Exceptions;
using System;

namespace PaymentGateway.Domain.Entities
{
    public class ExpiryDate
    {
        public int Year { get; private set; }
        public int Month { get; private set; }

        private ExpiryDate(int year, int month)
        {
            Year = year;
            Month = month;
        }

        public static ExpiryDate Create(int year, int month)
        {
            if (month < 1 || month > 12)
            {
                throw new InvalidExpiryDateException($"{month}/{year} is not a valid expiry date");
            }

            if (DateTime.Now.Year > year + 2000 || (DateTime.Now.Year == year + 2000 && DateTime.Now.Month > month))
            {
                throw new InvalidExpiryDateException("Invalid expiry date, card expired");
            }

            return new ExpiryDate(year, month);
        }
    }
}
