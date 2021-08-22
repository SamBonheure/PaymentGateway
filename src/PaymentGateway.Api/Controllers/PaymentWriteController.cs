using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Commands;
using PaymentGateway.Api.Responses;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentWriteController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentWriteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Make a payment to a merchant
        /// </summary>
        /// <param name="command">the payment being sent</param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(201, Type = typeof(CreatePaymentResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentCommand command)
        {
            command.MerchantId = Guid.Parse(User.FindFirstValue("MerchantId"));
            var result = await _mediator.Send(command);

            //TODO: refactor this into a bool and map
            if (result.Status.Contains("failed"))
                return BadRequest(result.Status);
            else
                return CreatedAtAction("MakePayment", new { paymentId = result.PaymentId }, result);
        }
    }
}
