using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.BillTag;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillTagsController : BaseController
    {
        private readonly IMediator _mediator;

        public BillTagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateBillTagResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBillTag payload)
        {
            var command = new CreateBillTagCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }

        [CustomAuthorizeFilter]
        [HttpPut]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateBillTagResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateBillTag payload)
        {
            var command = new UpdateBillTagCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DeleteBillTagResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var payload = new DeleteBillTag(id);
            var command = new DeleteBillTagCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}