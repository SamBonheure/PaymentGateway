using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Api.Security;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Repositories;

namespace PaymentGateway.Api
{
    /// <summary>
    /// DI class responsible for injecting middleware
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Register the Infrastructure Layer of the project
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        }

        /// <summary>
        /// Register the security layer.
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddSecurity(this IServiceCollection services)
        {
            services.AddAuthentication(ApiKeyAuthenticationSchemeOptions.Scheme)
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationSchemeOptions.Scheme, cfg => { });
        }

        /// <summary>
        /// Register the API rate limiting services
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddRateLimiting(this IServiceCollection services)
        {
            services.Configure<ClientRateLimitOptions>(Program.Configuration.GetSection("ClientRateLimiting"));
            services.Configure<ClientRateLimitPolicies>(Program.Configuration.GetSection("ClientRateLimitPolicies"));
            services.AddInMemoryRateLimiting();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
