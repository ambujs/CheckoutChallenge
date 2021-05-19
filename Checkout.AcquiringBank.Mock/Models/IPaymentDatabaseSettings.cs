namespace Checkout.AcquiringBank.Mock.Models
{
    public interface IPaymentDatabaseSettings
    {
        string PaymentsCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
