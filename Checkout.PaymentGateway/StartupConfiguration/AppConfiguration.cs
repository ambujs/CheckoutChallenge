using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Checkout.PaymentGateway.StartupConfiguration
{
    public static class AppConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IPaymentHandler, PaymentHandler>();
            services.AddSingleton<IAcquiringBankClient, AcquiringBankClient>();

            return services;
        }
    }
}
