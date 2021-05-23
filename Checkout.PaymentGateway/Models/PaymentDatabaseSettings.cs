namespace Checkout.PaymentGateway.Models
{
    public class PaymentDatabaseSettings : IPaymentDatabaseSettings
    {
        public string PaymentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
