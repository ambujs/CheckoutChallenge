using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Checkout.PaymentGateway.Models;
using FluentAssertions;
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

        [Fact]
        public void ValidateModel_WhenCardNumberIsShort_ReturnsError()
        {
            var payment = new Payment
            {
                Amount = 100,
                CardNumber = "1234"
            };

            Assert.Contains(ValidateModel(payment), _ => _.ErrorMessage.Contains("Credit card numbers must be numbers only and 15 or 16 digits"));
        }

        [Fact]
        public void ValidateModel_WhenValid_ReturnsNoError()
        {
            var payment = new Payment
            {
                Amount = 100,
                CardNumber = "1234123412341234",
                CVV = "013",
                Currency = Currency.GBP,
                ExpiryDate = new ExpiryDate
                {
                    Month = 12,
                    Year = 2023
                }
            };

            ValidateModel(payment).ToList().Count.Should().Be(0);
        }

        private static IEnumerable<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, true);
            return validationResults;
        }
    }
}
