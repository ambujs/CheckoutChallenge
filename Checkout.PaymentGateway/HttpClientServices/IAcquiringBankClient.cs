using System.Threading.Tasks;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.HttpClientServices
{
    public interface IAcquiringBankClient
    {
        Task<IActionResult> ProcessPayment(Payment payment);
        Task<IActionResult> GetPayment(string paymentId);
    }
}