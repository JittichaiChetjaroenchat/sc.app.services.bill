using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.Parcel;
using SC.App.Services.Bill.Business.Queries.Parcel;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParcelsController : BaseController
    {
        private readonly IMediator _mediator;

        public ParcelsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetParcelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetParcelById(id);
            var query = new GetParcelByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<List<GetParcelResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByFilterAsync([FromQuery] Guid? bill_id, [FromQuery] Guid[] id)
        {
            var payload = new GetParcelByFilter(bill_id, id);
            var query = new GetParcelByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateParcelResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateParcel payload)
        {
            var command = new CreateParcelCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }

        [CustomAuthorizeFilter]
        [HttpPost("bulk")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateParcelsResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreatesAsync([FromBody] CreateParcels payload)
        {
            var command = new CreateParcelsCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateParcelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateParcel payload)
        {
            var command = new UpdateParcelCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateParcelPrintedResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdatePrintedAsync(Guid id)
        {
            var payload = new UpdateParcelPrinted(id);
            var command = new UpdateParcelPrintedCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("{id}/cancel")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CancelParcelResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] CancelParcel payload)
        {
            var command = new CancelParcelCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}