namespace Checkout.AcquiringBank.Mock.Models
{
    public class PaymentDatabaseSettings : IPaymentDatabaseSettings
    {
        public string PaymentsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
