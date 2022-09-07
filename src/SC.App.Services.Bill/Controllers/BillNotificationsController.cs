using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.BillNotification;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Queries.BillNotification;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillNotificationsController : BaseController
    {
        private readonly IMediator _mediator;

        public BillNotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillNotificationResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetBillNotificationById(id);
            var query = new GetBillNotificationByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<SearchBillNotificationResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchByFilterAsync([FromQuery] Guid channel_id, [FromQuery] EnumSearchBillNotificationStatus status, [FromQuery] EnumPeriod period, [FromQuery] DateTime date, [FromQuery] string keyword, [FromQuery] string sort_by, [FromQuery] bool sort_desc, [FromQuery] int page, [FromQuery] int page_size)
        {
            var payload = new SearchBillNotificationByFilter(channel_id, status, period, date, keyword, sort_by, sort_desc, page, page_size);
            var query = new SearchBillNotificationByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("summary")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillNotificationSummaryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSummaryByFilterAsync([FromQuery] Guid channel_id, [FromQuery] EnumPeriod period, [FromQuery] DateTime date)
        {
            var payload = new GetBillNotificationSummaryByFilter(channel_id, period, date);
            var query = new GetBillNotificationSummaryByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost("notify/summary")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<NotifyBillSummaryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> NotifySummaryAsync([FromBody] NotifyBillSummary payload)
        {
            var query = new NotifyBillSummaryCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}