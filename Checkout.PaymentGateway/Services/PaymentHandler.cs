using System;
using System.Threading.Tasks;
using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentHandler : IPaymentHandler
    {
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentHandler(IAcquiringBankClient acquiringBankClient, IPaymentRepository paymentRepository)
        {
            _acquiringBankClient = acquiringBankClient;
            _paymentRepository = paymentRepository;
        }

        public async Task<ActionResult<PaymentResponse>> Process(Payment payment)
        {
            var bankResponse = await _acquiringBankClient.ProcessPayment(payment);

            var acquirerResponse = ((AcquirerResponse) ((ObjectResult) bankResponse.Result).Value);

            var mongoPayment = new Models.Mongo.Payment
            {
                Amount = payment.Amount,
                PaymentStatus = new PaymentStatus
                {
                    Successful = acquirerResponse.Successful,
                    ErrorCode = acquirerResponse.ErrorCode
                },
                CardNumber = payment.CardNumber,
                Currency = payment.Currency,
                CVV = payment.CVV,
                ExpiryDate = payment.ExpiryDate
            };

            var id = await _paymentRepository.SavePayment(mongoPayment);

            var response = new PaymentResponse
            {
                PaymentId = id,
                PaymentStatus = new PaymentStatus
                {
                    Successful = acquirerResponse.Successful,
                    ErrorCode = acquirerResponse.ErrorCode
                }
            };

            return await Task.FromResult(response);
        }

        public async Task<ActionResult<Payment>> Retrieve(string paymentId)
        {
            var payment = await _paymentRepository.GetPayment(paymentId);
            return await Task.FromResult(payment);
        }
    }
}
