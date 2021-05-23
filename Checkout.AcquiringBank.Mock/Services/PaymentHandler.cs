using System;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;

namespace Checkout.AcquiringBank.Mock.Services
{
    public class PaymentHandler : IPaymentHandler
    {
        public async Task<PaymentResponse> Process(PaymentRequest paymentRequest)
        {
            // Generate random response to replicate successful and failed payments
            var random = new Random();
            var successful = random.Next(0, 2) > 0;

            var paymentResponse = new PaymentResponse
            {
                Successful = successful,
                ErrorCode = successful ? null : "10001"
            };
            
            return await Task.FromResult(paymentResponse);
        }
    }
}
