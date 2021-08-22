using PaymentGateway.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        /// <summary>
        /// Gets a payment by Id
        /// </summary>
        /// <param name="id">The payment id</param>
        /// <returns>The payment or null if the payment cannot be found.</returns>
        public Task<Payment?> GetAsync(Guid id);

        /// <summary>
        /// Create a new payment
        /// </summary>
        /// <param name="payment">The <see cref="Payment"/></param>
        public Task CreateAsync(Payment payment);
    }
}
