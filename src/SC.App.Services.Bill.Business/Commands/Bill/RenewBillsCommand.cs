using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class RenewBills
    {
        [JsonProperty("ids")]
        public Guid[] Ids { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class RenewBillsCommand : BaseCommand, IRequest<Response<RenewBillsResponse>>
    {
        public RenewBills Payload { get; set; }

        public RenewBillsCommand(HttpRequest request, RenewBills payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}