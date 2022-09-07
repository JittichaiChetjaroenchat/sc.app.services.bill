using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class DoneBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class DoneBillCommand : BaseCommand, IRequest<Response<DoneBillResponse>>
    {
        public DoneBill Payload { get; set; }

        public DoneBillCommand(HttpRequest request, DoneBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}