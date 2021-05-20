using BenchmarkDotNet.Running;

namespace Checkout.PaymentGateway.PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PaymentGatewayApiBenchmark>();
        }
    }
}
