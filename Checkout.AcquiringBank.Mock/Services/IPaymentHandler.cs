using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;

namespace Checkout.AcquiringBank.Mock.Services
{
    public interface IPaymentHandler
    {
        Task<PaymentResponse> Process(PaymentRequest payment);
    }
}