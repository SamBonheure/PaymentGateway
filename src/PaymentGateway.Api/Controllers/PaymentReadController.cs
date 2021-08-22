using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Queries;
using PaymentGateway.Api.Responses;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("Payments")]
    public class PaymentReadController : ControllerBase
    {
        private readonly IMediator _mediator;
        public PaymentReadController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Request details of a previous payment
        /// </summary>
        /// <param name="query">the payment reqiest</param>
        /// <returns>A payment</returns>
        [HttpGet("{Id}")]
        [Produces("application/json")]
        [ProducesResponseType(200, Type = typeof(GetPaymentResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetPayment([FromRoute] GetPaymentQuery query)
        {
            query.MerchantId = Guid.Parse(User.FindFirstValue("MerchantId"));
            var result = await _mediator.Send(query);

            if (result is null)
                return NotFound($"Payment with id {query.Id} not found");

            return Ok(result);
        }
    }
}

