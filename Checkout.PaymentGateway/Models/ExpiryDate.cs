using System.ComponentModel.DataAnnotations;

namespace Checkout.PaymentGateway.Models
{
    public class ExpiryDate
    {
        [Required]
        [Range(2021, 2031)]                  // TODO: make it more dynamic
        public int Year { get; set; }

        [Required]
        [Range(1, 12)]
        public int Month { get; set; }
    }
}
