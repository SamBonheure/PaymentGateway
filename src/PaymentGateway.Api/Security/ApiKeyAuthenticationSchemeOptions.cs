using Microsoft.AspNetCore.Authentication;

namespace PaymentGateway.Api.Security
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public const string Scheme = "ApiKeyScheme";
    }
}
