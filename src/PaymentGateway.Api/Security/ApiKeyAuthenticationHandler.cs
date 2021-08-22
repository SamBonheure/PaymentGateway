using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Security
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;
        public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, ILoggerFactory logger, IConfiguration configuration, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var value))
            {
                await SetUnauthorizedContext("Authentication header cannot be found.");
                return AuthenticateResult.Fail("Authentication header cannot be found.");
            }
            if (!AuthenticationHeaderValue.TryParse(value, out var header))
            {
                await SetUnauthorizedContext("Authentication header is not valid.");
                return AuthenticateResult.Fail("Authentication header is not valid.");
            }

            // API Keys stored in config. In practice this should be a DB
            var apiKeys = _configuration.GetSection("ApiKeys");
            ApiKey matchingKey = null;

            //TODO: rewrite this to be more elegant
            foreach (IConfigurationSection section in apiKeys.GetChildren())
            {
                var key = section.GetValue<string>("Key");

                if (key.Equals(header.Scheme))
                {
                    matchingKey = new ApiKey
                    {
                        Key = key,
                        MerchantId = section.GetValue<string>("MerchantId")
                    };

                    break;
                }

            }

            if (matchingKey is null)
            {
                await SetUnauthorizedContext("Invalid API Key");
                return AuthenticateResult.Fail("Invalid API Key");
            }

            var claims = new[]{
                new Claim("MerchantId", matchingKey.MerchantId)
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, ApiKeyAuthenticationSchemeOptions.Scheme));

            return AuthenticateResult.Success(new AuthenticationTicket(principal, ApiKeyAuthenticationSchemeOptions.Scheme));
        }

        private async Task SetUnauthorizedContext(string message)
        {
            if (!Context.Response.HasStarted)
            {
                string result;
                Context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                result = JsonConvert.SerializeObject(new { error = message });
                Context.Response.ContentType = "application/json";
                await Context.Response.WriteAsync(result);
            }
            else
            {
                await Context.Response.WriteAsync(string.Empty);
            }
        }
    }

    public class ApiKey
    {
        public string Key { get; set; }
        public string MerchantId { get; set; }
    }
}
