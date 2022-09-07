using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Payment
{
    public class GetPaymentByFilter
    {
        [JsonProperty("bill_id")]
        public Guid BillId { get; set; }

        public GetPaymentByFilter(Guid billId)
        {
            BillId = billId;
        }
    }

    public class GetPaymentByFilterQuery : BaseQuery, IRequest<Response<List<GetPaymentResponse>>>
    {
        public GetPaymentByFilter Payload { get; private set; }

        public GetPaymentByFilterQuery(HttpRequest request, GetPaymentByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}