using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Commands;
using PaymentGateway.Api.Responses;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Handlers.Command
{
    public class CreatePaymentHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePaymentHandler> _logger;

        public CreatePaymentHandler(ILogger<CreatePaymentHandler> logger, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            var paymentResponse = new CreatePaymentResponse();

            try
            {
                var cardNumber = CardNumber.Create(request.Card.Number);
                var cvv = CVV.Create(request.Card.CVV);
                var expiryDate = ExpiryDate.Create(request.Card.ExpiryYear, request.Card.ExpiryMonth);
                var card = Card.Create(cardNumber, cvv, expiryDate, request.Card.OwnerName);
                var amount = Money.Create(request.Currency, request.Amount);
                Payment payment = Payment.Create(request.Id, request.MerchantId, card, amount, request.Description);

                await _paymentRepository.CreateAsync(payment);

                paymentResponse = _mapper.Map<CreatePaymentResponse>(payment);

                _logger.LogInformation($"Payment started processing to {request.MerchantId} by {request.Card.Number} for {request.Amount} {request.Currency}");
            }
            catch (Exception ex)
            {
                // Validation messages from payment creation will be passed here
                paymentResponse.Status = $"Payment failed: {ex.Message}";

                _logger.LogError($"Payment failed to {request.MerchantId} by {request.Card.Number} for {request.Amount} {request.Currency}. {ex.Message}");
            }

            return paymentResponse;
        }
    }
}
