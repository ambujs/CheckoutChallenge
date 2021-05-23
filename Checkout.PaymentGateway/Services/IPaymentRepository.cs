using System.Threading.Tasks;
using Checkout.PaymentGateway.Models.Mongo;

namespace Checkout.PaymentGateway.Services
{
    public interface IPaymentRepository
    {
        Task<string> SavePayment(Payment payment);
        Task<Payment> GetPayment(string id);
    }
}
