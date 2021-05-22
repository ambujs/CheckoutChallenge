using System.Net.Http;
using System.Text;
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
            var apiURL = "/api/payment";
            
            var endpoint = baseUri + apiURL;

            var json = "{\"cardNumber\": \"1231233312341234\",\"amount\": 9999.99,\"currency\": \"GBP\",\"cvv\": \"012\",\"expiryDate\": {\"year\": 2031,\"month\": 12}}";
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(endpoint, data);
        }

        [Benchmark]
        public async Task RetrievePayment()
        {
            var baseUri = "https://localhost:4001/";
            var apiURL = "/api/payment/ca3e5fb5-2b85-4365-87d0-1ec4d1ca72e9";

            var endpoint = baseUri + apiURL;
            var response = await Client.GetAsync(endpoint);
        }
    }
}
