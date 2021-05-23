namespace Checkout.PaymentGateway.Models
{
    public class AcquirerResponse
    {
        public bool Successful { get; set; }
        public string ErrorCode { get; set; }
    }
}
