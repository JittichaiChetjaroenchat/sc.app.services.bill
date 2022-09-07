using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.Payment;
using SC.App.Services.Bill.Business.Queries.Payment;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetPaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetPaymentById(id);
            var query = new GetPaymentByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<List<GetPaymentResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByFilterAsync([FromQuery] Guid bill_id)
        {
            var payload = new GetPaymentByFilter(bill_id);
            var query = new GetPaymentByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("open")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<OpenPaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> OpenAsync([FromBody] OpenPayment payload)
        {
            var command = new OpenPaymentCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("reject")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<RejectPaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RejectAsync([FromBody] RejectPayment payload)
        {
            var command = new RejectPaymentCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("accept")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<AcceptPaymentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AcceptAsync([FromBody] AcceptPayment payload)
        {
            var command = new AcceptPaymentCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}