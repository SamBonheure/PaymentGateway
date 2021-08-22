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
    public class CreatePaymentIntegrationTest : IClassFixture<ApiFixture>
    {
        private readonly ApiFixture _fixture;

        public CreatePaymentIntegrationTest(ApiFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Create_Payment_Should_Return_Correct_Data_When_Valid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
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

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var json = await response.Content.ReadAsStringAsync();
            var paymentResponse = JsonConvert.DeserializeObject<CreatePaymentResponse>(json);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            paymentResponse.PaymentId.Should().Be(paymentId);
            paymentResponse.Status.Should().Be("Payment processing");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_Currency_InValid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
            var paymentCommand = new CreatePaymentCommand()
            {
                Id = paymentId,
                Description = "Amazon Payment",
                Amount = 50,
                Currency = "USDDD",
                Card = new CreatePaymentCommand.CardDetails
                {
                    Number = "3792120488774391",
                    CVV = "665",
                    ExpiryMonth = 12,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            };

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: USDDD is not a valid currency");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_Amount_InValid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
            var paymentCommand = new CreatePaymentCommand()
            {
                Id = paymentId,
                Description = "Amazon Payment",
                Amount = -5,
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

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: -5.0 is not a valid amount");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_CardNumber_InValid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
            var paymentCommand = new CreatePaymentCommand()
            {
                Id = paymentId,
                Description = "Amazon Payment",
                Amount = 50,
                Currency = "USD",
                Card = new CreatePaymentCommand.CardDetails
                {
                    Number = "379212",
                    CVV = "665",
                    ExpiryMonth = 12,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            };

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: 379212 is not a valid card number");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_CVV_InValid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
            var paymentCommand = new CreatePaymentCommand()
            {
                Id = paymentId,
                Description = "Amazon Payment",
                Amount = 50,
                Currency = "USD",
                Card = new CreatePaymentCommand.CardDetails
                {
                    Number = "3792120488774391",
                    CVV = "66556",
                    ExpiryMonth = 12,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            };

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: 66556 is not a valid CVV code");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_ExpiryDate_InValid()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
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
                    ExpiryMonth = 13,
                    ExpiryYear = 25,
                    OwnerName = "Tom Cruise"
                }
            };

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: 13/25 is not a valid expiry date");
        }


        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_ExpiryDate_Expired()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
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
                    ExpiryYear = 19,
                    OwnerName = "Tom Cruise"
                }
            };

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var errormessage = await response.Content.ReadAsStringAsync();

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be("Payment failed: Invalid expiry date, card expired");
        }

        [Fact]
        public async Task Create_Payment_Should_Return_BadRequest_When_Duplicate_Payment()
        {
            // Arrange
            var client = _fixture.factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("123", "");

            var paymentId = Guid.NewGuid();

            // Act
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

            var payload = JsonConvert.SerializeObject(paymentCommand);
            var postContent = new StringContent(payload, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("/Payments", postContent);
            var secondaryResponse = await client.PostAsync("/Payments", postContent);
            var errormessage = await secondaryResponse.Content.ReadAsStringAsync();

            // Assert
            secondaryResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            errormessage.Replace("\"", "").Should().Be($"Payment failed: Duplicate payment with id {paymentId.ToString()}");
        }
    }
}
