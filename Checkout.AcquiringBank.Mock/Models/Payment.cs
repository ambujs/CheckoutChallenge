using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Checkout.AcquiringBank.Mock.Models
{
    public class Payment : PaymentRequest
    {
        [BsonId]
        public string Id { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; }
    }
}
