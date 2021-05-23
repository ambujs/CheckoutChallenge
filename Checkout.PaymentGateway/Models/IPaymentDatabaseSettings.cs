namespace Checkout.PaymentGateway.Models
{
    public interface IPaymentDatabaseSettings
    {
        string PaymentsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
