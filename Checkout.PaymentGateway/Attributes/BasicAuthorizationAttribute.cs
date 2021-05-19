using Microsoft.AspNetCore.Authorization;

namespace Checkout.PaymentGateway.Attributes
{
    public class BasicAuthorizationAttribute : AuthorizeAttribute
    {
        public BasicAuthorizationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
