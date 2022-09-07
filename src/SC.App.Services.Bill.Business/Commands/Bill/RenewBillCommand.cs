using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class RenewBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class RenewBillCommand : BaseCommand, IRequest<Response<RenewBillResponse>>
    {
        public RenewBill Payload { get; set; }

        public RenewBillCommand(HttpRequest request, RenewBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}