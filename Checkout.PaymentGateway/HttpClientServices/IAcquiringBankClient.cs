using System.Threading.Tasks;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.HttpClientServices
{
    public interface IAcquiringBankClient
    {
        Task<ActionResult<AcquirerResponse>> ProcessPayment(Payment payment);
        Task<ActionResult<Payment>> GetPayment(string paymentId);
    }
}