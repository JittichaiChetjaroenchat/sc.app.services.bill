using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.Tag;
using SC.App.Services.Bill.Business.Queries.Tag;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : BaseController
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetTagResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetTagById(id);
            var query = new GetTagByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<List<GetTagResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByFilterAsync([FromQuery] Guid channel_id)
        {
            var payload = new GetTagByFilter(channel_id);
            var query = new GetTagByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateTagResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTag payload)
        {
            var command = new CreateTagCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateTagResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateTag payload)
        {
            payload.Id = id;
            var command = new UpdateTagCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpDelete("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DeleteTagByIdResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var command = new DeleteTagByIdCommand(HttpContext.Request, new DeleteTagById(id));
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpDelete]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DeleteTagByIdsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteByIdsAsync([FromQuery] Guid[] id)
        {
            var payload = new DeleteTagByIds(id);
            var command = new DeleteTagByIdsCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}