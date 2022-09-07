﻿using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Commands.Bill
{
    public class CancelBill
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
    }

    public class CancelBillCommand : BaseCommand, IRequest<Response<CancelBillResponse>>
    {
        public CancelBill Payload { get; set; }

        public CancelBillCommand(HttpRequest request, CancelBill payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}