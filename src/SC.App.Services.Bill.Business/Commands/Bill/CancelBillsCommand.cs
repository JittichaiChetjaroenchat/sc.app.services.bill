using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class CancelBills
    {
        [JsonProperty("ids")]
        public Guid[] Ids { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CancelBillsCommand : BaseCommand, IRequest<Response<CancelBillsResponse>>
    {
        public CancelBills Payload { get; set; }

        public CancelBillsCommand(HttpRequest request, CancelBills payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}