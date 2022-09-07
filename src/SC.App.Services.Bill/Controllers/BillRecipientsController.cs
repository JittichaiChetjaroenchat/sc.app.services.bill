using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.BillRecipient;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillRecipientsController : BaseController
    {
        private readonly IMediator _mediator;

        public BillRecipientsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateBillRecipientResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateBillRecipient payload)
        {
            payload.Id = id;

            var command = new UpdateBillRecipientCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}