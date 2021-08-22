using AspNetCoreRateLimit;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MockBank;
using PaymentGateway.Api.Dispatcher;
using PaymentGateway.Api.Mapping;
using PaymentGateway.Domain.Interfaces;

namespace PaymentGateway.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddScoped<IEventDispatcher, PaymentEventDispatcher>();
            services.AddScoped<IBankAdaptar, MockBankAdaptar>();
            services.AddInfrastructure();
            services.AddMediatR(typeof(Startup));
            services.AddAutoMapper(typeof(PaymentMappingProfile));
            services.AddControllers();
            services.AddSecurity();
            services.AddRateLimiting();
            services.AddAutoDocumentation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseClientRateLimiting();
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment Gateway API");
                option.RoutePrefix = "docs";
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
