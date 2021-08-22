using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace PaymentGateway.Api.IntegrationTests
{
    public class ApiFixture : IDisposable
    {
        public readonly WebApplicationFactory<Startup> factory;

        public ApiFixture()
        {
            var configuration = GetIConfigurationRoot();

            this.factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddConfiguration(configuration);
                });
            });
        }

        public void Dispose()
        {
            factory.Dispose();
        }

        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
