using FluentAssertions;
using Newtonsoft.Json;
using PaymentGateway.Api.Commands;
using PaymentGateway.Api.Responses;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PaymentGateway.Api.IntegrationTests
{
    public class RetrievePaymentIntegrationTest : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public RetrievePaymentIntegrationTest(ApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Get_Payment_Should_Return_Correct_Data_When_Valid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();
            var paymentCommand = new CreatePaymentCommand()
            {
                Id = paymentId,
                Description = "Amazon Payment",
                Amount = 50,
                Currency = "USD",
                Card = new CreatePaymentCommand.CardDetails
                {
                    Number = "3792120488774391",
                    CVV = "665",
                    ExpiryMonth = 12,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            };

            await client.PostAsync("/Payments", new StringContent(JsonConvert.SerializeObject(paymentCommand), Encoding.UTF8, "application/json"));

            // Act
            var response = await client.GetAsync("/Payments/"+paymentId.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<GetPaymentResponse>(json);

            string maskedCardNumber = "************4391";

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            paymentResponse.PaymentId.Should().Be(paymentId);
            paymentResponse.Amount.Amount.Should().Be(paymentCommand.Amount);
            paymentResponse.Amount.Currency.Should().Be(paymentCommand.Currency);
            paymentResponse.Card.CardNumber.Should().Be(maskedCardNumber);
            paymentResponse.Card.ExpiryYear.Should().Be(paymentCommand.Card.ExpiryYear);
            paymentResponse.Card.ExpiryMonth.Should().Be(paymentCommand.Card.ExpiryMonth);
            paymentResponse.Description.Should().Be(paymentCommand.Description);
        }

        [Fact]
        public async Task Get_Payment_Should_Return_NotFound_When_PaymentId_Non_Existing()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");
            var paymentId = Guid.NewGuid();

            // Act
            var response = await client.GetAsync("/Payments/" + paymentId.ToString());
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            errormessage.Replace("\"", "").Should().Be($"Payment with id {paymentId} not found");
        }
    }
}
