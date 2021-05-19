using App.Metrics.AspNetCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Checkout.PaymentGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var isDevelopment = hostingContext.HostingEnvironment.IsDevelopment();
                    loggerConfiguration
                        .MinimumLevel.Is(isDevelopment ? LogEventLevel.Debug : LogEventLevel.Information)
                        .MinimumLevel.Override("Microsoft", isDevelopment ? LogEventLevel.Debug : LogEventLevel.Warning)
                        .MinimumLevel.Override("System", isDevelopment ? LogEventLevel.Debug : LogEventLevel.Warning)
                        .Enrich.FromLogContext();
                })
                .UseMetrics();   // Metrics
    }
}
