using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.PaymentVerification;
using SC.App.Services.Bill.Business.Queries.PaymentVerification;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentVerificationsController : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentVerificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetPaymentVerificationResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetPaymentVerificationById(id);
            var query = new GetPaymentVerificationByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost("{id}/verify")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetPaymentVerificationResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> VerifyAsync(Guid id)
        {
            var payload = new VerifyPayment(id);
            var query = new VerifyPaymentCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}