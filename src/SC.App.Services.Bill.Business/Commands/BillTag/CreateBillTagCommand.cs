using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillTag
{
    public class CreateBillTag
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("tag_id")]
        public Guid TagId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CreateBillTagCommand : BaseCommand, IRequest<Response<CreateBillTagResponse>>
    {
        public CreateBillTag Payload { get; private set; }

        public CreateBillTagCommand(HttpRequest request, CreateBillTag payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}