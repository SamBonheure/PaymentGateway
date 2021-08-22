using AutoMapper;
using PaymentGateway.Api.Responses;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Enums;

namespace PaymentGateway.Api.Mapping
{
    public class PaymentMappingProfile : Profile
    {
        public PaymentMappingProfile()
        {
            CreateMap<Payment, CreatePaymentResponse>()
               .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Payment processing"));

            CreateMap<Payment, GetPaymentResponse>()
              .ForMember(dest => dest.PaymentId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
              .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.Status == PaymentStatus.Approved))
              .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
              .ForMember(dest => dest.Card, opt => opt.MapFrom(src => new CardModel
              {
                  CardNumber = src.Card.Number.Number,
                  ExpiryMonth = src.Card.ExpiryDate.Month,
                  ExpiryYear = src.Card.ExpiryDate.Year
              }))
              .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => new MoneyModel
              {
                  Currency = src.Amount.Currency,
                  Amount = src.Amount.Amount
              }));

        }
    }
}
