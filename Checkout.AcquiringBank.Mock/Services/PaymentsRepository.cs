using System;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;
using MongoDB.Driver;

namespace Checkout.AcquiringBank.Mock.Services
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IMongoCollection<Payment> _payments;

        public PaymentsRepository(IPaymentDatabaseSettings paymentDatabaseSettings)
        {
            var client = new MongoClient(paymentDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(paymentDatabaseSettings.DatabaseName);

            _payments = database.GetCollection<Payment>(paymentDatabaseSettings.PaymentsCollectionName);
        }

        public async Task<string> SavePayment(Payment payment)
        {
            payment.Id = Guid.NewGuid().ToString();
            payment.PaymentStatus.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _payments.InsertOneAsync(payment);
            return payment.Id;
        }

        public async Task<Payment> GetPayment(string id)
        {
            var payment = await _payments.Find(_ => _.Id == id).SingleOrDefaultAsync();

            if (payment == null) return null;

            var length = payment.CardNumber.Length;
            payment.CardNumber = new string('X', length - 4) + payment.CardNumber[(length - 4)..];

            return payment;

        }
    }
}
