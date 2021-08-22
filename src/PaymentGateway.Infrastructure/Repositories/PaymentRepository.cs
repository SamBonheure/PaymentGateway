using Microsoft.Extensions.Caching.Memory;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Exceptions;
using PaymentGateway.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IMemoryCache _cache;

        public PaymentRepository(IMemoryCache cache)
        {
            _cache = cache;
        }

        public async Task<Payment> GetAsync(Guid id)
        {
            bool paymentFound = _cache.TryGetValue<Payment>(id, out var payment);
            return paymentFound ? payment : null;
        }

        public async Task CreateAsync(Payment payment)
        {
            if (_cache.TryGetValue<Payment>(payment.Id, out var existingPayment))
                throw new DuplicatePaymentException($"Duplicate payment with id {payment.Id}");

            _cache.Set(payment.Id, payment);
        }
    }
}
