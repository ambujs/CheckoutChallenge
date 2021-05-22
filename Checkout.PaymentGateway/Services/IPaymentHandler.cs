using System.Threading.Tasks;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentHandler
    {
        Task<ActionResult<PaymentResponse>> Process(Payment payment);
        Task<ActionResult<Payment>> Retrieve(string paymentId);
    }
}