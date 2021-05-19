using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Checkout.PaymentGateway.IntegrationTest
{
    // NOTE: more tests can be added to cover more scnearios
    public class PaymentControllerTests : IClassFixture<PaymentGatewayFactory<PaymentControllerTests.FakeStartup>>
    {
        private readonly WebApplicationFactory<FakeStartup> _factory;

        public PaymentControllerTests(PaymentGatewayFactory<FakeStartup> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseMetrics();
                builder.ConfigureTestServices(services =>
                {
                    services.AddMvc().AddApplicationPart(typeof(Startup).Assembly);
                });
            });
        }

        [Fact]
        public async void GetPayment_WithExistingPayment_ReturnsPayment()
        {
            // Arrange
            var client = _factory.CreateClient();
            var byteArray = new UTF8Encoding().GetBytes("checkout:checkout123");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            // Act
            var response = await client.GetAsync("/api/payment/123");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async void GetPayment_WithoutAuth_ReturnsUnauthorizedError()
        {
            // Arrange
            // Act
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/api/payment/123");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        public class MockAcquiringBankClient : IAcquiringBankClient
        {
            // TODO: add tests around this method
            public Task<IActionResult> ProcessPayment(Payment payment)
            {
                throw new NotImplementedException();
            }

            public async Task<IActionResult> GetPayment(string paymentId)
            {
                await Task.CompletedTask;   // to stop warnings
                return new OkObjectResult(new Payment());
            }
        }

        public class FakeStartup : Startup
        {
            public FakeStartup(IWebHostEnvironment environment, IConfiguration configuration) : base(environment, configuration)
            {
            }

            public override void ConfigureServices(IServiceCollection services)
            {
                base.ConfigureServices(services);
                services.AddMetrics();
                services.AddSingleton<IAcquiringBankClient, MockAcquiringBankClient>();
            }
        }
    }
}
