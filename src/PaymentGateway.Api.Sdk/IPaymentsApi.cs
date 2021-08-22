using PaymentGateway.Api.Commands;
using PaymentGateway.Api.Queries;
using PaymentGateway.Api.Responses;
using Refit;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Sdk
{
    [Headers("Authorization: ApiKey")]
    public interface IPaymentsApi
    {
        [Post("/Payments")]
        Task<ApiResponse<CreatePaymentResponse>> MakePaymentAsync([Body] CreatePaymentCommand paymentCommand);

        [Get("/Payments/{paymentQuery.id}")]
        Task<ApiResponse<GetPaymentResponse>> GetPaymentAsync(GetPaymentQuery paymentQuery);
    }
}
