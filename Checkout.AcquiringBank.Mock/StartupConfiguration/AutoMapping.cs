using AutoMapper;
using Checkout.AcquiringBank.Mock.Models;

namespace Checkout.AcquiringBank.Mock.StartupConfiguration
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<PaymentRequest, Payment>();
        }
    }
}
