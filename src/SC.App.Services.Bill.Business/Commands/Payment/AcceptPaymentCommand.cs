using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Payment
{
    public class AcceptPayment
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class AcceptPaymentCommand : BaseCommand, IRequest<Response<AcceptPaymentResponse>>
    {
        public AcceptPayment Payload { get; set; }

        public AcceptPaymentCommand(HttpRequest request, AcceptPayment payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}