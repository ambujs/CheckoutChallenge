namespace Checkout.PaymentGateway.Models
{
    public class PaymentResponse
    {
        public string PaymentId { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
