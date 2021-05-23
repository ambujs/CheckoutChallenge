using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Checkout.PaymentGateway.Models.Mongo
{
    public class Payment : Models.Payment
    {
        [BsonId]
        public string Id { get; set; }

        public PaymentStatus PaymentStatus { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
