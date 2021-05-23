using AutoMapper;

namespace Checkout.PaymentGateway.Services
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Models.Payment, Models.Mongo.Payment>();
        }
    }
}
