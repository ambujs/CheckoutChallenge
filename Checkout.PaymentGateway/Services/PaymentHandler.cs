using System.Threading.Tasks;
using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentHandler : IPaymentHandler
    {
        private readonly IAcquiringBankClient _acquiringBankClient;
        public PaymentHandler(IAcquiringBankClient acquiringBankClient)
        {
            _acquiringBankClient = acquiringBankClient;
        }

        public async Task<IActionResult> Process(Payment payment)
        {
            return await _acquiringBankClient.ProcessPayment(payment);
        }

        public async Task<IActionResult> Retrieve(string paymentId)
        {
            return await _acquiringBankClient.GetPayment(paymentId);
        }
    }
}
