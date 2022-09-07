using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.BillTag
{
    public class UpdateBillTag
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("tag_ids")]
        public List<Guid> TagIds { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class UpdateBillTagCommand : BaseCommand, IRequest<Response<UpdateBillTagResponse>>
    {
        public UpdateBillTag Payload { get; private set; }

        public UpdateBillTagCommand(HttpRequest request, UpdateBillTag payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}