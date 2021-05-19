using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Checkout.PaymentGateway.PerformanceTest
{
    public class PaymentGatewayApiBenchmark
    {
        private static readonly HttpClient Client = new HttpClient();

        [Benchmark]
        public async Task ProcessPayment()
        {
            var baseUri = "https://localhost:4001/";
            var apiURL = "/api/payment/ca3e5fb5-2b85-4365-87d0-1ec4d1ca72e9";
            
            var endpoint = baseUri + apiURL;
            var response = await Client.GetAsync(endpoint);
        }
    }
}
