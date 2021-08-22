using AspNetCoreRateLimit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using PaymentGateway.Api.Security;
using PaymentGateway.Domain.Interfaces;
using PaymentGateway.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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

        /// <summary>
        /// Register the API documentation services
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddAutoDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Payment Gateway API",
                    Version = "v1"
                });

                option.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Api Key in header",
                    Type = SecuritySchemeType.ApiKey,
                    Name = HeaderNames.Authorization,
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "ApiKey"
                            },
                            In = ParameterLocation.Header
                        }, new List<string>()
                    }
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                option.IncludeXmlComments(xmlPath);
            });
        }
    }
}
