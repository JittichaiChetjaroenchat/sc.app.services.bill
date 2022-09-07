using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class DepositBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("is_deposit")]
        public bool IsDeposit { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class DepositBillCommand : BaseCommand, IRequest<Response<DepositBillResponse>>
    {
        public DepositBill Payload { get; set; }

        public DepositBillCommand(HttpRequest request, DepositBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}