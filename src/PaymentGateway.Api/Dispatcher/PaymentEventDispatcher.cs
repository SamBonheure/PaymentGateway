using AutoMapper;
using MockBank;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Events;
using PaymentGateway.Domain.Interfaces;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Dispatcher
{
    public class PaymentEventDispatcher : IEventDispatcher
    {
        private readonly IBankAdaptar _adaptar;
        private readonly IMapper _mapper;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentEventDispatcher(IPaymentRepository paymentRepository, IMapper mapper, IBankAdaptar adaptar)
        {
            _adaptar = adaptar;
            _mapper = mapper;
            _paymentRepository = paymentRepository;
        }

        public Task PublishAsync<IEvent>(IEvent Event)
        {
            if (Event.GetType() == typeof(PaymentCreatedEvent))
            {
                var task = Task.Run(() => ProcessPayment((Event as PaymentCreatedEvent).Payment));//fire and forget
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Process the payment by sending it to the bank
        /// </summary>
        /// <param name="payment">The payment being process</param>
        /// <returns></returns>
        public async Task ProcessPayment(Payment payment)
        {
            var bankRequest = _mapper.Map<BankPaymentRequest>(payment);

            var response = await _adaptar.ProcessPaymentAsync(bankRequest);

            if (response.Status == BankPaymentStatus.Approved)
                payment.Approve();
            else
            {
                payment.Decline(response.Reason.Value);
            }

            await _paymentRepository.UpdateAsync(payment);
        }
    }
}
