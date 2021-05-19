using System;
using System.Threading.Tasks;
using AutoMapper;
using Checkout.AcquiringBank.Mock.Models;

namespace Checkout.AcquiringBank.Mock.Services
{
    public class PaymentHandler : IPaymentHandler
    {
        private readonly IMapper _mapper;
        private readonly IPaymentsRepository _paymentsRepository;
        public PaymentHandler(IPaymentsRepository paymentsRepository, IMapper mapper)
        {
            _paymentsRepository = paymentsRepository;
            _mapper = mapper;
        }

        public async Task<PaymentResponse> Process(PaymentRequest paymentRequest)
        {
            // Generate random response to replicate successful and failed payments
            var random = new Random();
            var successful = random.Next(0, 2) > 0;

            var paymentStatus = new PaymentStatus
            {
                Successful = successful,
                ErrorCode = successful
                    ? null
                    : "10001" // TODO: in real world this could be from a list of possible error codes
            };

            var payment = _mapper.Map<Payment>(paymentRequest);
            // Save to db with status
            payment.PaymentStatus = paymentStatus;
            
            var id = await _paymentsRepository.SavePayment(payment);

            var paymentResponse = new PaymentResponse
            {
                PaymentId = successful ? id : null,
                PaymentStatus = paymentStatus
            };
            
            return await Task.FromResult(paymentResponse);
        }

        public async Task<Payment> Retrieve(string paymentId)
        {
            var payment = await _paymentsRepository.GetPayment(paymentId);
            return await Task.FromResult(payment);
        }
    }
}
