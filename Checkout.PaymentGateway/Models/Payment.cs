using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Checkout.PaymentGateway.Models
{
    public class Payment
    {

        [Required]
        [RegularExpression("^[0-9]{15,16}$", ErrorMessage = "Credit card numbers must be numbers only and 15 or 16 digits")]
        public string CardNumber { get; set; }  // TODO: do a more comprehensive regex validation or don't do any regex at all?

        [Required]
        [Range(0.01, 9999.99)]                  // NOTE: arbitrary max amount
        public decimal Amount { get; set; }

        [BsonRepresentation(BsonType.String)]
        [Required(ErrorMessage = "Currency not supported")]
        public Currency? Currency { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "CVV must be 3 or 4 characters long and should only be numbers")]
        public string CVV { get; set; }

        [Required]
        public ExpiryDate ExpiryDate { get; set; }
    }
}
