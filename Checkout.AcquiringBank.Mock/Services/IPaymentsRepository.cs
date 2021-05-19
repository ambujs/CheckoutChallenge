using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;

namespace Checkout.AcquiringBank.Mock.Services
{
    public interface IPaymentsRepository
    {
        Task<string> SavePayment(Payment payment);
        Task<Payment> GetPayment(string id);
    }
}
