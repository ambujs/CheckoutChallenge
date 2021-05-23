using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Checkout.PaymentGateway.StartupConfiguration
{
    public static class AppConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaymentDatabaseSettings>(
                configuration.GetSection(nameof(PaymentDatabaseSettings)));

            services.AddSingleton<IPaymentDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PaymentDatabaseSettings>>().Value);

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPaymentHandler, PaymentHandler>();
            services.AddSingleton<IAcquiringBankClient, AcquiringBankClient>();

            services.AddSingleton<IPaymentRepository, PaymentRepository>();
            return services;
        }
    }
}
