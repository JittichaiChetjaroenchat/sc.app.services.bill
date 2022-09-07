using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.BillConfiguration;
using SC.App.Services.Bill.Business.Queries.BillConfiguration;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillConfigurationsController : BaseController
    {
        private readonly IMediator _mediator;

        public BillConfigurationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillConfigurationResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByFilterAsync([FromQuery] Guid channel_id)
        {
            var query = new GetBillConfigurationByFilterQuery(HttpContext.Request, new GetBillConfigurationByFilter(channel_id));
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateBillConfigurationResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateBillConfiguration payload)
        {
            var command = new CreateBillConfigurationCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }
    }
}