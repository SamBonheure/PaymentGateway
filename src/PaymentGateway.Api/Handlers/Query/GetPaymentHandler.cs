using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Queries;
using PaymentGateway.Api.Responses;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Domain.Services;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Handlers.Query
{
    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, GetPaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPaymentHandler> _logger;

        public GetPaymentHandler(ILogger<GetPaymentHandler> logger, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<GetPaymentResponse> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetAsync(request.Id);
            _logger.LogInformation($"Payment requested with id {request.Id} for Merchant with id {request.MerchantId}");

            if (payment is null)
                return null;

            var paymentResponse = _mapper.Map<GetPaymentResponse>(payment);
            paymentResponse.Card.CardNumber = CardMaskingService.Mask(payment.Card.Number);

            return paymentResponse;
        }
    }
}
