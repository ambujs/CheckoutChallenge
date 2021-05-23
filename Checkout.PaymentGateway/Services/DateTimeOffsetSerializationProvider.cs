using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Checkout.PaymentGateway.Services
{
    public class DateTimeOffsetSerializationProvider : IBsonSerializationProvider
    {
        public IBsonSerializer GetSerializer(Type type)
        {
            if (type == typeof(DateTimeOffset)) return new DateTimeOffsetSerializer(BsonType.String);

            return null; // falls back to Mongo defaults
        }
    }
}
