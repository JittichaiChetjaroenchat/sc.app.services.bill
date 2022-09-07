using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SC.App.Services.Bill.Business.Commands.Bill;
using SC.App.Services.Bill.Business.Enums;
using SC.App.Services.Bill.Business.Queries.Bill;
using SC.App.Services.Bill.Common.Responses;
using SC.App.Services.Bill.Filters;
using SC.App.Services.Bill.Lib.Helpers;

namespace SC.App.Services.Bill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillsController : BaseController
    {
        private readonly IMediator _mediator;

        public BillsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [CustomAuthorizeFilter]
        [HttpGet("{id}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var payload = new GetBillById(id);
            var query = new GetBillByIdQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("latest")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetLatestByFilterAsync([FromQuery] Guid channel_id, [FromQuery] Guid customer_id)
        {
            var payload = new GetLatestBillByFilter(channel_id, customer_id);
            var query = new GetLatestBillByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("latest/manifest")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<List<GetBillManifestResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetLatestManifestByFilterAsync([FromQuery] Guid live_id)
        {
            var payload = new GetLatestBillManifestByFilter(live_id);
            var query = new GetLatestBillManifestByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("search")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<SearchBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> SearchByFilterAsync([FromQuery] Guid channel_id, [FromQuery] EnumSearchBillStatus status, [FromQuery] EnumPeriod period, [FromQuery] DateTime date, [FromQuery] string keyword, [FromQuery] string sort_by, [FromQuery] bool sort_desc, [FromQuery] int page, [FromQuery] int page_size)
        {
            var payload = new SearchBillByFilter(channel_id, status, period, date, keyword, sort_by, sort_desc, page, page_size);
            var query = new SearchBillByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpGet("summary")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<GetBillSummaryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetSummaryByFilterAsync([FromQuery] Guid channel_id, [FromQuery] EnumPeriod period, [FromQuery] DateTime date)
        {
            var payload = new GetBillSummaryByFilter(channel_id, period, date);
            var query = new GetBillSummaryByFilterQuery(HttpContext.Request, payload);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPost("offline")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateBillResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOfflineAsync([FromBody] CreateOfflineBill payload)
        {
            var command = new CreateOfflineBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }

        [CustomAuthorizeFilter]
        [HttpPost("online")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CreateBillResponse>), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateOnlineAsync([FromBody] CreateOnlineBill payload)
        {
            var command = new CreateOnlineBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            if (result.Errors.Count > 0)
            {
                return Ok(result);
            }

            var location = UriHelper.BuildLocation(Request, result.Data.Id);
            return Created(location, result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("offline")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<UpdateBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateOfflineAsync([FromBody] UpdateOfflineBill payload)
        {
            var command = new UpdateOfflineBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("{id}/deposit")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DepositBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DepositAsync(Guid id, [FromBody] DepositBill payload)
        {
            payload.Id = id;
            var command = new DepositBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("confirm")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<ConfirmBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ConfirmAsync([FromBody] ConfirmBill payload)
        {
            var command = new ConfirmBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("cancel")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CancelBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelAsync([FromBody] CancelBill payload)
        {
            var command = new CancelBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("cancels")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<CancelBillsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CancelsAsync([FromBody] CancelBills payload)
        {
            var command = new CancelBillsCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("renew")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<RenewBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RenewAsync([FromBody] RenewBill payload)
        {
            var command = new RenewBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("renews")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<RenewBillsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> RenewsAsync([FromBody] RenewBills payload)
        {
            var command = new RenewBillsCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("done")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<DoneBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DoneAsync([FromBody] DoneBill payload)
        {
            var command = new DoneBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("archive")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<ArchiveBillResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ArchiveAsync([FromBody] ArchiveBill payload)
        {
            var command = new ArchiveBillCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [CustomAuthorizeFilter]
        [HttpPut("archives")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(Response<ArchiveBillsResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ArchivesAsync([FromBody] ArchiveBills payload)
        {
            var command = new ArchiveBillsCommand(HttpContext.Request, payload);
            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}