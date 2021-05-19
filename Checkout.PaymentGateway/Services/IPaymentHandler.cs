using System.Threading.Tasks;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentHandler
    {
        Task<IActionResult> Process(Payment payment);
        Task<IActionResult> Retrieve(string paymentId);
    }
}