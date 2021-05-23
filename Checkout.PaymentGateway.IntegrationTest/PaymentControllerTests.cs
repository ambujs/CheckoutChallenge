using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using App.Metrics.AspNetCore;
using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using Checkout.PaymentGateway.Services;
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
    // NOTE: more tests can be added to cover more scenarios
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
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/payment/123");

            // Assert
            response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
        }

        // NOTE: we are not testing mongo integration here so it's ok to mock this
        // If needed we can have in-memory version of mongodb that can be used with integration tests and CI pipelines
        public class MockPaymentRepository : IPaymentRepository
        {
            // TODO: add tests around this method
            public Task<string> SavePayment(Models.Mongo.Payment payment)
            {
                throw new NotImplementedException();
            }

            public Task<Models.Mongo.Payment> GetPayment(string id)
            {
                return Task.FromResult(new Models.Mongo.Payment());
            }
        }

        public class MockAcquiringBankClient : IAcquiringBankClient
        {
            // TODO: add tests around this method
            public Task<ActionResult<AcquirerResponse>> ProcessPayment(Payment payment)
            {
                throw new NotImplementedException();
            }

            public async Task<ActionResult<Payment>> GetPayment(string paymentId)
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
                services.AddSingleton<IPaymentRepository, MockPaymentRepository>();
            }
        }
    }
}
