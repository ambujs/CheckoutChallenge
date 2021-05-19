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
        public async Task<ActionResult> Process([FromBody] Payment payment)
        {
            Log.Logger.Information("Here..");   // NOTE: personal preference here to use a static logger over injecting an ILogger

            var response = await _paymentHandler.Process(payment);
            return Ok(response);
        }

        [HttpGet]
        [Route("/api/payment/{paymentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get the details of a payment")]
        public async Task<ActionResult> Retrieve(string paymentId)
        {
            var payment = await _paymentHandler.Retrieve(paymentId);

            if (payment != null)
            {
                return Ok(payment);
            }

            return NotFound($"Payment with id {paymentId} not found");
        }
    }
}
