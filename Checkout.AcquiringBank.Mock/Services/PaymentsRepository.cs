using System;
using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Checkout.AcquiringBank.Mock.Services
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IMongoCollection<Payment> _payments;
        private readonly IConfiguration _configuration;

        public PaymentsRepository(IPaymentDatabaseSettings paymentDatabaseSettings, IConfiguration configuration)
        {
            _configuration = configuration;

            var client = new MongoClient(paymentDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(paymentDatabaseSettings.DatabaseName);

            _payments = database.GetCollection<Payment>(paymentDatabaseSettings.PaymentsCollectionName);
        }

        public async Task<string> SavePayment(Payment payment)
        {
            payment.Id = Guid.NewGuid().ToString();

            payment.CardNumber = payment.CardNumber.Encrypt(_configuration["encryptionKey"]);
            payment.PaymentStatus.UpdatedAt = DateTimeOffset.UtcNow;
            
            await _payments.InsertOneAsync(payment);
            return payment.Id;
        }

        public async Task<Payment> GetPayment(string id)
        {
            var payment = await _payments.Find(_ => _.Id == id).SingleOrDefaultAsync();

            if (payment == null) return null;

            var cardNumber = payment.CardNumber;
            cardNumber = cardNumber.Decrypt(_configuration["encryptionKey"]);

            var length = cardNumber.Length;
            payment.CardNumber = new string('X', length - 4) + cardNumber[(length - 4)..];

            return payment;

        }
    }
}
