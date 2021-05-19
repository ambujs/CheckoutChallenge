using System.Runtime.Serialization;
using Checkout.PaymentGateway.Attributes;
using Newtonsoft.Json;

namespace Checkout.PaymentGateway.Models
{
    [JsonConverter(typeof(CustomStringToEnumConverter))]
    public enum Currency
    {
        [EnumMember(Value = "GBP")]
        GBP,
        [EnumMember(Value = "EUR")]
        EUR,
        [EnumMember(Value = "USD")]
        USD
    }
}
