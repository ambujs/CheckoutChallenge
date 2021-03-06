using System.Threading.Tasks;
using Checkout.AcquiringBank.Mock.Models;
using Checkout.AcquiringBank.Mock.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Checkout.AcquiringBank.Mock.Controllers
{
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
        public async Task<ActionResult> Process([FromBody] PaymentRequest payment)
        {
            var response = await _paymentHandler.Process(payment);
            if (response.Successful)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
