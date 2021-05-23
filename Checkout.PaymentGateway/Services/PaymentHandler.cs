using System.Threading.Tasks;
using AutoMapper;
using Checkout.PaymentGateway.HttpClientServices;
using Checkout.PaymentGateway.Models;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Services
{
    public class PaymentHandler : IPaymentHandler
    {
        private readonly IAcquiringBankClient _acquiringBankClient;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentHandler(IAcquiringBankClient acquiringBankClient, IPaymentRepository paymentRepository, IMapper mapper)
        {
            _acquiringBankClient = acquiringBankClient;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        public async Task<ActionResult<PaymentResponse>> Process(Payment payment)
        {
            var bankResponse = await _acquiringBankClient.ProcessPayment(payment);

            var acquirerResponse = ((AcquirerResponse)((ObjectResult)bankResponse.Result).Value);

            var mongoPayment = _mapper.Map<Models.Mongo.Payment>(payment);
            mongoPayment.PaymentStatus = new PaymentStatus
            {
                Successful = acquirerResponse.Successful,
                ErrorCode = acquirerResponse.ErrorCode
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
