namespace Checkout.PaymentGateway.Models
{
    public class PaymentStatus
    {
        public bool Successful { get; set; }
        public string ErrorCode { get; set; }
    }
}
