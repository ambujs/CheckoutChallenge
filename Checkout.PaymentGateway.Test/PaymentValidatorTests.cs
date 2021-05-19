using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Checkout.PaymentGateway.Models;
using Xunit;

namespace Checkout.PaymentGateway.Test
{
    public class PaymentValidatorTests
    {
        [Fact]
        public void ValidateModel_WhenAmountIsZero_ReturnsError()
        {
            var payment = new Payment
            {
                Amount = 0
            };

            Assert.Contains(ValidateModel(payment), _ => _.ErrorMessage.Contains("The field Amount must be between 0.01 and 9999.99."));
        }

        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
