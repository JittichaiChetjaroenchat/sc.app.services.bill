using System;
using MediatR;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SC.App.Services.Bill.Common.Requests;
using SC.App.Services.Bill.Common.Responses;

namespace SC.App.Services.Bill.Business.Queries.Bill
{
    public class GetLatestBillByFilter
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("customer_id")]
        public Guid CustomerId { get; set; }

        public GetLatestBillByFilter(Guid channelId, Guid customerId)
        {
            ChannelId = channelId;
            CustomerId = customerId;
        }
    }

    public class GetLatestBillByFilterQuery : BaseQuery, IRequest<Response<GetBillResponse>>
    {
        public GetLatestBillByFilter Payload { get; private set; }

        public GetLatestBillByFilterQuery(HttpRequest request, GetLatestBillByFilter payload)
            : base(request)
        {
            Payload = payload;
        }
    }
}