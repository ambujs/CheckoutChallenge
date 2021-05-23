namespace Checkout.AcquiringBank.Mock.Models
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public decimal Amount { get; set; }

        public Currency Currency { get; set; }

        public string CVV { get; set; }
        public ExpiryDate ExpiryDate { get; set; }
    }
}
