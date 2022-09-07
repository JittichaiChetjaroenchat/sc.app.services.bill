using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Payment
{
    public class OpenPayment
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class OpenPaymentCommand : BaseCommand, IRequest<Response<OpenPaymentResponse>>
    {
        public OpenPayment Payload { get; set; }

        public OpenPaymentCommand(HttpRequest request, OpenPayment payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}