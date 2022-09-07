using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Payment
{
    public class RejectPayment
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class RejectPaymentCommand : BaseCommand, IRequest<Response<RejectPaymentResponse>>
    {
        public RejectPayment Payload { get; set; }

        public RejectPaymentCommand(HttpRequest request, RejectPayment payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}