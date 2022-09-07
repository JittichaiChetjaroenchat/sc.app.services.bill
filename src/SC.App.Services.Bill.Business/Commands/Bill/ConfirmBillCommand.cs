using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class ConfirmBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class ConfirmBillCommand : BaseCommand, IRequest<Response<ConfirmBillResponse>>
    {
        public ConfirmBill Payload { get; set; }

        public ConfirmBillCommand(HttpRequest request, ConfirmBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}