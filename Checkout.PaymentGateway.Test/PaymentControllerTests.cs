using System;
using Checkout.PaymentGateway.Controllers;
using Checkout.PaymentGateway.Models;
using Checkout.PaymentGateway.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Checkout.PaymentGateway.Test
{
    public class PaymentControllerTests
    {
        [Fact]
        public async void Retrieve_WhenPaymentExists_ReturnsPayment()
        {
            // Arrange
            var paymentId = Guid.NewGuid().ToString();
            var paymentHandlerMock = new Mock<IPaymentHandler>();
            paymentHandlerMock.Setup(_ => _.Retrieve(paymentId)).ReturnsAsync(new OkObjectResult(new Payment()));
            
            var sut = new PaymentController(paymentHandlerMock.Object);

            // Act
            var paymentResponse = await sut.Retrieve(paymentId);

            // Assert
            paymentResponse.Should().NotBeNull();
            ((ObjectResult)paymentResponse.Result).StatusCode.Should().Be(StatusCodes.Status200OK);
            var payment = ((ObjectResult)paymentResponse.Result).Value as Payment;
            payment.Should().NotBeNull();
        }

        [Fact]
        public async void Retrieve_WhenPaymentNotExists_ReturnsNotFound()
        {
            // Arrange
            var paymentId = Guid.NewGuid().ToString();
            var paymentHandlerMock = new Mock<IPaymentHandler>();
            paymentHandlerMock.Setup(_ => _.Retrieve(paymentId)).ReturnsAsync(new NotFoundResult());

            var sut = new PaymentController(paymentHandlerMock.Object);

            // Act
            var paymentResponse = await sut.Retrieve(paymentId);

            // Assert
            paymentResponse.Should().NotBeNull();
            ((StatusCodeResult)paymentResponse.Result).StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }
    }
}
