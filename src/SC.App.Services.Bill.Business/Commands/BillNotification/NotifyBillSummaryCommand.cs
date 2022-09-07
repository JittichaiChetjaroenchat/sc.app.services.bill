using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillNotification
{
    public class NotifyBillSummary
    {
        [JsonProperty("id")]
        public Guid[] Id { get; set; }
    }

    public class NotifyBillSummaryCommand : BaseCommand, IRequest<Response<NotifyBillSummaryResponse>>
    {
        public NotifyBillSummary Payload { get; set; }

        public NotifyBillSummaryCommand(HttpRequest request, NotifyBillSummary payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}