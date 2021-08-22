using System;
using System.Threading.Tasks;
using PaymentGateway.Api.Commands;
using PaymentGateway.Api.Queries;
using Refit;

namespace PaymentGateway.Api.Sdk.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var paymentsApi = RestService.For<IPaymentsApi>("https://localhost:44395", new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult("123")
            });

            var paymentId = Guid.NewGuid();

            var makePaymentResponse = await paymentsApi.MakePaymentAsync(new CreatePaymentCommand
            {
                Id = paymentId,
                Amount = 50,
                Currency = "USD",
                Description = "Amazon payment",
                Card = new CreatePaymentCommand.CardDetails
                {
                    Number = "1424 4587 9898 2230",
                    CVV = "665",
                    ExpiryMonth = 5,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            });

            var getPaymentResponse = await paymentsApi.GetPaymentAsync(new GetPaymentQuery
            {
                Id = paymentId
            });

            Console.WriteLine($"Payment requested for {paymentId}");
            Console.ReadLine();
        }
    }
}
