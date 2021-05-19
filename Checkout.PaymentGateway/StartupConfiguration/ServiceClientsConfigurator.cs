using System;
using Checkout.PaymentGateway.HttpClientServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.StartupConfiguration
{
    public static class ServiceClientsConfigurator
    {
        public static IServiceCollection AddServiceClients(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            ConfigureHttpClientFor<IAcquiringBankClient, AcquiringBankClient>(serviceCollection, configuration["acquiringBankBaseUri"]);
            return serviceCollection;
        }

        private static void ConfigureHttpClientFor<T, TImplementation>(IServiceCollection services, string baseUri)
            where TImplementation : class, T where T : class
        {
            services.AddHttpClient<T, TImplementation>(c =>
                {
                    c.BaseAddress = new Uri(baseUri);
                    c.Timeout = TimeSpan.FromSeconds(30);
                });
        }
    }
}
