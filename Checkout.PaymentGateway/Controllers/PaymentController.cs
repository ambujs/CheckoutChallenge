using System.Threading.Tasks;
using Checkout.PaymentGateway.Attributes;
using Checkout.PaymentGateway.Models;
using Checkout.PaymentGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;

namespace Checkout.PaymentGateway.Controllers
{
    [BasicAuthorization]
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentHandler _paymentHandler;

        public PaymentController(IPaymentHandler paymentHandler)
        {
            _paymentHandler = paymentHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Processes a payment")]
        public async Task<ActionResult<PaymentResponse>> Process([FromBody] Payment payment)
        {
            Log.Logger.Information("Process payment request received...");   // NOTE: personal preference here to use a static logger over injecting an ILogger

            var response = await _paymentHandler.Process(payment);

            if (response.Value.PaymentStatus.Successful)
            {
                return Ok(response.Value);
            }

            return BadRequest(response.Value);
        }

        [HttpGet]
        [Route("/api/payment/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get the details of a payment")]
        public async Task<ActionResult<Payment>> Retrieve(string paymentId)
        {
            var payment = await _paymentHandler.Retrieve(paymentId);
            return payment;
        }
    }
}
